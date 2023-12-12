using API.DTOs;
using API.Helpers;
using API.Interfaces;
using API.Models;
using API.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly ICountryService _countryService;

        public CountryController(ICountryService countryService)
        {
            _countryService = countryService;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<Country>))]
        public async Task<ActionResult<List<Country>>> GetAllCountries()
        {
            var countries = await _countryService.GetAllCountriesAsync();

            return Ok(countries);
        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(Country))] // 201 Created
        public async Task<ActionResult<Country>> AddCountry([FromBody] CountryDTO countryDto)
        {
            var country = new Country
            {
                Name = countryDto.Name,
            };

            var addedCountry = await _countryService.AddCountryAsync(country);

            return CreatedAtAction(nameof(GetAllCountries), addedCountry);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(200, Type = typeof(OldToNewUpdatedCountry))]
        [ProducesResponseType(404)] // Not Found
        public async Task<ActionResult<OldToNewUpdatedCountry>> UpdateCountry(int id, [FromBody] CountryDTO countryDTO)
        {
            var existingCountry = await _countryService.GetCountryByIdAsync(id);

            if (existingCountry == null)
            {
                return NotFound();
            }

            var countryUpdate = new OldToNewUpdatedCountry
            {
                OldName = existingCountry.Name,

                NewName = countryDTO.Name,
            };

            existingCountry.Name = countryDTO.Name;

            await _countryService.UpdateCountryAsync(existingCountry);

            return Ok(countryUpdate);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(200, Type = typeof(DeletedCountry))]
        [ProducesResponseType(404)] // Not Found
        public async Task<ActionResult<DeletedCountry>> DeleteCountry(int id)
        {
            try
            {
                var country = await _countryService.GetCountryByIdAsync(id);

                var deletedCountry = new DeletedCountry
                {
                    deletedCountryName = country.Name
                };

                await _countryService.DeleteCountryAsync(id);
                return Ok(deletedCountry);
            }
            catch (Exception)
            {
                return NotFound();
            }
        }
    }
}
