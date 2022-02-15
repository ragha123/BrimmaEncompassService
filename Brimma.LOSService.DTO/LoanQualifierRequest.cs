using System;
using System.Collections.Generic;
using System.Text;

namespace Brimma.LOSService.DTO
{
    public class LoanQualifierRequest
    {
        public LoanQualiferInfo LoanQualifierInfo { get; set; }
        public CurrentRateLockDetails CurrentRateLockDetails { get; set; }
    }
}
