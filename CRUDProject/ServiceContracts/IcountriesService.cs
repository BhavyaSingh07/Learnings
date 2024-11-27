using Entities;
using ServiceContracts.DTO;

namespace ServiceContracts
{
    /// <summary>
    /// Represents business logic for manipulating Country
    /// </summary>
    public interface IcountriesService
    {
        /// <summary>
        /// adds a country to the list of countries
        /// </summary>
        /// <param name="countryAddRequest"></param>
        /// <returns>Returns country object after adding it in newly added countryresponse</returns>
        Task<CountryResponse> AddCountry(CountryAddRequest? countryAddRequest);

        /// <summary>
        /// Returns all countries
        /// </summary>
        /// <returns>Returns all countries from the lists</returns>
        Task<List<CountryResponse>> GetAllCountries();

        /// <summary>
        /// Returns a country based on the given country ID
        /// </summary>
        /// <param name="countryID"></param>
        /// <returns>Matching country object</returns>
        Task<CountryResponse?> GetCountryByCountryID(Guid? countryID);
    }
}

