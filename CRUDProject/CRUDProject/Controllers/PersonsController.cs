using System.ComponentModel;
using Entities;
using Microsoft.AspNetCore.Mvc;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace CRUDProject.Controllers
{
    public class PersonsController : Controller
    {
        private readonly IPersonsService _personsService;
        private readonly IcountriesService _countriesService;   
        public PersonsController(IPersonsService personsService, IcountriesService countriesService)
        {
            _personsService = personsService;
            _countriesService = countriesService;
        }
        [Route("persons/index")]
        [Route("/")]
        public IActionResult Index(string searchBy, string? searchString, string sortBy = nameof(PersonResponse.PersonName), SortOrderOptions sortOrder = SortOrderOptions.ASC)
        {
            ViewBag.SearchFields = new Dictionary<string, string>()
            {
                {nameof(Person.PersonName), "Person Name" },
                {nameof(Person.Email), "Email" },
                {nameof(Person.DateOfBirth), "Date of Birth" },
                {nameof(Person.Gender), "Gender" },
                {nameof(Person.CountryId), "Country" },
                {nameof(Person.Address), "Address" }

            };
           // List<PersonResponse> persons = _personsService.GetAllPersons();
           List<PersonResponse> persons = _personsService.GetFilteredPersons(searchBy, searchString);   
            ViewBag.CurrentSearchBy = searchBy;
            ViewBag.CurrentSearchString = searchString;

            List<PersonResponse> sortedPersons = _personsService.GetSortedPersons(persons, sortBy, sortOrder);
            ViewBag.CurrentSortBy = sortBy;
            ViewBag.CurrentSortOrder = sortOrder.ToString();
            return View(sortedPersons);
        }

        [Route("persons/create")]
        [HttpGet]
        public IActionResult Create()
        {
            List<CountryResponse> countries = _countriesService.GetAllCountries();
            ViewBag.Countries = countries;
            return View();
        }

        [HttpPost]
        [Route("persons/create")]
        public IActionResult Create(PersonAddRequest personAddRequest)
        {
            if (!ModelState.IsValid)
            {
                List<CountryResponse> countries = _countriesService.GetAllCountries();
                ViewBag.Countries = countries;

                ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).SelectMany(e => e.ErrorMessage).ToList();
                return View();
            }

            PersonResponse personResponse = _personsService.AddPerson(personAddRequest);
            return RedirectToAction("Index", "Persons");
        }
    }
}
