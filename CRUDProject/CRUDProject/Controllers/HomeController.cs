using Microsoft.AspNetCore.Mvc;

namespace CRUDProject.Controllers
{
    public class HomeController : Controller
    {
        [Route("Error")]
        public IActionResult Error()
        {
            return View();
        }
    }
}
