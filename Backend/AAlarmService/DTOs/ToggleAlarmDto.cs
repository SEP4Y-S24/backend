using Newtonsoft.Json;

namespace AAlarmServices.DTOs;

public class ToggleAlarmDto
{
    [JsonProperty("state")]
    public bool? state { get; set; }
}