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
        public decimal ChargePower { get; set; }

        [JsonPropertyName("batteryCapacity")]
        public decimal BatteryCapacity { get; set; }


        [JsonPropertyName("currentBatteryLevel")]
        public decimal CurrentBatteryLevel { get; set; }


    }

}
