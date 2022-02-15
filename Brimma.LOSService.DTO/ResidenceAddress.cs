using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Brimma.LOSService.DTO
{
    public class ResidenceAddress
    {
        [JsonProperty(PropertyName = "street")]
        public string Street { get; set; }

        [JsonProperty(PropertyName = "unit")]
        public string Unit { get; set; }

        [JsonProperty(PropertyName = "city")]
        public string City { get; set; }

        [JsonProperty(PropertyName = "stateOrProvince")]
        public string StateOrProvince { get; set; }

        [JsonProperty(PropertyName = "postalCode")]
        public string PostalCode { get; set; }

        [JsonProperty(PropertyName = "country")]
        public string Country { get; set; }

        [JsonProperty(PropertyName = "monthsAtAddress")]
        public int MonthsAtAddress { get; set; }

        [JsonProperty(PropertyName = "residencyBasis")]
        public string ResidencyBasis { get; set; }
    }
}
