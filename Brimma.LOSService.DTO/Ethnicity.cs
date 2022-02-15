using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Brimma.LOSService.DTO
{
    public class Ethnicity
    {
        [JsonProperty(PropertyName = "dnp")]
        public bool Dnp { get; set; }

        [JsonProperty(PropertyName = "hispanicOrLatino")]
        public bool HispanicOrLatino { get; set; }

        [JsonProperty(PropertyName = "hispanicOriginTypes")]
        public string[] HispanicOriginTypes { get; set; }

        [JsonProperty(PropertyName = "hispanicOriginOtherDetail")]
        public string HispanicOriginOtherDetail { get; set; }

        [JsonProperty(PropertyName = "notHispanicOrLatino")]
        public bool NotHispanicOrLatino { get; set; }
    }
}
