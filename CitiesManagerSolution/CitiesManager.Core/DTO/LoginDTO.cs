using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitiesManager.Core.DTO
{
    public class LoginDTO
    {
        [Required(ErrorMessage ="Email can't be blank")]
        public string Email { get; set; } = string.Empty;
        [Required(ErrorMessage = "Password has to be provied")]
        public string Password { get; set; } = string.Empty;
    }
}
