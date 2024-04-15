
using ChargingProfileGenerator.Domain.Helper;
using System.Text.Json.Serialization;

namespace ChargingProfileGenerator.Domain
{
    public class ChargingProfile
    {
        [JsonPropertyName("startingTime")]
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime StartingTime { get; set; }

        [JsonPropertyName("userSettings")]
        public UserSettings UserSettings { get; set; } = new UserSettings();

        [JsonPropertyName("carData")]
        public CarData CarData { get; set; } = new CarData();


    }

}
