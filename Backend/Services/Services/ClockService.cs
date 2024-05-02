using Application.DAO;
using EfcDatabase.Model;
using Services.IServices;

namespace Services.Services;

public class ClockService : IClockService
{
    private readonly IClockDAO _clockDao;

    public ClockService(IClockDAO clockDao)
    {
        this._clockDao = clockDao;
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
}