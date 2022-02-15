using System;
using System.Collections.Generic;
using System.Text;

namespace Brimma.LOSService.DTO
{
    public class LoanQualiferInfo
    {
        public string RequestAction { get; set; }
        public string EppsUserName { get; set; }
        public int[] LockDays { get; set; }
        public string LoanType { get; set; }
        public LoanInformation LoanInformation { get; set; }
        public Compensation Compensation { get; set; }
        public int[] ProductType { get; set; }
        public List<int> ProductOptions { get; set; }
        public int[] SpecialProducts { get; set; }
        public int[] StandardProducts { get; set; }
        public int DocumentationLevel { get; set; }
        public List<object> Borrowers { get; set; }
        public BorrowerFinancialHistory BorrowerFinancialHistory { get; set; }
        public Property Property { get; set; }
    }
}
