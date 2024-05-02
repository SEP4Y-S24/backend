namespace Services.IServices;

public interface IClockService
{
    Task SetTimeZoneAsync(char TimeZone, Guid id);
}
