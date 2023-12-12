using API.Data;
using API.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Services
{
    public class ContactService : IContactService
    {
        private readonly DataContext _context;

        public ContactService(DataContext context)
        {
            _context = context;
        }

        public async Task<List<Contact>> GetAllContactsAsync()
        {
            var list = await _context.Contacts.ToListAsync();
            return list;
        }
        public async Task<Contact> GetContactByIdAsync(int contactId)
        {
            return await _context.Contacts.FindAsync(contactId);
        }

        public async Task<Contact> UpdateContactAsync(Contact contact)
        {
            _context.Entry(contact).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return contact;
        }

        public async Task<Company> GetCompanyByIdAsync(int companyId)
        {
            var company = await _context.Companies.FindAsync(companyId);
            return company; // Assuming 'Name' is the property representing the company name.
        }
        public async Task<Country> GetCountryByIdAsync(int countryId)
        {
            var country = await _context.Countries.FindAsync(countryId);
            return country; // Assuming 'Name' is the property representing the company name.
        }

        public async Task<Contact> AddContactAsync(Contact contact)
        {
            _context.Contacts.Add(contact);
            await _context.SaveChangesAsync();
            return contact;
        }

        public async Task<Contact> DeleteContactAsync(int id)
        {
            var contact = await _context.Contacts.FindAsync(id);

            if (contact == null)
            {
                throw new Exception("Contact not found");
            }

            _context.Contacts.Remove(contact);
            await _context.SaveChangesAsync();

            return contact;
        }
        public async Task<List<Contact>> GetContactsWithCompanyCountryAsync(int? companyId = null, int? countryId = null)
        {
            // Start with the base query
            var query = _context.Contacts
                .Include(c => c.Company)
                .Include(c => c.Country)
                .AsQueryable();

            // Apply filters if provided
            if (companyId.HasValue)
            {
                query = query.Where(c => c.CompanyId == companyId);
            }

            if (countryId.HasValue)
            {
                query = query.Where(c => c.CountryId == countryId);
            }

            // Execute the query and return the result
            var contacts = await query.ToListAsync();
            return contacts;
        }
    }
}
