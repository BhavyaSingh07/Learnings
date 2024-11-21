using Microsoft.AspNetCore.Mvc;
using ModelvalidationsExample.CustomModelBinders;
using ModelvalidationsExample.Models; 

namespace ModelvalidationsExample.Controllers
{
    public class HomeController : Controller
    {
        [Route("register")]
        // [FromBody]
        //[ModelBinder(BinderType = typeof(PersonModelBinder))]
        //[Bind(nameof(Person.PersonName), nameof(Person.Email), nameof(Person.Password), nameof(Person.ConfirmPassword))]
        public IActionResult Index(Person person, [FromHeader(Name = "User-Agent")] string useragent)
        {
            if (!ModelState.IsValid)
            {
                List<string> errorsList = new List<string>();
                //foreach(var values in ModelState.Values)
                //{
                //    foreach(var error in values.Errors)
                //    {
                //        errorsList.Add(error.ErrorMessage);
                //    }
                //}
                string errors = string.Join("", ModelState.Values.SelectMany(value => value.Errors).SelectMany(err => err.ErrorMessage));
                //
                //string errors = string.Join("\n", errorsList);
                return BadRequest(errors);
                
            }

           // ControllerContext.HttpContext.Request.Headers["key"];
                return Content($"{person}, {useragent}");
            
        }
    }
}
