using System.Linq.Expressions;
using UserService.Model;

namespace UserService.IDAO;

public interface IClockDAO
{ 
    Task<IEnumerable<Clock>> GetAllByAsync(Expression<Func<Clock, bool>> filter);
    Task<Clock?> GetByIdAsync(Guid clockId);
    Task<Clock> CreateAsync(Clock clock);

}

