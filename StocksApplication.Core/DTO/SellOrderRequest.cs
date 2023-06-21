using Entities;
using ServiceContracts.CustomValidators;
using System;
using System.ComponentModel.DataAnnotations;

namespace ServiceContracts.DTO
{
    public class SellOrderRequest
    {
        [Required(ErrorMessage = "Stock Symbol can't be blank")]
        public string? StockSymbol { get; set; }


        [Required(ErrorMessage = "Stock Name can't be blank")]
        public string? StockName { get; set; }


        [MinimumYearValidator("2000-01-01", ErrorMessage = "Minimum year should be 2000-01-01")]
        public DateTime? DateAndTimeOfOrder { get; set; }


        [Range(1, 100000, ErrorMessage = "{0} should be between ${1} and ${2}")]
        public uint? Quantity { get; set; }


        [Range(1, 10000, ErrorMessage = "{0} should be between ${1} and ${2}")]
        public double? Price { get; set; }

        public Guid UserId { get; set; }    
        
        public SellOrder ToSellOrder()
        {
            return new SellOrder
            {
                StockSymbol = StockSymbol,
                StockName = StockName,
                Price = Price,
                DateAndTimeOfOrder = DateAndTimeOfOrder,
                Quantity = Quantity,
                UserId= UserId
            };
        }
    }
}
