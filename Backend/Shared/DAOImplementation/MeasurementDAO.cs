using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Models;
using Shared.Context;
using Shared.IDAO;

namespace Shared.DAOImplementation;

public class MeasurementDAO : IMeasurementDAO
{
    private readonly ClockContext _context;

    public MeasurementDAO(ClockContext dbContext)
    {
        _context = dbContext;
    }

    public async Task<Measurement> CreateAsync(Measurement measurement)
    {
        try
        {
            if (measurement == null && measurement.ClockId == null)
            {
                throw new ArgumentNullException($"The given Measurement object {measurement} is null");
            }

            await _context.Measurements.AddAsync(measurement);
            await _context.SaveChangesAsync();
            return measurement;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }    }

    public async Task UpdateAsync(Measurement measurement)
    {
        if (measurement==null)
        {
            throw new ArgumentNullException("Measurement object is not found in the database");
        }
        Measurement? dbEntity = await GetByIdAsync(measurement.Id);
        if (dbEntity == null)
        {
            throw new ArgumentNullException("Measurement object is not found in the database");
        }
 
        _context.Measurements.Entry(dbEntity).CurrentValues.SetValues(measurement);

        _context.Update(dbEntity);
        await _context.SaveChangesAsync();
    }

    public async Task<Measurement?> GetByIdAsync(Guid measurementId)
    {
        if (measurementId.Equals(null))
        {
            throw new ArgumentNullException("Measurement's id is null!");
        }
        Measurement? existing = await _context.Measurements.FirstOrDefaultAsync(t => t.Id == measurementId);
        return existing;
    }

    public async Task DeleteAsync(Guid measurementId)
    {
        var entity = await GetByIdAsync(measurementId);

        if (entity == null)
        {
            throw new ArgumentException("Measuremnt is null!");
        }

        _context.Remove(entity);

        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Measurement>> GetAllAsync()
    {
        return await _context.Set<Measurement>().ToListAsync();
    }

    public async Task<IEnumerable<Measurement>> GetAllByAsync(Expression<Func<Measurement, bool>> filter)
    {
        return await _context.Set<Measurement>().Where(filter).ToListAsync();
    }
}