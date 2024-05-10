using System.Linq.Expressions;
using EfcDatabase.Model;

namespace EfcDatabase.IDAO;

public interface IClockDAO
{ 
    Task<Clock> CreateAsync(Clock clock);
    //Task<IEnumerable<Clock>> GetAsync();
    Task<IEnumerable<Clock>> GetAll();
    Task<IEnumerable<Clock>> GetAllBy(Expression<Func<Clock, bool>> filter);
    Task UpdateAsync(Clock clock);
    Task<Clock?> GetByIdAsync(Guid clockId);
    Task DeleteAsync(Guid id);
    
}

