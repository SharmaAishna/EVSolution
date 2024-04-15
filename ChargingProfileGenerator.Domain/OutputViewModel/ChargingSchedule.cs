using ChargingProfileGenerator.Domain.BaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ChargingProfileGenerator.Domain.OutputViewModel
{
    public class ChargingSchedule : BaseClass
    {

        [JsonPropertyName("isCharging")]
        public bool IsCharging { get; set; }

    }
}
