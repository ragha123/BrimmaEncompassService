using System;
using System.Collections.Generic;
using System.Text;

namespace Brimma.LOSService.DTO
{
    public class CurrentRateLockDetails
    {
        public string LoanProgram { get; set; }
        public string RateLockInvestor { get; set; }
        public double NetbyRate { get; set; }
        public double NetbyPrice { get; set; }
    }
}
