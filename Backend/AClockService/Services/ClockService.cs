using AClockService.Context;
using AClockService.Dtos;
using AClockService.IDAO;
using AClockService.IServices;
using AClockService.Model;

namespace AClockService.Services;

public class ClockService : IClockService
{
    private readonly  IClockDAO _clockDao;
    private readonly IUserDAO _userDao;

    public ClockService(IClockDAO clockDao, IUserDAO userDao)
    {
        this._clockDao = clockDao;
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

    Task<Clock> IClockService.UpdateClockAsync(CreateClockDTO clockToUpdate, Guid id)
    {
        Clock clock = new Clock()
        {
            Id = id,
            OwnerId = clockToUpdate.UserId,
            Name = clockToUpdate.Name,
            TimeOffset = clockToUpdate.TimeOffset
        };
        return UpdateClockAsync(clock);
    }

    public async Task<Clock> UpdateClockAsync(Clock clockToUpdate)
    {
        User? user = await _userDao.GetByIdAsync(clockToUpdate.OwnerId);
        if (user == null)
        {
            throw new Exception($"User with id {clockToUpdate.OwnerId} was not found.");
        }

        clockToUpdate.Owner = user;
        Clock updated = await _clockDao.UpdateAsync(clockToUpdate);
        return updated;
    }

    public async Task DeleteAsync(Guid id)
    {
        try
        {
            Clock clock = await _clockDao.GetByIdAsync(id);
            await _clockDao.DeleteAsync(id);
            _userDao.DeleteClock(id, clock.OwnerId);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public async Task<string> GetClockTimeAsync()
    {
        var offset=await _clockDao.GetOffsetByIdAsync(Guid.Parse("f656d97d-63b7-451a-91ee-0e620e652c9e"));//TODO resolve hardcode
        var time= DateTime.UtcNow.Add(TimeSpan.FromMinutes(offset)).ToString("hh:mm:ss");
        return await Task.FromResult(time);
    }
}