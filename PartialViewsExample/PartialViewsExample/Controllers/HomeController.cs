using Microsoft.AspNetCore.Mvc;
using PartialViewsExample.Models;

namespace PartialViewsExample.Controllers
{
    public class HomeController : Controller
    {
        [Route("/")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("about")]
        public IActionResult About()
        {
            return View();
        }

        [Route("proglang")]
        public IActionResult ProgrammingLanguages()
        {
            ListModel listModel = new ListModel()
            {
                ListTitle = "Programmin languages list",
                ListItems = new List<string>()
                {
                    "Python", "Swift", "Csharp", "Java"
                }
            };

            return PartialView("_ListPartialView", listModel);

        }
    }
}
