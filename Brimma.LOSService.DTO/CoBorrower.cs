using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Brimma.LOSService.DTO
{
    public class CoBorrower : IBorrowerInfo
    {
        [JsonProperty(PropertyName = "personalInfo")]
        public PersonalInfo PersonalInfo { get; set; }

        [JsonProperty(PropertyName = "currentAddress")]
        public ResidenceAddress CurrentAddress { get; set; }

        [JsonProperty(PropertyName = "hasSeparateMailingAddress")]
        public bool HasSeparateMailingAddress { get; set; }

        [JsonProperty(PropertyName = "mailingAddress")]
        public ResidenceAddress MailingAddress { get; set; }

        [JsonProperty(PropertyName = "formerAddresses")]
        public List<ResidenceAddress> FormerAddresses { get; set; } = new List<ResidenceAddress>();

        [JsonProperty(PropertyName = "hasDependents")]
        public bool HasDependents { get; set; }

        [JsonProperty(PropertyName = "ageOfDependents")]
        public int[] AgeOfDependents { get; set; }

        [JsonProperty(PropertyName = "employmentInfo")]
        public List<EmploymentInfo> EmploymentInfo { get; set; } = new List<EmploymentInfo>();

        [JsonProperty(PropertyName = "demographics")]
        public Demographics Demographics { get; set; }

        [JsonProperty(PropertyName = "declarations")]
        public Declaration Declarations { get; set; }
    }
}
