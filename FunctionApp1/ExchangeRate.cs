namespace CurrencyExchange.Models
{
    public class ExchangeRate
    {
        public int Id { get; set; }
        public int CurrencyRateId { get; set; }
        public string TargetCurrency { get; set; }
        public decimal Rate { get; set; }
        public CurrencyRate CurrencyRate { get; set; }
    }
}

