using System;
using System.ComponentModel.DataAnnotations;
using Entities;
using ServiceContracts.CustomValidators;

namespace ServiceContracts.DTO
{
    public class BuyOrderRequest 
    {
        [Required(ErrorMessage = "Stock Symbol can't be blank")]
        public string? StockSymbol { get; set; }


        [Required(ErrorMessage = "Stock Name can't be blank")]
        public string? StockName { get; set; }

        [MinimumYearValidator("2000-01-01", ErrorMessage = "adasda")] //We create a custom validator
        public DateTime? DateAndTimeOfOrder { get; set; }


        [Range(1, 100000, ErrorMessage = "{0} should be between ${1} and ${2}")]
        public uint? Quantity { get; set; }


        [Range(1, 10000, ErrorMessage = "{0} should be between ${1} and ${2}")]
        public double? Price { get; set; } 


        /// <summary>
        /// After inputting values of BuyOrderRequest we convert it BuyOrder class which acts like database Model 
        /// </summary>
        /// <returns></returns>
        public BuyOrder ToBuyOrder()
        {
            return new BuyOrder
            {
                StockSymbol = StockSymbol,
                StockName = StockName,
                Price = Price,
                DateAndTimeOfOrder = DateAndTimeOfOrder,
                Quantity = Quantity
            };
        }
    }
}
