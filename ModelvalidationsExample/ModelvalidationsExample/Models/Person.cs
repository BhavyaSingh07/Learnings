using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using ModelvalidationsExample.CustomValidators;

namespace ModelvalidationsExample.Models
{
    public class Person : IValidatableObject
    {
        [Required(ErrorMessage = "{0} is null or empty")]
        [Display(Name = "Person Name")]
        [StringLength(15, MinimumLength =4, ErrorMessage ="{0} is short or too long")]
        public string? PersonName {  get; set; }
        [EmailAddress(ErrorMessage ="Email address is not of correct format")]
        public string? Email { get; set; }
        [Phone(ErrorMessage ="Fill the correct {0}")]
        public string? Phone { get; set; }
        [Required]
        public string? Password { get; set; }
        [Required]
        [Compare("Password", ErrorMessage ="{0} is not the same as {1}")]
        public string? ConfirmPassword { get; set; }
        [Range(0, 99, ErrorMessage ="{0} is out of valid range, should be between {1} and {2}")]
        public double? Price { get; set; }
        [MinimumYearValidator(2005, ErrorMessage ="The year is above {0}")]
        [BindNever]
        public DateTime? DateofBirth { get; set; }
        public int? Age { get; set; }
        public DateTime? FromDate {  get; set; }
        [DateRangeValidator("FromDate", ErrorMessage="From date should be equal or older than the to date")]
        public DateTime? ToDate { get; set; }
        public List<string?> Tags { get; set; } = new List<string?>();

        public override string ToString()
        {
            return $"\tPerson Name : {PersonName}\n Email: {Email}\n Phone: {Phone}\n Password: {Password}\n Confirm: {ConfirmPassword}\n Price: {Price}";
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if(DateofBirth.HasValue==false && Age.HasValue == false)
            {
                yield return new ValidationResult("Either dob or age should be applied", new string[] { nameof(Age) });
            }
        }
    }
}
