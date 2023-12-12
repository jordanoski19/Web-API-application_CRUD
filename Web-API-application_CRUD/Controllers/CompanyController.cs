using API.DTOs;
using API.Helpers;
using API.Models;
using API.Services;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyService _companyService;

        public CompanyController(ICompanyService companyService)
        {
            _companyService = companyService;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<Company>))]
        public async Task<ActionResult<List<Company>>> GetAllCompanies()
        {
            var companies = await _companyService.GetAllCompaniesAsync();

            return Ok(companies);
        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(Company))] // 201 Created
        public async Task<ActionResult<Company>> AddCompany([FromBody] CompanyDTO companyDto)
        {
            var company = new Company
            {
                Name = companyDto.Name,
            };

            var addedCompany = await _companyService.AddCompanyAsync(company);

            return CreatedAtAction(nameof(GetAllCompanies), addedCompany);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(200, Type = typeof(OldToNewUpdatedCompany))]
        [ProducesResponseType(404)] // Not Found
        public async Task<ActionResult<OldToNewUpdatedCompany>> UpdateCompany(int id, [FromBody] CompanyDTO companyDTO)
        {
            var existingCompany = await _companyService.GetCompanyByIdAsync(id);

            if (existingCompany == null)
            {
                return NotFound();
            }

            var companyUpdate = new OldToNewUpdatedCompany
            {
                OldName = existingCompany.Name,

                NewName = companyDTO.Name,
            };

            existingCompany.Name = companyDTO.Name;

            await _companyService.UpdateCompanyAsync(existingCompany);

            return Ok(companyUpdate);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(200, Type = typeof(DeletedCompany))]
        [ProducesResponseType(404)] // Not Found
        public async Task<ActionResult<DeletedCompany>> DeleteCompany(int id)
        {
            try
            {
                var company = await _companyService.GetCompanyByIdAsync(id);

                var deletedCompany = new DeletedCompany
                {
                    deletedCompanyName = company.Name
                };

                await _companyService.DeleteCompanyAsync(id);
                return Ok(deletedCompany);
            }
            catch (Exception)
            {
                return NotFound();
            }
        }
    }
}
