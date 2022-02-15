using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Brimma.LOSService.DTO
{
    public class BorrowerFinancialHistory
    {
        [JsonProperty(PropertyName = "30DaysMortgageLatestin12Months")]
        public int Days30MortgageLatestin12Months {get;set;}

        [JsonProperty(PropertyName = "60DaysMortgageLatestin12Months")]
        public int Days60MortgageLatestin12Months { get; set; }

        [JsonProperty(PropertyName = "90DaysMortgageLatestin12Months")]
        public int Days90MortgageLatestin12Months { get; set; }

        [JsonProperty(PropertyName = "30DaysMortgageLatestin24Months")]
        public int Days30MortgageLatestin24Months { get; set; }

        [JsonProperty(PropertyName = "60DaysMortgageLatestin24Months")]
        public int Days60MortgageLatestin24Months { get; set; }

        [JsonProperty(PropertyName = "90DaysMortgageLatestin24Months")]
        public int Days90MortgageLatestin24Months { get; set; }

        [JsonProperty(PropertyName = "120DaysMortgageLatestin12Months")]
        public int Days120MortgageLatestin12Months { get; set; }
        public string NoticeOfDefaultForeClosure { get; set; }
        public string BankruptcyInMonths { get; set; }
        public bool DemonstrateHousingPaymentHistory { get; set; }
        public bool FirstTimeHomeBuyers { get; set; }
    }
}
