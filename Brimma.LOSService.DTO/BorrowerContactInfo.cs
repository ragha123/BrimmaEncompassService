using Newtonsoft.Json;

namespace Brimma.LOSService.DTO
{
    public class BorrowerContactInfo
    {
        [JsonProperty(PropertyName = "Contact.FirstName")]
        public string FirstName { get; set; }

        [JsonProperty(PropertyName = "Contact.Birthdate")]
        public string Birthdate { get; set; }

        [JsonProperty(PropertyName = "Contact.BizEmail")]
        public string BizEmail { get; set; }

        [JsonProperty(PropertyName = "Contact.MiddleName")]
        public string MiddleName { get; set; }

        [JsonProperty(PropertyName = "Contact.SuffixName")]
        public string SuffixName { get; set; }

        [JsonProperty(PropertyName = "Contact.AccessLevel")]
        public string AccessLevel { get; set; }
    }
}