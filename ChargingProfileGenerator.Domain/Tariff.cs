using ChargingProfileGenerator.Domain.BaseModel;
using ChargingProfileGenerator.Domain.Helper;
using System.Text.Json.Serialization;

namespace ChargingProfileGenerator.Domain
{
    public class Tariff
    {
        [JsonPropertyName("startTime")]
        [JsonConverter(typeof(TimeSpanConverter))]

        public TimeSpan StartTime { get; set; }
        [JsonPropertyName("endTime")]

        [JsonConverter(typeof(TimeSpanConverter))]
        public TimeSpan EndTime { get; set; }
        [JsonPropertyName("energyPrice")]

        [JsonConverter(typeof(DecimalConverter))]
        public decimal EnergyPrice { get; set; }
    }

}
