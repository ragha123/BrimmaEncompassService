using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Brimma.LOSService.DTO
{
    public class PersonalInfo
    {
        [JsonProperty(PropertyName = "dateOfBirth")]
        public string DateOfBirth { get; set; }

        [JsonProperty(PropertyName = "citizenship")]
        public string Citizenship { get; set; }

        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }

        [JsonProperty(PropertyName = "legalFullName")]
        public LegalFullName LegalFullName { get; set; }

        [JsonProperty(PropertyName = "maritalStatus")]
        public string MaritalStatus { get; set; }

        [JsonProperty(PropertyName = "homePhoneNumber")]
        public string HomePhoneNumber { get; set; }

        [JsonProperty(PropertyName = "mobilePhoneNumber")]
        public string MobilePhoneNumber { get; set; }

        [JsonProperty(PropertyName = "workPhoneNumber")]
        public string WorkPhoneNumber { get; set; }

        [JsonProperty(PropertyName = "socialSecurityNumber")]
        public string SocialSecurityNumber { get; set; }
    }
}
