
using ChargingProfileGenerator.Domain.Helper;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ChargingProfileGenerator.Domain.BaseModel
{
    public class BaseClass
    {
        [JsonPropertyName("startTime")]
        [JsonConverter(typeof(TimeSpanConverter))]
        public DateTime StartTime { get; set; }
        [JsonPropertyName("endTime")]
        [JsonConverter(typeof(TimeSpanConverter))]
        public DateTime EndTime { get; set; }
    }
}
