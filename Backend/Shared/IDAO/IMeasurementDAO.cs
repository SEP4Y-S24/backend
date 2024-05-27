using System.Linq.Expressions;
using Models;

namespace Shared.IDAO;

public interface IMeasurementDAO
{
    Task<Measurement> CreateAsync(Measurement measurement);
    Task UpdateAsync(Measurement measurement);
    Task<Measurement?> GetByIdAsync(Guid measurementId);
    Task DeleteAsync(Guid measurementId);
    Task<IEnumerable<Measurement>> GetAllAsync();
    Task<IEnumerable<Measurement>> GetAllByAsync(Expression<Func<Measurement, bool>> filter);

}