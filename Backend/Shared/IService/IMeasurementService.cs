using Models;

namespace Shared.IService;

public interface IMeasurementService
{
    Task<Measurement> CreateAsync(Measurement measurementToCreate);
    Task<Measurement?> GetByIdAsync(Guid measurementId);
    Task UpdateAsync(Measurement measurement);
    Task DeleteAsync(Guid measurementId);
    Task<IEnumerable<Measurement>> GetAllAsync();
    Task<IEnumerable<Measurement>> GetAllByClockIdAsync(Guid clockId);
    Task<double> GetAvarageByClockTodayAsync(Guid clockId, MeasurementType type);

}