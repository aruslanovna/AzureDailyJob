using CurrencyExchange.Models;
using ExchangeRateKeeper;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FunctionApp1
{
    public class Function1
    {
        private readonly ILogger<Function1> _logger;
        static readonly HttpClient _httpClient = new HttpClient();
        private readonly CurrencyDbContext _dbContext;

        public Function1(ILogger<Function1> logger, CurrencyDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        [Function("Function1")]
        public async Task Run([TimerTrigger("* * 24 * * *")] TimerInfo myTimer)
        {
            try
            {
                string url = "https://data.fixer.io/api/latest?access_key=b0ecaea3c5e15c8163a12c7b4d27df15";
                var response = await _httpClient.GetStringAsync(url);

                if (string.IsNullOrWhiteSpace(response))
                {
                    _logger.LogError("API response was empty.");
                    throw new Exception("API response was empty.");
                }

                var currencyResponse = JsonConvert.DeserializeObject<CurrencyResponse>(response);

                if (currencyResponse == null || !currencyResponse.Success)
                {
                    _logger.LogError("API request failed or exceeded rate limits.");
                    throw new Exception("API request failed or exceeded rate limits.");
                }

                var exchangeRate = new List<ExchangeRate>();
                foreach (var rate in currencyResponse.Rates)
                {
                    exchangeRate.Add(new ExchangeRate() { TargetCurrency = rate.Key, Rate = rate.Value });
                }

                var newRate = new CurrencyRate
                {
                    CurrencySymbol = currencyResponse.Base,
                    ConversionDate = currencyResponse.Date,
                    Rates = exchangeRate
                };

                _dbContext.CurrencyRates.Add(newRate);
                await _dbContext.SaveChangesAsync();
                _logger.LogInformation("Daily currency update completed.");

            }
            catch (HttpRequestException httpEx)
            {
                _logger.LogError($"HTTP request error: {httpEx.Message}");
                _logger.LogError($"Inner exception: {httpEx.InnerException?.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"General error: {ex.Message}");
                _logger.LogError($"Stack Trace: {ex.StackTrace}");
            }
        }
    }
}
