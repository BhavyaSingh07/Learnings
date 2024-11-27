using System.Runtime.ConstrainedExecution;
using Entities;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using ServiceContracts.DTO;

namespace Services
{
    public class CountriesService : IcountriesService
    {
        //private readonly List<Country> _db;
        private readonly PersonsDbContext _db;
        public CountriesService(PersonsDbContext personsDbContext)
            //bool initialize = true
        {
            //_db = new List<Country>();
            _db = personsDbContext;
            //if(initialize)
            //{
            //    _db.AddRange(new List<Country>() {
            //    new Country() {CountryId = Guid.Parse("703C066D-EF01-48FD-97B0-299E91B222FB"), CountryName = "USA"},
            //    new Country() { CountryId = Guid.Parse("831C28F1-BB68-41AC-9CEF-A7658FC0B089"), CountryName = "UK" },
            //    new Country() { CountryId = Guid.Parse("59E0053B-D49B-4D6C-9B18-C13C3A9FEB0D"), CountryName = "India" },
            //    new Country() { CountryId = Guid.Parse("10F1E8FB-8C14-400F-A125-C6724A195436"), CountryName = "China" },
            //    new Country() { CountryId = Guid.Parse("4A19E568-E873-4230-86C8-F744BF803E8D"), CountryName = "Brazil" }
            //    });
               
            //}//if ka close
        }
        public async Task<CountryResponse> AddCountry(CountryAddRequest? countryAddRequest)
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
            //if(_db.Where(temp => temp.CountryName == countryAddRequest.CountryName).Count() > 0)
            //{
            //    throw new ArgumentNullException("Country name already exists");
            //}
            if (await _db.Countries.CountAsync(temp => temp.CountryName == countryAddRequest.CountryName) > 0)
            {
                throw new ArgumentNullException("Country name already exists");
            }

            Country country = countryAddRequest.ToCountry();
            country.CountryId = Guid.NewGuid();

            _db.Countries.Add(country);
            await _db.SaveChangesAsync();

            return country.ToCountryResponse();


        }

        public async Task<List<CountryResponse>> GetAllCountries()
        {
            //return _db.Select(country => country.ToCountryResponse()).ToList();
            return await _db.Countries.Select(country => country.ToCountryResponse()).ToListAsync();
        }

        public async Task<CountryResponse?> GetCountryByCountryID(Guid? countryID)
        {
            if(countryID == null)
            {
                return null;
            }
            Country? country_response_from_list = await _db.Countries.FirstOrDefaultAsync(temp => temp.CountryId == countryID);

            if(country_response_from_list == null) { return null; }
            return country_response_from_list.ToCountryResponse();
        }
    }
}
