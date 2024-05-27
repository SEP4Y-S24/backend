using Models;

namespace Shared.IService;

public interface IContactService
{
    Task<Contact> CreateAsync(Contact eventToCreate);
    Task<Contact?> GetByIdAsync(Guid eventId);
    Task DeleteAsync(string user1, string user2);
    Task<IEnumerable<Contact>> GetAllAsync();
    Task<IEnumerable<User>> GetAllContactsByUserIdAsync(string userEmail);

}