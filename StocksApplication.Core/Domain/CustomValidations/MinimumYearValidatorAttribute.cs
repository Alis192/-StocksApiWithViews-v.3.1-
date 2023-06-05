using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ServiceContracts.CustomValidators
{
    public class MinimumYearValidatorAttribute : ValidationAttribute
    {
        public string MinimumYear { get; set; }
        public string DefaultErrorMessage { get; set; } = "The year shouldn't be older than {0}"; //here 0 represents MinimumYear which is mentioned inside " return new ValidationResult(string.Format(ErrorMessage ? DefaultErrorMessage, MinimumYear));"

        //parameterless constructor
        public MinimumYearValidatorAttribute() { }

        //parametrized constructor
        public MinimumYearValidatorAttribute(string minYear)
        {
            MinimumYear= minYear;
            
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value != null)
            {
                DateTime date = Convert.ToDateTime(value);
                DateTime input = Convert.ToDateTime(MinimumYear);
                if (date < input)
                {
                    return new ValidationResult(string.Format(ErrorMessage ?? DefaultErrorMessage, MinimumYear));
                } else
                {
                    return ValidationResult.Success;
                }
            } 
            return null;
            
        }

    }
}
