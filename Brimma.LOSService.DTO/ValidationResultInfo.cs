using System.Collections.Generic;

namespace Brimma.LOSService.DTO
{
    public class ValidationResultInfo
    {
        public bool Success { get; set; }
        public string Information { get; set; }
        public string ValidationType { get; set; }
        public string loanGuid { get; set; }
        public List<string> ruleViolationMessages { get; set; }
    }
}