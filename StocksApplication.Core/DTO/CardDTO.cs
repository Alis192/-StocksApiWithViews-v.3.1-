using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace StocksApplication.Core.DTO
{
    public class CardDTO
    {
        [Required]
        [CreditCard(ErrorMessage = "Please enter card number correctly")]
        public string? CardNumber { get; set; }


        [Required]
        [RegularExpression(@"^(0[1-9]|1[0-2])$", ErrorMessage = "Please enter a valid expiration month.")] //Year: XX
        public string? ExpirationMonth { get; set; }


        [Required]
        [RegularExpression(@"^(2[3-9]|[3-9][0-9])$", ErrorMessage = "Please enter a valid expiration year.")] //Month: 01 - 12 
        public string? ExpirationYear { get; set;}


        [Required]
        [RegularExpression(@"^[0-9]{3}$", ErrorMessage = "Please enter a valid CVC code.")]
        public string? Cvc { get; set; }


        [Required]
        [Range(1, 1000000, ErrorMessage = "Top-up amount should be between ${1} and ${2}")]
        public double? BalanceAmount { get; set; }


    }
}
