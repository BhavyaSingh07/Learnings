using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace CitiesManager.Core.DTO
{
    public class RegisterDTO
    {
        [Required(ErrorMessage = "Person Name can't be blank")]
        public string PersonName { get; set; } = string.Empty;
        [EmailAddress(ErrorMessage = "Email Can't be blank")]
        [Remote(action: "IsEmailAlreadyRegistered", controller: "Account", ErrorMessage = "Email already registered")]
        public string Email { get; set; } = string.Empty;
        [RegularExpression("^[0-9]*$", ErrorMessage = "Only Digits")]

        public string PhoneNumber { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
        [Compare("Password", ErrorMessage ="Password and confirm password do not match")]
        public string ConfirmPassword {  get; set; } = string.Empty;
    }
}
