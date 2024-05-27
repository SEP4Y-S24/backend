using System.Net;
using Models;
using Shared.IDAO;
using Shared.IService;

namespace Shared.Service;

public class EventService : IEventService
{
    private readonly IEventDAO _eventDao;
    private readonly IUserDAO _userDao;
    private readonly ITagDao _tagDao;

    public EventService(IEventDAO eventDao, IUserDAO userDao, ITagDao tagDao)
    {
        _eventDao = eventDao;
        _userDao = userDao;
        _tagDao = tagDao;
    }

    public async Task<Event> CreateAsync(Event eventToCreate)
    {
        try
        {
            User? user = await _userDao.GetByIdAsync(eventToCreate.UserId);
            if (user==null)
            {
                throw new Exception($"User with id {eventToCreate.UserId} was not found.");
            }

            Event e = new Event
            {
                Id =   Guid.NewGuid(),
                UserId = eventToCreate.UserId,
                Name = eventToCreate.Name,
                Description = eventToCreate.Description,
                StartDate = eventToCreate.StartDate,
                EndDate = eventToCreate.EndDate,
                Status = eventToCreate.Status
            };
            Event created = await _eventDao.CreateAsync(e);
            return created;
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
            throw new Exception(exception.Message);
        }
    }

    public async Task<Event?> GetByIdAsync(Guid eventId)
    {

        try
        {
            return await _eventDao.GetByIdAsync(eventId);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }    
    }

    public async Task UpdateAsync(Event ev)
    {
        try
        {
            await _eventDao.UpdateAsync(ev);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task UpdateStatusByIdAsync(Guid eventId, Status status)
    {
        try
        {
            await _eventDao.UpdateStatusByIdAsync(eventId, status);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception(e.Message);
        }
    }

    public async Task DeleteAsync(Guid eventId)
    {
        try
        {
            await _eventDao.DeleteAsync(eventId);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception(e.Message);
        }
    }

    public async Task<IEnumerable<Event>> GetAllAsync()
    {
        try
        {
            return await _eventDao.GetAllAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception(e.Message);
        }
    }

    public async Task<IEnumerable<Event>> GetAllByUserIdAsync(Guid userId)
    {
        try
        {
            return await _eventDao.GetAllByUserIdAsync(userId);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception(e.Message);
        }
    }

    public async Task<IEnumerable<Event>> FilterByTags(List<Tag> tags, Guid userId)
    {
        try
        {
            IEnumerable<Tag> t = await _tagDao.GetAllAsync();
            List<Event> events = new List<Event>();
            foreach (var tag in t)
            {
                if (events.Any(ta=>ta.Name.Equals(tag.Name)))
                {
                    foreach (var e in tag.Events)
                    {
                        if (e.UserId == userId)
                        {
                            events.Add(e);
                        }
                    }
                }
            }
            events = events.Distinct().ToList();
            return events;

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception(e.Message);
        }
    }
}