using ChargingProfileGenerator.Domain.BaseModel;
using System.Text.Json.Serialization;

namespace ChargingProfileGenerator.Domain
{
    public class Tariff : BaseClass
    {
        [JsonPropertyName("energyPrice")]
        public decimal EnergyPrice { get; set; }
    }

}
