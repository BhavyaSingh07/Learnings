using Microsoft.AspNetCore.Mvc;

namespace IActionResultExample.Controllers
{
    public class StoreController : Controller
    {
        [Route("store/books/{id}")]
        public IActionResult Books()

        {
            int id = Convert.ToInt32(Request.RouteValues["id"]);
            return Content($"<h2>Bookstore {id}</h2>", "text/html");
        }
    }
}
