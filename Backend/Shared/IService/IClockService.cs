
using Models;
using Shared.Dtos;

namespace Shared.IService;

public interface IClockService
{
    Task SetTimeZoneAsync(long timeOffset, Guid id);
    Task<Clock> CreateClockAsync(CreateClockDTO clockToCreate);
    Task<string> GetClockTimeAsync(Guid id);
    Task<Clock> UpdateClockAsync(CreateClockDTO clockToUpdate, Guid id);
    Task DeleteAsync(Guid id);
}
