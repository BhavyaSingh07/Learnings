using Microsoft.AspNetCore.Mvc;
using ViewsExample.Models;

namespace ViewsExample.Controllers
{
    public class HomeController : Controller
    {
        [Route("home")]
        [Route("/")]
        public IActionResult Index()
        {
            ViewData["appTitle"] = "asp.net Core application";

            
            List<Person> people = new List<Person>(){  
               new Person(){ Name = "John", DateOfBirth = DateTime.Parse("2002-07-08"), PersonGender=Gender.Male},
               new Person(){ Name = "Anya", DateOfBirth = DateTime.Parse("2008-02-08"), PersonGender=Gender.Female},
               new Person(){ Name = "Linda", DateOfBirth = DateTime.Parse("2006-04-03"), PersonGender=Gender.Other},
    }; //ViewData["people"] = people; //ViewBag.people = people;
                        return View(people);
            //return View("abc");//Views/Home/Index.cshtml
           // return new ViewResult() {  ViewName = "abc" };
        }

        [Route("person-details/{name}")]
        public IActionResult Details(string? name)
        {
            if(name == null)
            {
                return Content("Person name cannot be null");
            }

            List<Person> people = new List<Person>(){
               new Person(){ Name = "John", DateOfBirth = DateTime.Parse("2002-07-08"), PersonGender=Gender.Male},
               new Person(){ Name = "Anya", DateOfBirth = DateTime.Parse("2008-02-08"), PersonGender=Gender.Female},
               new Person(){ Name = "Linda", DateOfBirth = DateTime.Parse("2006-04-03"), PersonGender=Gender.Other},
            };

            Person? matchingPerson = people.Where(temp => temp.Name == name).FirstOrDefault();
            return View(matchingPerson);
        }

        [Route("person-with-product")]
        public IActionResult PersonWithProduct()
        {
            Person person = new Person { Name="Sarah", PersonGender=Gender.Female, DateOfBirth=Convert.ToDateTime("2004-06-07")};

            Product product = new Product() { ProductId = 1, ProductName = "Air conditioner" };

            PersonAndProductWrapperModel personandproductwrappermodel = new PersonAndProductWrapperModel()
            {
                PersonData = person,
                ProductData = product
            };

            return View(personandproductwrappermodel);

        }
    }
}
