using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Brimma.LOSService.DTO
{
    public class _1003_Mismo34
    {
        [JsonProperty(PropertyName = "borrower")]
        public Borrower Borrower { get; set; }

        [JsonProperty(PropertyName = "coborrowers")]
        public List<CoBorrower> Coborrowers { get; set; } = new List<CoBorrower>();

        [JsonProperty(PropertyName = "assets")]
        public List<Asset> Assets { get; set; } = new List<Asset>();

        [JsonProperty(PropertyName = "realEstate")]
        public List<RealEstate> RealEstate { get; set; } = new List<RealEstate>();

        [JsonProperty(PropertyName = "loanInfo")]
        public LoanInfo LoanInfo { get; set; }

        [JsonProperty(PropertyName = "downpaymentPercentage")]
        public string DownpaymentPercentage { get; set; }
    }
}
