using System.ComponentModel.DataAnnotations;

namespace ModelvalidationsExample.CustomValidators
{
    public class MinimumYearValidatorAttribute : ValidationAttribute
    {
        public int minimumYear { get; set; } = 2000;
        public string DefaultErrorMessage { get; set; } = "Year should be below {0}";
        public MinimumYearValidatorAttribute() { }
        public MinimumYearValidatorAttribute(int minimumYear) {
            this.minimumYear = minimumYear;

        }
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value != null)
            {
                DateTime dt = (DateTime)value;
                if (dt.Year >= minimumYear)
                {
                    return new ValidationResult(string.Format(ErrorMessage ?? DefaultErrorMessage, minimumYear));
                }
                else
                {
                    return ValidationResult.Success;
                }
            }

            return null;    
        }
    }
}
