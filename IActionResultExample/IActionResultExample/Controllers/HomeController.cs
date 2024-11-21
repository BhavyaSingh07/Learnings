using Microsoft.AspNetCore.Mvc;
using static System.Formats.Asn1.AsnWriter;
using static System.Reflection.Metadata.BlobBuilder;
using IActionResultExample.Models;

namespace IActionResultExample.Controllers
{
    public class HomeController : Controller
    {
        [Route("bookstore/{bookid?}/{isloggedin?}")]
        public IActionResult Index([FromRoute]int? bookid, [FromRoute]bool? isloggedin, Book book)
        {
            if (bookid.HasValue==false)
            {
                //Response.StatusCode = 400;
                //return Content("Book id is not supplied");
                return BadRequest("Book id is not supplied");
            }
            //if (string.IsNullOrEmpty(Convert.ToString(Request.Query["bookid"])))
            //{
            //    return BadRequest("Book id cannot be empty");
            //}
            //int bookId = Convert.ToInt32(ControllerContext.HttpContext.Request.Query["bookid"]);
            if(bookid <= 0)
            {
                return BadRequest("Book id cannot be less than 0");
            }
            if(bookid > 1000)
            {
                return NotFound("Book id cannot be greater than 1000");
            }

            //if (Convert.ToBoolean(Request.Query["isloggedin"]) == false)
            if(isloggedin==false)
            {
                //Response.StatusCode = 401;
                return Unauthorized("User must be logged in");
            }


            //return File("/sample.pdf", "application/pdf");
            //return RedirectToAction("Books", "Store", new { id = bookId});
            //return new RedirectToActionResult("Books", "Store", new { }, true); 
            //return RedirectToActionPermanent("Books", "Store", new { id = bookId});

            //return new LocalRedirectResult("books/store/{id}", true);

            //return RedirectPermanent($"store/books/{bookid}");

            return Content($"Book id is: {bookid}, Book : {book} ", "text/plain");
            //return redirect

            // return new LocalRedirect("books/store/{id}");
        }
    }
}
