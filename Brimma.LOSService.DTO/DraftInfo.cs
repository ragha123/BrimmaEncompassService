using System;
using System.Threading.Tasks;

namespace Brimma.LOSService.DTO
{
    public class DraftInfo
    {
        public string id { get; set; }
        public string userId { get; set; }
        public string borrowerName { get; set; }
        public string subjectPropertyAddress { get; set; }
        public string loanPurpose { get; set; }
        public double loanAmount { get; set; }
        public double noteRate { get; set; }
        public string loanType { get; set; }
        public string mileStone { get; set; }
        public DateTime createdDate { get; set; }
        public DateTime lastModifieddate { get; set; }
        public object loan { get; set; }
        public string loId { get; set; }       
        public string loaId { get; set; }
        public string loEmail { get; set; }
        public string loanProgramPath { get; set; }
        public string closingCostPath { get; set; }
    }
}
