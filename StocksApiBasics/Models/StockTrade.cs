using System.ComponentModel.DataAnnotations;

namespace StocksApiBasics.Models
{
    public class StockTrade
    {
        public string? StockSymbol { get; set; }

        public string? StockName { get; set; }

        [Range(1, 10000, ErrorMessage = "{0} should be between ${1} and ${2}")]
        public double Price { get; set; }

        [Range(1, 100000, ErrorMessage = "{0} should be between ${1} and ${2}")]
        public uint? Quantity { get; set; }
    }
}