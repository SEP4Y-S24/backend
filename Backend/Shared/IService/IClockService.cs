
using Models;
using Shared.Dtos;

namespace Shared.IService;

public interface IClockService
{
    Task SetTimeZoneAsync(long timeOffset, Guid id);
    Task<string> GetClockTimeAsync();
    Task<Clock?> GetClockById(Guid clockId);
    Task<Clock> CreateClockAsync(ClockDTO clockToCreate);
    Task<Clock> UpdateClockAsync(ClockDTO clockToUpdate, Guid id);
    Task DeleteAsync(Guid id);
}
