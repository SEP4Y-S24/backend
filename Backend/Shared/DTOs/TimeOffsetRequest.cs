using Newtonsoft.Json;

namespace Shared.Dtos;

public class TimeOffsetRequest
{
    [JsonProperty("timeOffset")]
    public long TimeOffset { get; set; }
}