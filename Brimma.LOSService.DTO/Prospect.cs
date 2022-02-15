using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Brimma.LOSService.DTO
{
    public class Prospect
    {
        [JsonProperty(PropertyName = "incomplete")]
        public bool Incomplete { get; set; }

        [JsonProperty(PropertyName = "mobile")]
        public string Mobile { get; set; }

        [JsonProperty(PropertyName = "smsEnabled")]
        public bool SmsEnabled { get; set; }

        [JsonProperty(PropertyName = "coMobile")]
        public string CoMobile { get; set; }

        [JsonProperty(PropertyName = "coSmsEnabled")]
        public bool CoSmsEnabled { get; set; }

        [JsonProperty(PropertyName = "coUseBorrowerMail")]
        public bool CoUseBorrowerMail { get; set; }

        [JsonProperty(PropertyName = "loanName")]
        public string LoanName { get; set; }

        [JsonProperty(PropertyName = "loanOwner")]
        public string LoanOwner { get; set; }

        [JsonProperty(PropertyName = "teamManager")]
        public string TeamManager { get; set; }

        [JsonProperty(PropertyName = "borrowerAccess")]
        public bool BorrowerAccess { get; set; }

        [JsonProperty(PropertyName = "termsEconsentBorrowerAccepted")]
        public bool TermsEconsentBorrowerAccepted { get; set; }

        [JsonProperty(PropertyName = "termsEconsentCoborrowerAccepted")]
        public bool TermsEconsentCoborrowerAccepted { get; set; }

        [JsonProperty(PropertyName = "termsCreditAuthorizationBorrowerAccepted")]
        public bool TermsCreditAuthorizationBorrowerAccepted { get; set; }

        [JsonProperty(PropertyName = "termsCreditAuthorizationCoborrowerAccepted")]
        public bool TermsCreditAuthorizationCoborrowerAccepted { get; set; }

        [JsonProperty(PropertyName = "1003-mismo34")]
        public _1003_Mismo34 _1003_mismo34 { get; set; }
    }
}
