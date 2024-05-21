using Newtonsoft.Json;

namespace AAlarmService.DTOs;

public class ToggleAlarmDto
{
    [JsonProperty("state")]
    public bool? state { get; set; }
}