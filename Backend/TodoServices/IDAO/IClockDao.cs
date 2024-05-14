﻿using System.Linq.Expressions;
using TodoServices.Model;

namespace TodoServices.IDAO;

public interface IClockDao
{
    Task<Clock> CreateAsync(Clock clock);
    //Task<IEnumerable<Clock>> GetAsync();
    Task<IEnumerable<Clock>> GetAll();
    Task<IEnumerable<Clock>> GetAllByAsync(Expression<Func<Clock, bool>> filter);
    Task<Clock> UpdateAsync(Clock clock);
    Task<Clock?> GetByIdAsync(Guid clockId);
    Task<long> GetOffsetByIdAsync(Guid clockId);
    Task DeleteAsync(Guid id);
}