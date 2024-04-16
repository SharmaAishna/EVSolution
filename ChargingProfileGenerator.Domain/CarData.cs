using ChargingProfileGenerator.Domain.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ChargingProfileGenerator.Domain
{
    public class CarData
    {
        [JsonPropertyName("chargePower")]
        [JsonConverter(typeof(DecimalConverter))]
        public decimal ChargePower { get; set; }

        [JsonPropertyName("batteryCapacity")]
        [JsonConverter(typeof(DecimalConverter))]
        public decimal BatteryCapacity { get; set; }


        [JsonPropertyName("currentBatteryLevel")]
        [JsonConverter(typeof(DecimalConverter))]
        public decimal CurrentBatteryLevel { get; set; }


    }

}
