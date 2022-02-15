namespace Brimma.LOSService.DTO
{
    public interface ILoanCheckInfo
    {
        public string LoanGuid { get; set; }
        public string LoanType { get; set; }
        public string LoanPurpose { get; set; }
        public string AppraisalLoanOrdered { get; set; }
        public string AppraisalProduct { get; set; }
        public string AppraisalManagementCompany { get; set; }
        public string PaymentMethod { get; set; }
        public string AppraisalDueDate { get; set; }
        public string FHACaseNumberLoanOrdered { get; set; }
        public string ProcessorOpener { get; set; }
        public string IntentToProceedDocComment { get; set; }
        public string PayOffRequested { get; set; }
        public string VestingType { get; set; }
        public string TransDetailsEstClosingDate { get; set; }
        public string DPAProgramUtilized { get; set; }
        public string IntentToProceedSignDate { get; set; }
        public string LoanFolder { get; set; }
        public string CurrentMilestone { get; set; }
        public string PurchaseContractDocAckDate { get; set; }
        public string IntentToProceedDocAckDate { get; set; }
        public string VACertificateOfEligibility { get; set; }
        public string BorrowerAuthorization { get; set; }
        public string TitleEscrowLoanOrdered { get; set; }
        public string TitleCompanyName { get; set; }
        public string LoanPropertyType { get; set; }
        public string Appraisal2203CondoDocAckDate { get; set; }
        public string Appraisal2204CondoDocAckDate { get; set; }
        public string Appraisal2205CondoDocAckDate { get; set; }
        public string Appraisal2206CondoDocAckDate { get; set; }
        public string TescSalesContDocAckDate { get; set; }
        public string TescLoanEstDocAckDate { get; set; }
        public string TescIntProcDocAckDate { get; set; }
        public string TescBorrAuthDocAckDate { get; set; }
        public string CurrentFHACaseNumberMilestone { get; set; }
        public string CurrentTitleEscrowMilestone { get; set; }
        public string UWCondition { get; set; }
        public string PrelimCondition { get; set; }                        
    }
}
