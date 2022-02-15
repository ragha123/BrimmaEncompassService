using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Brimma.LOSService.DTO
{
    public class Income
    {
        [JsonProperty(PropertyName = "base")]
        public double Base { get; set; }

        [JsonProperty(PropertyName = "overtime")]
        public double Overtime { get; set; }

        [JsonProperty(PropertyName = "bonus")]
        public double Bonus { get; set; }

        [JsonProperty(PropertyName = "commission")]
        public double Commission { get; set; }

        [JsonProperty(PropertyName = "other")]
        public double Other { get; set; }
    }
}
