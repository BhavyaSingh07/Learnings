using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ControllersExample.Models;

namespace ControllersExample.Controllers
{
    // [Controller]
    public class HomeController : Controller
    {
        [Route("home")]
        [Route("/")]
        public ContentResult Index()
        {
            //return new ContentResult()
            //{
            //    Content = "Hello from index",
            //    ContentType = "text/plain"
            //};

            // return Content("Hello from main body", "text/plain");

            return Content("<h1> Weclome</h1> <h2>Hello from index</h2>", "text/html");
        }

        [Route("person")]
        public JsonResult Person()
        {
            Person person = new Person()
            {
                Id = Guid.NewGuid(),
                firstName = "James",
                lastName = "smith",
                Age = 25
            };

            return Json(person);
            //return new JsonResult(person);
        }

        //[Route("contact/{mobile:regex(^\\d{{10}}$)}")]
        //public string Contact()
        //{
        //    return "Contact us";
        //}

        [Route("filedownload")]
        public VirtualFileResult Filedownload()
        {
            //return new VirtualFileResult("/sample.pdf", "application/pdf");
            return File("/sample.pdf", "application/pdf");
        }

        [Route("filedownload2")]
        public PhysicalFileResult Filedownload2()
        {
            return new PhysicalFileResult(@"C:\Users\BhavyaSingh\Downloads\sample.pdf", "application/pdf");
            //return PhysicalFile
        }

        [Route("filedownload3")]
        public FileContentResult Filedownload3()
        {
            byte[] bytes = System.IO.File.ReadAllBytes(@"C:\\Users\\BhavyaSingh\\Downloads\\sample.pdf");
            return new FileContentResult(bytes, "application/pdf");
            //return File(bytes, "application/pdf");
        }
    }
}
