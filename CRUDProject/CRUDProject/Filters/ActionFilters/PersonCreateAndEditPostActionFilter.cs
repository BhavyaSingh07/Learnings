using CRUDProject.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using ServiceContracts;
using ServiceContracts.DTO;

namespace CRUDProject.Filters.ActionFilters
{
    public class PersonCreateAndEditPostActionFilter : IAsyncActionFilter
    {
        private readonly IcountriesService _countriesService;
        public PersonCreateAndEditPostActionFilter(IcountriesService countriesService)
        {
            _countriesService = countriesService;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if(context.Controller is PersonsController personsController)
            {

                if (!personsController.ModelState.IsValid)
                {
                    List<CountryResponse> countries = await _countriesService.GetAllCountries();
                    personsController.ViewBag.Countries = countries;

                    personsController.ViewBag.Errors = personsController.ModelState.Values.SelectMany(v => v.Errors).SelectMany(e => e.ErrorMessage).ToList();
                    var personRequest = context.ActionArguments["personRequest"];
                    context.Result= personsController.View(personRequest); //short circuit
                }
                else 
                {
                    await next();
                }
            }
            else
            {
                await next();
            }

        }
    }
}