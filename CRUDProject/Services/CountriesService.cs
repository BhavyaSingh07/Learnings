using System.Runtime.ConstrainedExecution;
using Entities;
using ServiceContracts;
using ServiceContracts.DTO;

namespace Services
{
    public class CountriesService : IcountriesService
    {
        private readonly List<Country> _countries;
        public CountriesService(bool initialize = true)
        {
            _countries = new List<Country>();
            if(initialize)
            {
                _countries.AddRange(new List<Country>() {
                new Country() {CountryId = Guid.Parse("703C066D-EF01-48FD-97B0-299E91B222FB"), CountryName = "USA"},
                new Country() { CountryId = Guid.Parse("831C28F1-BB68-41AC-9CEF-A7658FC0B089"), CountryName = "UK" },
                new Country() { CountryId = Guid.Parse("59E0053B-D49B-4D6C-9B18-C13C3A9FEB0D"), CountryName = "India" },
                new Country() { CountryId = Guid.Parse("10F1E8FB-8C14-400F-A125-C6724A195436"), CountryName = "China" },
                new Country() { CountryId = Guid.Parse("4A19E568-E873-4230-86C8-F744BF803E8D"), CountryName = "Brazil" }
                });
               
            }//if ka close
        }
        public CountryResponse AddCountry(CountryAddRequest? countryAddRequest)
        {
            //countryaddrequest cant be null
            if(countryAddRequest == null)
            {
                throw new ArgumentNullException(nameof(countryAddRequest));
            }

            //countryname cant be null
            if(countryAddRequest.CountryName == null)
            {
                throw new ArgumentNullException(nameof(countryAddRequest.CountryName)); 
            }

            //duplicate country name not allowed
            if(_countries.Where(temp => temp.CountryName == countryAddRequest.CountryName).Count() > 0)
            {
                throw new ArgumentNullException("Country name already exists");
            }

            Country country = countryAddRequest.ToCountry();
            country.CountryId = Guid.NewGuid();

            _countries.Add(country);

            return country.ToCountryResponse();


        }

        public List<CountryResponse> GetAllCountries()
        {
            return _countries.Select(country => country.ToCountryResponse()).ToList();
        }

        public CountryResponse? GetCountryByCountryID(Guid? countryID)
        {
            if(countryID == null)
            {
                return null;
            }
            Country? country_response_from_list = _countries.FirstOrDefault(temp => temp.CountryId == countryID);

            if(country_response_from_list == null) { return null; }
            return country_response_from_list.ToCountryResponse();
        }
    }
}
