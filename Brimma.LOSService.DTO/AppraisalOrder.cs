using Newtonsoft.Json;

namespace Brimma.LOSService.DTO
{
    public class product
    {
        public entityRef entityRef;
        public string name { get; set; }
        public credentials credentials;
        public options options;
        public resource[] resources;
    }
    public class payment
    {
        public string paymentType { get; set; }
    }
    public class entityRef
    {
        public string entityId { get; set; }
        public string entityType { get; set; }
    }
    public class credentials
    {
        public string userName { get; set; }
        public string password { get; set; }
        public string accountId { get; set; }
    }

    //ToDo: For Document via Appraisal Order API
    public class resource
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string entityId { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string entityType { get; set; }
    }
    public class options
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string requestType { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? trackingID { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string productID { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string clientGroup { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? amc { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? intentToProceedReceivedDate { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? disclosureDate { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? earlyAppraisalConsent { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? orderedBy { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? notificationEmail { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? dueDate { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? rushOrder { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? complex { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? loanPurpose { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]        
        public string? loanType { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? occupancy { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? propertyType { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string? propertyRights { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public payment payment { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public contacts contacts;
    }
    public class contacts
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string appointmentContact { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public contact contact { get; set; }
    }
    public class contact
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public borrower borrower;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public coBorrower coBorrower;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public agent agent;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public occupant occupant;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public owner owner;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public broker broker;
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public other other;        
    }
    public class borrower: contactBase
    {
    }
    public class coBorrower : contactBase
    {
    }
    public class agent : contactBase
    {
    }
    public class occupant : contactBase
    {
    }
    public class owner : contactBase
    {
    }
    public class broker : contactBase
    {
    }
    public class other : contactBase
    {
    }   
    public abstract class contactBase
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string name { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string workPhone { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string homePhone { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string cellPhone { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string email { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string address { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string city { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string state { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string zip { get; set; }        
    }
}
