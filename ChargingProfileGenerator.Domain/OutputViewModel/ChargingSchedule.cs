using ChargingProfileGenerator.Domain.BaseModel;
using ChargingProfileGenerator.Domain.Helper;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ChargingProfileGenerator.Domain.OutputViewModel
{
    public class ChargingSchedule 
    {
        [JsonPropertyName("startTime")]
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime StartTime { get; set; }
        [JsonPropertyName("endTime")]
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime EndTime { get; set; }
        [JsonPropertyName("isCharging")]
        public bool IsCharging { get; set; }

    }
}
