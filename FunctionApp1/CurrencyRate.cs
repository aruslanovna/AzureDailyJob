namespace CurrencyExchange.Models
{
    public class CurrencyRate
    {
        public int Id { get; set; }
        public string CurrencySymbol { get; set; }
        public DateTime? ConversionDate { get; set; }
        public List<ExchangeRate> Rates { get; set; } = new();
    }
}

