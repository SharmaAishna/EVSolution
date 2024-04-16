using ChargingProfileGenerator.Domain.BaseModel;
using ChargingProfileGenerator.Domain.Helper;
using System.Text.Json.Serialization;

namespace ChargingProfileGenerator.Domain
{
    public class Tariff : BaseClass
    {
        [JsonPropertyName("energyPrice")]
        [JsonConverter(typeof(DecimalConverter))]
        public decimal EnergyPrice { get; set; }
    }

}
