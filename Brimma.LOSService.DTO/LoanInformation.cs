using System;
using System.Collections.Generic;
using System.Text;

namespace Brimma.LOSService.DTO
{
    public class LoanInformation
    {
        public int LienPosition { get; set; }
        public int LoanPurpose { get; set; }
        public double FirstMortgageAmount { get; set; }
        public double SecondMortgageAmount { get; set; }
        public double TotalMortgageAmount { get; set; }
        public double FinancedAmount { get; set; }
        public double OtherPayment { get; set; }
        public double CashOut { get; set; }
        public double Target { get; set; }
        public bool IncludeClosingCost { get; set; }
        public bool NoClosingCost { get; set; }
        public int LoanChannel { get; set; }
        public string LoanId { get; set; }
    }
}
