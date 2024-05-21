﻿using Newtonsoft.Json;

namespace AAlarmService.DTOs;

public class AlarmDTO
{
    [JsonProperty("id")]
    public Guid Id { get; set;}
    [JsonProperty("clock_id")]
    public Guid ClockId { get; set; }
    [JsonProperty("set_of_time")]
    public DateTime SetOffTime { get; set; }
    [JsonProperty("is_active")]
    public bool IsActive { get; set; }
    [JsonProperty("is_snoozed")]
    public bool IsSnoozed { get; set; }
}