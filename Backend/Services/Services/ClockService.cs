using EfcDatabase.IDAO;
using EfcDatabase.Model;
using Services.IServices;

namespace Services.Services;

public class ClockService : IClockService
{
    private readonly  IClockDAO _clockDao;
    private readonly IUserDAO _userDao;

    public ClockService(IClockDAO clockDao, IUserDAO userDao)
    {
        this._clockDao = clockDao;
        _userDao = userDao;
    }

    public async Task SetTimeZoneAsync(char TimeZone, Guid id)
    {
        Clock? existing = await _clockDao.GetByIdAsync(id);

        if (existing == null)
        {
            throw new Exception($"Clock with ID {id} not found!");
        }

       /* User? user = null;
        if (dto.OwnerId != null)
        {
            user = await userDao.GetById((int)dto.OwnerId);
            if (user == null)
            {
                throw new Exception($"User with id {dto.OwnerId} was not found.");
            }
        }
        User userToUse = user ?? existing.Owner;
    */
       existing.TimeZone = TimeZone;


        await _clockDao.UpdateAsync(existing);

    }

    public async Task<Clock> CreateClockAsync(Clock clockToCreate)
    {
        User? user = await _userDao.GetByIdAsync(clockToCreate.OwnerId);
        if (user == null)
        {
            throw new Exception($"User with id {clockToCreate.OwnerId} was not found.");
        }

        //ValidateTodo(dto);
        // Post post = new Post(user.Id, dto.Title, dto.Content);
        // Post created = await postDao.CreateAsync(post);
        // return created;

        Clock clock = new Clock(user, clockToCreate.Name, clockToCreate.TimeZone);
        Clock created = await _userDao.CreateAsync(clock);
        return created;
    }
}