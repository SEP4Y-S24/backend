using System.ComponentModel.DataAnnotations;

namespace Models;

public class Measurement
{
    [Key]
    public Guid Id { get; set; }
    public MeasurementType Type { get; set; }
    public DateTime TimeOfReading { get; set; }
    public double Value { get; set; }
    public Clock Clock { get; set; }
    public Guid ClockId { get; set; }
}