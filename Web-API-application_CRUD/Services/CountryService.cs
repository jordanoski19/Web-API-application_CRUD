using API.Data;
using API.Models;
using API.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Services
{
    public class CountryService : ICountryService
    {
        private readonly DataContext _context;

        public CountryService(DataContext context)
        {
            _context = context;
        }

        public async Task<List<Country>> GetAllCountriesAsync()
        {
            var list = await _context.Countries.ToListAsync();
            return list;
        }

        public async Task<Country> GetCountryByIdAsync(int countryId)
        {
            return await _context.Countries.FindAsync(countryId);
        }

        public async Task<Country> UpdateCountryAsync(Country country)
        {
            _context.Entry(country).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return country;
        }

        public async Task<Country> AddCountryAsync(Country country)
        {
            _context.Countries.Add(country);
            await _context.SaveChangesAsync();
            return country;
        }

        public async Task<Country> DeleteCountryAsync(int id)
        {
            var country = await _context.Countries.FindAsync(id);

            if (country == null)
            {
                throw new Exception("Country not found");
            }

            _context.Countries.Remove(country);
            await _context.SaveChangesAsync();

            return country;
        }
    }
}
