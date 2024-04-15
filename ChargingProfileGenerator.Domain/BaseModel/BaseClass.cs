
using ChargingProfileGenerator.Domain.Helper;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ChargingProfileGenerator.Domain.BaseModel
{
    public class BaseClass
    {
        [JsonPropertyName("startTime")]
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime StartTime { get; set; }
        [JsonPropertyName("endTime")]
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime EndTime { get; set; }
    }
}
