using System.Linq.Expressions;
using Models;

namespace Shared.IDAO;

public interface IContactDAO
{
    Task<Contact> CreateAsync(Contact contact);
    Task<IEnumerable<Contact>> GetAllAsync();
    Task<IEnumerable<Contact>> GetAllByAsync(Expression<Func<Contact, bool>> filter);
    Task<Contact> UpdateAsync(Contact contact);
    Task<Contact?> GetByIdAsync(Guid contactId);
    Task DeleteAsync(string user1, string user2);

}