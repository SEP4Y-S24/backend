using Models;
using Shared.IDAO;
using Shared.IService;

namespace Shared.Service;

public class ContactService: IContactService
{
    private readonly IContactDAO _contactDao;
    private readonly IUserDAO _userDao;

    public ContactService(IContactDAO contactDao, IUserDAO userDao)
    {
        _contactDao = contactDao;
        _userDao = userDao;
    }

    public async Task<Contact> CreateAsync(Contact entity)
    {
        try
        {
            User? user1 = await _userDao.GetByAsync(u=>u.Email.Equals(entity.Email1));
            User? user2 = await _userDao.GetByAsync(u=>u.Email.Equals(entity.Email2));
            if (user1==null)
            {
                throw new Exception($"User with email {entity.Email1} was not found.");
            }
            if (user2==null)
            {
                throw new Exception($"User with email {entity.Email2} was not found.");
            }
        
            entity.User1 = user1;
            entity.User2 = user2;
            Contact contact = await _contactDao.CreateAsync(entity);
            return contact;

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception(e.Message);
        }
    }

    public async Task<Contact?> GetByIdAsync(Guid contactId)
    {
        try
        {
            return await _contactDao.GetByIdAsync(contactId);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }        
    }

    public async Task DeleteAsync(string user1, string user2)
    {
        try
        {
            await _contactDao.DeleteAsync(user1,user2);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception(e.Message);
        }
    }

    public async Task<IEnumerable<Contact>> GetAllAsync()
    {
        try
        {
            return await _contactDao.GetAllAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception(e.Message);
        }
    }

    public async Task<IEnumerable<User>> GetAllContactsByUserIdAsync(string userEmail)
    {
        List<Contact?> contacts = await _contactDao.GetAllByAsync(c => c.Email1 == userEmail || c.Email2 == userEmail) as List<Contact?>;
        List<User> users = new List<User>();
        foreach (var contact in contacts)
        {
            if (contact.Email1.Equals(userEmail))
            {
                users.Add(contact.User2);
            }
            if (contact.Email2.Equals(userEmail))
            {
                users.Add(contact.User1);
            }
        }

        return users;
    }
}