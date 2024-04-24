using EfcDatabase.Model;

namespace Application.DAO;

public interface IClockDAO
{ 
    Task<Clock> CreateAsync(Clock clock);
    //Task<IEnumerable<Clock>> GetAsync();
    Task UpdateAsync(Clock clock);
    Task<Clock?> GetByIdAsync(Guid clockId);
    Task DeleteAsync(Guid id);
    
}