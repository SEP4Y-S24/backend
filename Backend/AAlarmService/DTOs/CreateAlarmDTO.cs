﻿using Newtonsoft.Json;

namespace AAlarmServices.DTOs;

public class CreateAlarmDTO
{
    [JsonProperty("clock_id")]
    public Guid ClockId { get; set; }
    [JsonProperty("set_of_time")]
    public DateTime SetOffTime { get; set; }

}