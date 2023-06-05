namespace StocksApiBasics.Models
{
    public class Stock
    {
        public string? StockSymbol { get; set; }
        public string? StockName { get; set; }

        public string? CompanyLogo { get; set; }
        public string? StockType { get; set; }
        public string? StockExchange { get; set; }
        public double? StockPrice { get; set; } = 210.3;
    }
}
