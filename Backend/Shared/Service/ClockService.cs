using Models;
using Shared.Dtos;
using Shared.IDAO;
using Shared.IService;

namespace Shared.Service;

public class ClockService : IClockService
{
    private readonly  IClockDAO _clockDao;
    private readonly IUserDAO _userDao;

    public ClockService(IClockDAO clockDao, IUserDAO userDao)
    {
        _clockDao = clockDao;
        _userDao = userDao;
    }

    public async Task SetTimeZoneAsync(long timeOffset, Guid id)
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
       existing.TimeOffset = timeOffset;


        await _clockDao.UpdateAsync(existing);

    }

    public async Task<Clock> CreateClockAsync(CreateClockDTO clockToCreate)
    {
        User? user = await _userDao.GetByIdAsync(clockToCreate.UserId);
        if (user == null)
        {
            throw new Exception($"User with id {clockToCreate.UserId} was not found.");
        }

        Clock clock = new Clock()
        {
            Id = Guid.NewGuid(),
            OwnerId = clockToCreate.UserId,
            Name = clockToCreate.Name,
            TimeOffset = clockToCreate.TimeOffset,
            Owner = user
        };
        Clock created = await _clockDao.CreateAsync(clock);
        return created;
    }
    public async Task<Clock> UpdateClockAsync(CreateClockDTO clockToUpdate, Guid id)
    {
        Clock clock = new Clock()
        {
            Id = id,
            OwnerId = clockToUpdate.UserId,
            Name = clockToUpdate.Name,
            TimeOffset = clockToUpdate.TimeOffset
        };
        User? user = await _userDao.GetByIdAsync(clockToUpdate.UserId);
        if (user == null)
        {
            throw new Exception($"User with id {clockToUpdate.UserId} was not found.");
        }

        clock.Owner = user;
        Clock updated = await _clockDao.UpdateAsync(clock);
        return updated;
    }

    public async Task DeleteAsync(Guid id)
    {
        try
        {
            Clock? clock = await _clockDao.GetByIdAsync(id);
            if (clock == null)
            {
                throw new Exception($"Clock with ID {id} not found!");
            }
            await _clockDao.DeleteAsync(id);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public async Task<string> GetClockTimeAsync(Guid id)
    {
        var offset=await _clockDao.GetOffsetByIdAsync(id);
        var time= DateTime.UtcNow.Add(TimeSpan.FromMinutes(offset)).ToString("hh:mm");
        time= "TM|1|4|" + time.Replace(":","") + "|";
        return await Task.FromResult(time);
    }
}