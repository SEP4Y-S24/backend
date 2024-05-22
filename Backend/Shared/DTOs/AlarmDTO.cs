using Newtonsoft.Json;

namespace Shared.DTOs;

public class AlarmDTO
{
    [JsonProperty("id")]
    public Guid Id { get; set;}
    [JsonProperty("clock_id")]
    public Guid ClockId { get; set; }
    [JsonProperty("name")]
    public string Name { get; set; }
    [JsonProperty("hours")]
    public int Hours { get; set; }
    [JsonProperty("minutes")]

    public int Minutes { get; set; }
    [JsonProperty("is_active")]
    public bool IsActive { get; set; }
    [JsonProperty("is_snoozed")]
    public bool IsSnoozed { get; set; }
}