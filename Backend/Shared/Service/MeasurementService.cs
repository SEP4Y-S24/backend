using System.Diagnostics.Metrics;
using Models;
using Shared.IDAO;
using Shared.IService;

namespace Shared.Service;

public class MeasurementService : IMeasurementService
{
    private readonly  IClockDAO _clockDao;
    private readonly IMeasurementDAO _measurementDao;

    public MeasurementService(IClockDAO clockDao, IMeasurementDAO measurementDao)
    {
        _clockDao = clockDao;
        _measurementDao = measurementDao ;
    }


    public async Task<Measurement> CreateAsync(Measurement measurementToCreate)
    {
        Clock? clock = await _clockDao.GetByIdAsync(measurementToCreate.ClockId);
        if (clock == null)
        {
            throw new Exception($"Clock with id {measurementToCreate.ClockId} was not found.");
        }

        Measurement measurement = new Measurement()
        {
            Id = Guid.NewGuid(),
            ClockId = clock.Id,
            Clock = clock,
            Value = measurementToCreate.Value,
            TimeOfReading = DateTime.UtcNow.AddMinutes(clock.TimeOffset), 
            Type= measurementToCreate.Type
        };
        Measurement m = await _measurementDao.CreateAsync(measurement);
        return m;
    }

    public async Task<Measurement?> GetByIdAsync(Guid measurementId)
    {
        try
        {
            Measurement? measurement = await _measurementDao.GetByIdAsync(measurementId);
            return measurement;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw new InvalidOperationException();
        }
    }

    public Task UpdateAsync(Measurement measurement)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteAsync(Guid measurementId)
    {
        try
        {
            Measurement? measurement = await _measurementDao.GetByIdAsync(measurementId);
            if (measurement == null)
            {
                throw new Exception($"Measurement with ID {measurementId} not found!");
            }
            await _measurementDao.DeleteAsync(measurementId);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }    }

    public async Task<IEnumerable<Measurement>> GetAllAsync()
    {
        try
        {
            return await _measurementDao.GetAllAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<IEnumerable<Measurement>> GetAllByClockIdAsync(Guid clockId)
    {
        try
        {
            return await _measurementDao.GetAllByAsync(a=>a.ClockId.Equals(clockId));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }    
    }

    public async Task<double> GetAvarageByClockTodayAsync(Guid clockId, MeasurementType type)
    {
        Clock? c = await _clockDao.GetByIdAsync(clockId);
        if (c==null)
        {
            throw new Exception("Clock not found!");
        }
        IEnumerable<Measurement> m = await this.GetAllByClockIdAsync(clockId);
        int day = DateTime.UtcNow.AddMinutes(c.TimeOffset).Day;
        int month = DateTime.UtcNow.AddMinutes(c.TimeOffset).Month;
        int year = DateTime.UtcNow.AddMinutes(c.TimeOffset).Year;
        List<Measurement> ms = m.Where(a=>a.Type.Equals(type)).Where(a => a.TimeOfReading.Day.Equals(day) && a.TimeOfReading.Month.Equals(month) && a.TimeOfReading.Year.Equals(year)).ToList();
         double avarage = ms.Average(a => a.Value);
         return avarage;
    }
}