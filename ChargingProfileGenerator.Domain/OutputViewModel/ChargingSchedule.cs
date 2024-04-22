using ChargingProfileGenerator.Domain.Helper;
using System.Text.Json.Serialization;

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
