using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Brimma.LOSService.DTO
{
    public class LoanInfo
    {
        [JsonProperty(PropertyName = "buyingState")]
        public string BuyingState { get; set; }

        [JsonProperty(PropertyName = "loanAmount")]
        public double LoanAmount { get; set; }

        [JsonProperty(PropertyName = "loanPurpose")]
        public string LoanPurpose { get; set; }

        [JsonProperty(PropertyName = "propertyAddress")]
        public Address PropertyAddress { get; set; }

        [JsonProperty(PropertyName = "numberOfUnits")]
        public int NumberOfUnits { get; set; }

        [JsonProperty(PropertyName = "propertyValue")]
        public double PropertyValue { get; set; }

        [JsonProperty(PropertyName = "occupancy")]
        public string Occupancy { get; set; }

        [JsonProperty(PropertyName = "typeOfProperty")]
        public string TypeOfProperty { get; set; }
    }
}
