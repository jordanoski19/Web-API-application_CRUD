using API.Models;

namespace API.Interfaces
{
    public interface ICountryService
    {
        Task<List<Country>> GetAllCountriesAsync();
        Task<Country> GetCountryByIdAsync(int countryId);
        Task<Country> AddCountryAsync(Country country);
        Task<Country> UpdateCountryAsync(Country country);
        Task<Country> DeleteCountryAsync(int countryId);
    }
}
