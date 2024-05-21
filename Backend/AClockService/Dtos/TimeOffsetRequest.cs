using Newtonsoft.Json;

namespace AClockService.Dtos;

public class TimeOffsetRequest
{
    [JsonProperty("timeOffset")]
    public long TimeOffset { get; set; }
}