using API.Models;

public interface IContactService
{
    Task<List<Contact>> GetAllContactsAsync();
    Task<Contact> GetContactByIdAsync(int contactId);
    Task<Contact> AddContactAsync(Contact contact);
    Task<Contact> UpdateContactAsync(Contact contact);
    Task<Contact> DeleteContactAsync(int contactId);
    Task<Company> GetCompanyByIdAsync(int companyId);
    Task<Country> GetCountryByIdAsync(int countryId);
    Task<List<Contact>> GetContactsWithCompanyCountryAsync(int? companyId = null, int? countryId = null);

}
