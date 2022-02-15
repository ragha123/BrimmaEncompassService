using System;
using System.Collections.Generic;
using System.Text;

namespace Brimma.LOSService.DTO
{
    public class BorrowerFinanancial
    {
        public int CreditScore { get; set; }
        public double LiquidAsset { get; set; }
        public double RetirementAsset { get; set; }
        public double Income { get; set; }
        public double MonthlyDebt { get; set; }
    }
}
