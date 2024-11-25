using System.ComponentModel;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace CRUDProject.Controllers
{
    [Route("[controller]")]
    public class PersonsController : Controller
    {
        private readonly IPersonsService _personsService;
        private readonly IcountriesService _countriesService;   
        public PersonsController(IPersonsService personsService, IcountriesService countriesService)
        {
            _personsService = personsService;
            _countriesService = countriesService;
        }
        //[Route("persons/index")]
        [Route("[action]")]
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

        //[Route("persons/create")]
        [Route("[action]")]
        [HttpGet]
        public IActionResult Create()
        {
            List<CountryResponse> countries = _countriesService.GetAllCountries();
            //ViewBag.Countries = countries;
            ViewBag.Countries = countries.Select(temp => new SelectListItem() { Text = temp.CountryName, Value = temp.CountryId.ToString() });
            return View();
        }

        [HttpPost]
        // [Route("persons/create")]
        [Route("[action]")]
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

        [HttpGet]
        [Route("[action]/{personID}")]
        public IActionResult Edit(Guid personID)
        {
            PersonResponse? personResponse = _personsService.GetPersonByPersonID(personID);
            if(personResponse == null)
            {
                return RedirectToAction("Index");
            }

            PersonUpdateRequest personUpdateRequest = personResponse.ToPersonUpdateRequest();

            List<CountryResponse> countries = _countriesService.GetAllCountries();
            //ViewBag.Countries = countries;
            ViewBag.Countries = countries.Select(temp => new SelectListItem() { Text = temp.CountryName, Value = temp.CountryId.ToString() });

            return View(personUpdateRequest);
        }

        [HttpPost]
        [Route("[action]/{personID}")]
        public IActionResult Edit(PersonUpdateRequest personUpdateRequest)
        {
            PersonResponse? personResponse = _personsService.GetPersonByPersonID(personUpdateRequest.PersonID);
            if(personResponse == null)
            {
                RedirectToAction("Index");
            }
            if(ModelState.IsValid)
            {
                PersonResponse updatePerson = _personsService.UpdatePerson(personUpdateRequest);
                return RedirectToAction("Index");
            }
            else
            {
                List<CountryResponse> countries = _countriesService.GetAllCountries();
                ViewBag.Countries = countries;

                ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).SelectMany(e => e.ErrorMessage).ToList();
                return View(personResponse.ToPersonUpdateRequest());
            }
           // return View();
        }

        [HttpGet]
        [Route("[action]/{personID}")]
        public IActionResult Delete(Guid personID)
        {
            PersonResponse? personResponse = _personsService.GetPersonByPersonID(personID);
            if(personResponse == null)
            {
                return RedirectToAction("Index");
            }
            return View(personResponse);
        }

        [HttpPost]
        [Route("[action]/{personID}")]
        public IActionResult Delete(PersonUpdateRequest personUpdateResult)
        {
            PersonResponse? personResponse = _personsService.GetPersonByPersonID(personUpdateResult.PersonID);
            if (personResponse == null)
            {
                return RedirectToAction("Index");
            }
            _personsService.DeletePerson(personUpdateResult.PersonID);
            return RedirectToAction("Index");
        }
        
    }
}
