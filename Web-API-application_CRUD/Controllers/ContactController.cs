using API.DTOs;
using API.Helpers;
using API.Models;
using API.Services;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly IContactService _contactService;

        public ContactController(IContactService contactService)
        {
            _contactService = contactService;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<Contact>))]
        public async Task<ActionResult<List<Contact>>> GetAllContacts()
        {
            var contacts = await _contactService.GetAllContactsAsync();

            foreach (var contact in contacts)
            {
                var company = await _contactService.GetCompanyByIdAsync(contact.CompanyId);
                var country = await _contactService.GetCountryByIdAsync(contact.CountryId);

                contact.Country = country;
                contact.Company = company;
            }

            return Ok(contacts);
        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(Contact))] // 201 Created
        public async Task<ActionResult<Contact>> AddContact([FromBody] ContactDTO contactDto)
        {
            var company = await _contactService.GetCompanyByIdAsync(contactDto.CompanyID);
            var country = await _contactService.GetCountryByIdAsync(contactDto.CountryID);

            var contact = new Contact
            {
                Name = contactDto.Name,
                CompanyId = contactDto.CompanyID,
                Company = company,
                CountryId = contactDto.CountryID,
                Country = country
            };

            var addedContact = await _contactService.AddContactAsync(contact);

            return CreatedAtAction(nameof(GetAllContacts), addedContact);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(200, Type = typeof(OldToNewUpdatedContact))]
        [ProducesResponseType(404)] // Not Found
        public async Task<ActionResult<OldToNewUpdatedContact>> UpdateContact(int id, [FromBody] ContactDTO contactDTO)
        {
            var existingContact = await _contactService.GetContactByIdAsync(id);

            if (existingContact == null)
            {
                return NotFound();
            }

            var contactUpdate = new OldToNewUpdatedContact
            {
                OldName = existingContact.Name,
                OldCompanyId = existingContact.CompanyId,
                OldCountryId = existingContact.CountryId,

                NewName = contactDTO.Name,
                NewCompanyId = contactDTO.CompanyID,
                NewCountryId = contactDTO.CountryID
            };

            existingContact.Name = contactDTO.Name;
            existingContact.CompanyId = contactDTO.CompanyID;
            existingContact.CountryId = contactDTO.CountryID;

            await _contactService.UpdateContactAsync(existingContact);

            return Ok(contactUpdate);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(200, Type = typeof(DeletedContact))]
        [ProducesResponseType(404)] // Not Found
        public async Task<ActionResult<DeletedContact>> DeleteContact(int id)
        {
            try
            {
                var contact = await _contactService.GetContactByIdAsync(id);

                var company = await _contactService.GetCompanyByIdAsync(contact.CompanyId);
                var country = await _contactService.GetCountryByIdAsync(contact.CountryId);

                var deletedContact = new DeletedContact
                {
                    deletedContactName = contact.Name,
                    deletedContactCountryName = country.Name,
                    deletedContactCompanyName = company.Name
                };

                await _contactService.DeleteContactAsync(id);
                return Ok(deletedContact);
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        [HttpGet("contactsWithCompanyAndCountry/{companyId}/{countryId}")]
        [ProducesResponseType(200, Type = typeof(List<Contact>))]
        public async Task<ActionResult<List<Contact>>> GetContactsWithCompanyAndCountry(int companyId, int countryId)
        {
            var contacts = await _contactService.GetContactsWithCompanyCountryAsync(companyId, countryId);

            return Ok(contacts);
        }

        [HttpGet("contactsWithCompanyOrCountry/")]
        [ProducesResponseType(200, Type = typeof(List<Contact>))]
        public async Task<ActionResult<List<Contact>>> GetContactsWithCompanyOrCountry(int? companyId = null, int? countryId = null)
        {
            var contacts = await _contactService.GetContactsWithCompanyCountryAsync(companyId, countryId);

            return Ok(contacts);
        }
    }
}
