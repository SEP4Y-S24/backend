using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Models;
using Shared.Context;
using Shared.IDAO;

namespace Shared.DAOImplementation;

public class ContactDAO : IContactDAO
{
    private readonly ClockContext _context;

    public ContactDAO(ClockContext dbContext)
    {
        this._context = dbContext;
    }

    public async Task<Contact> CreateAsync(Contact contact)
    {
        if (_context.Contacts.Any(c =>
                (c.Email1 == contact.Email1 && c.Email2 == contact.Email2) ||
                (c.Email1 == contact.Email2 && c.Email2 == contact.Email1)))
        {
            throw new Exception("Contact already exists!");
        }
        EntityEntry<Contact> added = await _context.Contacts.AddAsync(contact);
        await _context.SaveChangesAsync();
        return added.Entity;

    }

    public async Task<IEnumerable<Contact>> GetAllAsync()
    {
        return await _context.Set<Contact>().ToListAsync();

    }

    public async Task<IEnumerable<Contact>> GetAllByAsync(Expression<Func<Contact, bool>> filter)
    {
        return await _context.Set<Contact>().Include(u=>u.User2).Include(u=>u.User1).Where(filter).ToListAsync();
    }

    public Task<Contact> UpdateAsync(Contact contact)
    {
        throw new NotImplementedException();
    }

    public async Task<Contact?> GetByIdAsync(Guid contactId)
    {
        if (contactId.Equals(null))
        {
            throw new ArgumentNullException("Contact's id is null!");
        }
        Contact? existing = await _context.Contacts.FirstOrDefaultAsync(t => t.id == contactId);
        return existing;    }


    public async Task DeleteAsync(string user1, string user2)
    {
       List<Contact> c = _context.Contacts.Where(c => c.Email1 == user1 && c.Email2 == user2).ToList();
       if(c.Count!= 1)
       {
           throw new ArgumentNullException("Contact does not exist!");
       }
       _context.Remove(c[0]);

       await _context.SaveChangesAsync();
    }
}