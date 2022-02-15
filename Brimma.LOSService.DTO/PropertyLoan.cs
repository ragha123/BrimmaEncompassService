using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Brimma.LOSService.DTO
{
    public class PropertyLoan
    {
        [JsonProperty(PropertyName = "creditorName")]
        public string CreditorName { get; set; }

        [JsonProperty(PropertyName = "accountNumber")]
        public string AccountNumber { get; set; }

        [JsonProperty(PropertyName = "monthlyPayment")]
        public double MonthlyPayment { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "balance")]
        public double Balance { get; set; }

        [JsonProperty(PropertyName = "isToBePaidOff")]
        public bool IsToBePaidOff { get; set; }

        [JsonProperty(PropertyName = "creditLimit")]
        public double CreditLimit { get; set; }
        
    }
}
