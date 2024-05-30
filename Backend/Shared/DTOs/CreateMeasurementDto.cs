using Models;

namespace Shared.DTOs;

public class CreateMeasurementDto
{
    public MeasurementType Type { get; set; }
    public double Value { get; set; }
    public Guid ClockId { get; set; }
}