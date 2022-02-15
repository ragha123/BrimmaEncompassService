using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Brimma.LOSService.DTO
{
    public class RealEstate
    {
        [JsonProperty(PropertyName = "address")]
        public Address Address { get; set; }

        [JsonProperty(PropertyName = "ownership")]
        public string Ownership { get; set; }

        [JsonProperty(PropertyName = "intendedOccupancy")]
        public string IntendedOccupancy { get; set; }

        [JsonProperty(PropertyName = "propertyValue")]
        public double PropertyValue { get; set; }

        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; }

        [JsonProperty(PropertyName = "monthlyRentalIncome")]
        public double MonthlyRentalIncome { get; set; }

        [JsonProperty(PropertyName = "monthlyExpenses")]
        public double MonthlyExpenses { get; set; }

        [JsonProperty(PropertyName = "propertyLoans")]
        public List<PropertyLoan> PropertyLoans { get; set; } = new List<PropertyLoan>();
        
    }
}
