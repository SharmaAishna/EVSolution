using ChargingProfileGenerator.Domain.Helper;
using System.Text.Json.Serialization;

namespace ChargingProfileGenerator.Domain.DTOs
{
    public class TariffDTO
    {
        [JsonPropertyName("startTime")]
        public DateTime StartTime { get; set; }

        [JsonPropertyName("endTime")]
        public DateTime EndTime { get; set; }

        [JsonConverter(typeof(DecimalConverter))]
        public decimal EnergyPrice { get; set; }
    }
}
