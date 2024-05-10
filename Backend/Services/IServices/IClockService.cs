namespace Services.IServices;

public interface IClockService
{
    Task SetTimeZoneAsync(long timeOffset, Guid id);
    Task<string> GetClockTimeAsync();
}
