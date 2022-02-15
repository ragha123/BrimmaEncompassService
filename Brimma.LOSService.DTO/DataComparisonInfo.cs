using System;
using System.Collections.Generic;
using System.Text;

namespace Brimma.LOSService.DTO
{
    public class DataComparisonInfo
    {
        public string Mail { get; set; }
        public string CoMail { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Suffix { get; set; }
        public string CoFirstName { get; set; }
        public string CoMiddleName { get; set; }
        public string CoLastName { get; set; }
        public string CoSuffix { get; set; }
        public string StartDate { get; set; }
        public string EConsentDate { get; set; }
        public string CoEConsentDate { get; set; }
        public string EConsentDateBP1 { get; set; }
        public string CoEConsentDateBP1 { get; set; }
        public string EConsentDateBP2 { get; set; }
        public string CoEConsentDateBP2 { get; set; }
        public string EConsentDateBP3 { get; set; }
        public string CoEConsentDateBP3 { get; set; }
        public string EConsentDateBP4 { get; set; }
        public string CoEConsentDateBP4 { get; set; }
        public string EConsentDateBP5 { get; set; }
        public string CoEConsentDateBP5 { get; set; }
        public string EConsentDateBP6 { get; set; }
        public string CoEConsentDateBP6 { get; set; }
        public bool ShareBorrowerMail { get; set; }
        public string LoanName { get; set; }
        public string LoanNumber { get; set; }
        public string BorrowerPairId { get; set; }
    }
}
