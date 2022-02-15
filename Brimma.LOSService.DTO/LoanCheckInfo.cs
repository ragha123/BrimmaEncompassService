namespace Brimma.LOSService.DTO
{
    public class LoanCheckInfo:ILoanCheckInfo
    {
        public string LoanGuid { get; set; } = string.Empty;
        public string LoanType { get; set; } = string.Empty;
        public string LoanPurpose { get; set; } = string.Empty;
        public string AppraisalLoanOrdered { get; set; } = string.Empty;
        public string AppraisalProduct { get; set; } = string.Empty;
        public string AppraisalManagementCompany { get; set; } = string.Empty;
        public string PaymentMethod { get; set; } = string.Empty;
        public string AppraisalDueDate { get; set; } = string.Empty;
        public string FHACaseNumberLoanOrdered { get; set; } = string.Empty;
        public string ProcessorOpener { get; set; } = string.Empty;
        public string IntentToProceedDocComment { get; set; } = string.Empty;
        public string PayOffRequested { get; set; } = string.Empty;
        public string VestingType { get; set; } = string.Empty;
        public string TransDetailsEstClosingDate { get; set; } = string.Empty;
        public string DPAProgramUtilized { get; set; } = string.Empty;
        public string IntentToProceedSignDate { get; set; } = string.Empty;
        public string LoanFolder { get; set; } = string.Empty;
        public string CurrentMilestone { get; set; } = string.Empty;
        public string PurchaseContractDocAckDate { get; set; } = string.Empty;
        public string IntentToProceedDocAckDate { get; set; } = string.Empty;
        public string VACertificateOfEligibility { get; set; } = string.Empty;
        public string BorrowerAuthorization { get; set; } = string.Empty;
        public string TitleEscrowLoanOrdered { get; set; } = string.Empty;
        public string TitleCompanyName { get; set; } = string.Empty;
        public string TitlePrelim { get; set; } = string.Empty;
        public string ClosingPrtLetter { get; set; } = string.Empty;
        public string WireInstructions { get; set; } = string.Empty;
        public string TitleFees { get; set; } = string.Empty;
        public string LoanPropertyType { get; set; } = string.Empty;
        public string Appraisal2203CondoDocAckDate { get; set; } = string.Empty;
        public string Appraisal2204CondoDocAckDate { get; set; } = string.Empty;
        public string Appraisal2205CondoDocAckDate { get; set; } = string.Empty;
        public string Appraisal2206CondoDocAckDate { get; set; } = string.Empty;
        public string TescSalesContDocAckDate { get; set; } = string.Empty;
        public string TescLoanEstDocAckDate { get; set; } = string.Empty;
        public string TescIntProcDocAckDate { get; set; } = string.Empty;
        public string TescBorrAuthDocAckDate { get; set; } = string.Empty;
        public string CurrentFHACaseNumberMilestone { get; set; } = string.Empty;
        public string CurrentTitleEscrowMilestone { get; set; } = string.Empty;
        public string UWCondition { get; set; } = string.Empty;
        public string PrelimCondition { get; set; } = string.Empty;
        public string AppraisalOrderAutomated { get; set; } = string.Empty;
        public string TitleEscrowOrderAutomated { get; set; } = string.Empty;
        public string RequestPayoffFrom { get; set; } = string.Empty;
    }
}
