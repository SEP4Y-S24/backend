using EfcDatabase.Model;

namespace EfcDatabase.IDAO;

public interface IClockDAO
{ 
    Task<Clock> CreateAsync(Clock clock);
    //Task<IEnumerable<Clock>> GetAsync();
    Task UpdateAsync(Clock clock);
    Task<Clock?> GetByIdAsync(Guid clockId);
    Task<long> GetOffsetByIdAsync(Guid clockId);
    Task DeleteAsync(Guid id);
    
}