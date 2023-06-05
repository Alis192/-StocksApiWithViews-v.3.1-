using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Reflection;


namespace Services.Helpers
{
    public class ValidationHelpers
    {
        internal static void ModelValidation(object? obj)
        {
            ValidationContext validationContext = new ValidationContext(obj);//context object contains reference to model which has to be validated. We also need to supply model object which has to be validated    
            List<ValidationResult> validationResults= new List<ValidationResult>(); //in order to store list of validations
            bool isValid = Validator.TryValidateObject(obj, validationContext, validationResults, true);// it validates entire model object, (true means not only required should be validated but olse other fields as well
            if (!isValid)
            {
                throw new ArgumentException(validationResults.FirstOrDefault()?.ErrorMessage);
            }
        }
    }
}
