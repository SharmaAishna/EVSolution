using ChargingProfileGenerator.Domain.Helper;
using System.Text.Json.Serialization;

namespace ChargingProfileGenerator.Domain
{
    public class UserSettings
    {
        [JsonPropertyName("desiredStateOfCharge")]
        public int DesiredStateOfCharge { get; set; }


        [JsonPropertyName("leavingTime")]
        [JsonConverter(typeof(TimeSpanConverter))]
        public TimeSpan LeavingTime { get; set; }

        [JsonPropertyName("directChargingPercentage")]
        public int DirectChargingPercentage { get; set; }

        [JsonPropertyName("tariffs")]
        public List<Tariff> Tariffs { get; set; } = [];
    }

}
