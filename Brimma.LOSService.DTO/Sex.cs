using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Brimma.LOSService.DTO
{
    public class Sex
    {
        [JsonProperty(PropertyName = "dnp")]
        public bool Dnp { get; set; }

        [JsonProperty(PropertyName = "female")]
        public bool Female { get; set; }

        [JsonProperty(PropertyName = "male")]
        public bool Male { get; set; }
    }
}
