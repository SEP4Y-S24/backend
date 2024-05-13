using EfcDatabase.Model;

namespace Services.IServices;

public interface IClockService
{
    Task SetTimeZoneAsync(long timeOffset, Guid id);
    Task<string> GetClockTimeAsync();
    Task<Clock> CreateClockAsync(Clock clockToCreate);
    Task<Clock> UpdateClockAsync(Clock clockToUpdate);
    Task DeleteAsync(Guid id);
}
