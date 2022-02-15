namespace Brimma.LOSService.Config
{
    public class ErrorMessages
    {
        public string InternalError { get; set; }
        public string LoanGuidBadRequestError { get; set; }
        public string DocumentIdBadRequestError { get; set; }
        public string DocumentTitlesError { get; set; }
        public string OriginalAttachmentsUrlsInternalError { get; set; }
        public string AttachmentIdBadRequestError { get; set; }
        public string DateFormatBadRequest { get; set; }
        public string StartBadRequest { get; set; }
        public string RequestEmptyBadRequest { get; set; }
        public string WebhookSubscriptionBodyEndpointBadRequest { get; set; }
        public string WebhookSubscriptionBodyResourceBadRequest { get; set; }
        public string WebhookSubscriptionBodyFiltersBadRequest { get; set; }
        public string WebhookSubscriptionBodyEventsBadRequest { get; set; }
        public string PartnerIdBadRequestError { get; set; }
        public string TransactionIdBadRequestError { get; set; }
        public string UserIdBadRequestError { get; set; }
        public string InvalidRequest { get; set; }
        public string InvalidCustomDataObjectName { get; set; }
        public string RateLockNumberOfDaysRequiredError { get; set; }
        public string RateLockBadRequestError { get; set; }
        public string RateLockExtensionError { get; set; }
        public string RateLockError { get; set; } 
        public string FieldIdsError { get; set; }
        public string ConditionTitlesBadRequestError { get; set; }
        public string DocumentIdsError { get; set; }
        public string LoanTemplatePathBadRequestError { get; set; }
        public string LoanProgramTemplatePathBadRequestError { get; set; }
        public string ClosingCostTemplatePathBadRequestError { get; set; }
        public string BusinessContactIdBadRequestError { get; set; }
    }
}