using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
using Services;
using Xunit;
using Microsoft.EntityFrameworkCore;
using EntityFrameworkCoreMock;

namespace CRUDTests
{
    public class CountriesServiceTest
    {
        private readonly IcountriesService _countriesService;
        public CountriesServiceTest()
        {
            var countriesInitialData = new List<Country>() {  };
            DbContextMock<ApplicationDbContext> dbContextMock = new DbContextMock<ApplicationDbContext>(
                new DbContextOptionsBuilder<ApplicationDbContext>().Options
                );
            //var dbContext = new ApplicationDbContext(new DbContextOptionsBuilder<ApplicationDbContext>().Options);
            ApplicationDbContext dbContext = dbContextMock.Object;  
            dbContextMock.CreateDbSetMock(temp => temp.Countries, countriesInitialData ); 
            _countriesService = new CountriesService(null);
        }

        #region AddCountry

        //when countryaddrequest is null, it should argumentnull
        [Fact]
        public async Task AddCountry_NullCountry()
        {
            //arrange
            CountryAddRequest? request = null;
            //assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => {
                await _countriesService.AddCountry(request);
            });

        }
        //when countryname is null, argumentexception
        [Fact]
        public async Task AddCountry_CountryNameIsNull()
        {
            //arrange
            CountryAddRequest? request = new CountryAddRequest() { CountryName=null};
            //assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => {
                await _countriesService.AddCountry(request);
            });

        }
        //country name is duplicate, argumentexception
        [Fact]
        public async Task AddCountry_DuplicateCountryName()
        {
            //arrange
            CountryAddRequest? request1 = new CountryAddRequest() { CountryName = "USA" };
            CountryAddRequest? request2 = new CountryAddRequest() { CountryName = "USA" };
            //assert
            await Assert.ThrowsAsync<ArgumentNullException>(async() => {
                await _countriesService.AddCountry(request1);
                await _countriesService.AddCountry(request2);
            });

        }
        //supply proper country name, insert it into list
        [Fact]
        public async Task AddCountry_ProperCountryDetails()
        {
            //arrange
            CountryAddRequest? request = new CountryAddRequest() { CountryName = "Japan" };
            //act
            CountryResponse response = await _countriesService.AddCountry(request);
            
            List<CountryResponse> countries_from_GetAllCountries = await _countriesService.GetAllCountries();
            //assert
            Assert.True(response.CountryId!= Guid.Empty);
            Assert.Contains(response, countries_from_GetAllCountries);

        }

        #endregion

        #region GetAllCountries

        //list of countries should be empty by default

        [Fact]
        public async Task GetAllCountries_EmptyList()
        {
            //act
            List<CountryResponse> actual_country_response_list =await _countriesService.GetAllCountries();

            //assert
            Assert.Empty(actual_country_response_list);

        }

        [Fact]
        public async Task GetAllCountries_AddFewCountries()
        {
            List<CountryAddRequest> country_request_list = new List<CountryAddRequest>() { new CountryAddRequest() { CountryName = "USA" }, new CountryAddRequest() { CountryName = "Japan" } };

            List<CountryResponse> countries_list_from_add_country = new List<CountryResponse>();
            foreach(CountryAddRequest country_request in country_request_list)
            {
                countries_list_from_add_country.Add(await _countriesService.AddCountry(country_request));
            }

            List<CountryResponse> actualcountryResponseList = await _countriesService.GetAllCountries();

            foreach(CountryResponse expected_country in countries_list_from_add_country)
            {
                Assert.Contains(expected_country, actualcountryResponseList);
            }
        }
        #endregion

        #region GetCountryByCountryID

        [Fact]
        public async Task GetCountryByCountryID_NullCountryID()
        {
            //arrange
            Guid? countryID = null; 
            //act
            CountryResponse? country_response_from_get_method = await _countriesService.GetCountryByCountryID(countryID);
            //assert
            Assert.Null(country_response_from_get_method);
        }

        [Fact]
        public async Task GetCountryByCountryID_ValidCountryID()
        {
            CountryAddRequest? country_add_request = new CountryAddRequest() { CountryName = "China" };
            CountryResponse country_response_from_add = await _countriesService.AddCountry(country_add_request);

            //act
            CountryResponse? country_reponse_from_get = await _countriesService.GetCountryByCountryID(country_response_from_add.CountryId);
            Assert.Equal(country_response_from_add, country_reponse_from_get);
        }
        #endregion
    }
}
