
using AClockService.Dtos;
using AClockService.Model;

namespace AClockService.IServices;

public interface IClockService
{
    Task SetTimeZoneAsync(long timeOffset, Guid id);
    Task<string> GetClockTimeAsync();
    Task<Clock> CreateClockAsync(CreateClockDTO clockToCreate);
    Task<Clock> UpdateClockAsync(CreateClockDTO clockToUpdate, Guid id);
    Task DeleteAsync(Guid id);
}
