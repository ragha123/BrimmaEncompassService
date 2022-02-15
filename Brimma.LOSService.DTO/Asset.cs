using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Brimma.LOSService.DTO
{
    public class Asset
    {
        [JsonProperty(PropertyName = "category")]
        public string Category { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "institutionName")]
        public string InstitutionName { get; set; }

        [JsonProperty(PropertyName = "accountNumber")]
        public string AccountNumber { get; set; }

        [JsonProperty(PropertyName = "value")]
        public double Value { get; set; }
    }
}
