using Newtonsoft.Json;

namespace Shared.DTOs;

public class ToggleAlarmDto
{
    [JsonProperty("state")]
    public bool? state { get; set; }
}