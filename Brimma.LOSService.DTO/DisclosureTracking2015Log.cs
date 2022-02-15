using System;
using System.Collections.Generic;

namespace Brimma.LOSService.DTO
{
    public class DisclosureTracking2015Log
    {
        public string Id { get; set; }
        public DisclosureTracking2015LogBorrowerPair BorrowerPair { get; set; }
        public bool ContainCD { get; set; }
        public bool ContainLE { get; set; }
        public DisclosureTracking2015LogDisclosedBy DisclosedBy { get; set; }
        public string DisclosedMethod { get; set; }
        public string DisclosedMethodOther { get; set; }
        public DateTime DisclosureCreatedDate { get; set; }
        public DisclosureTracking2015LogEDisclosure EDisclosure { get; set; }
        public List<DisclosureTracking2015LogForms> Forms { get; set; }
        public string DisclosureType { get; set; }
        public DisclosureTracking2015LogLEReason LeReason { get; set; }
        public DisclosureTracking2015LogIntentToProceed IntentToProceed { get; set; }
        public string BorrowerDisclosedMethod { get; set; }
        public string BorrowerDisclosedMethodOther { get; set; }
        public DateTime BorrowerPresumedReceivedDate { get; set; }
        public string CoBorrowerDisclosedMethod { get; set; }
        public DisclosureTracking2015LogSnapshotFields SnapshotFields { get; set; }
        public string LoanProgram { get; set; }
        public DisclosureTracking2015LogPropertyAddress PropertyAddress { get; set; }
        public DateTime DateAdded { get; set; }
        public string DisclosedDate { get; set; }
        public string DisclosureMethod { get; set; }
        public string DisclosedMethodName { get; set; }               
        public int NumberOfDisclosureDocs { get; set; }
        public string AutomaticFulfillmentServiceName { get; set; }
        public string ClosingDate { get; set; }
        public List<string> LogIndicators { get; set; }
    }
}