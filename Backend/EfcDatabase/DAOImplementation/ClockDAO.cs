using Application.DAO;
using EfcDatabase.Model;

namespace EfcDatabase.DAOImplementation;

public class ClockDAO: IClockDAO
{
    public Task<Clock> CreateAsync(Clock clock)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(Clock clock)
    {
        throw new NotImplementedException();
    }

    public Task<Clock?> GetByIdAsync(Guid clockId)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}