using Brimma.LOSService.Common;
using Brimma.LOSService.DTO;
using Brimma.LOSService.Services;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Brimma.LOSService.Test
{
    public class OrderOutTitleEscrowUnitTests
    {
        private string bearerTokenValue = "0007HPNDFXH5F82g2nSI9lTsuIQq";
        private LoanCheckInfo dummyLoanTitleOrderDataCheckPass, dummyLoanTitleOrderDocumentCheckPass, dummyLoanTitleOrderMilestoneCheckPass, dummyLoanTitleOrderConditionsCheckPass, dummyLoanTitleOrderDataCheckFail, dummyLoanTitleOrderDocumentCheckFail;
        private string loanGuidTitleOrderDocumentCheckPass = "b4f0f77c-1910-4617-ae9a-851013472319";
        private DependencyResolverHelpercs _serviceProvider;
        public OrderOutTitleEscrowUnitTests()
        {
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Development");
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var webHost = WebHost.CreateDefaultBuilder()
                .UseStartup<Startup>()
                .UseEnvironment(env)
                .Build();
            _serviceProvider = new DependencyResolverHelpercs(webHost);
            SetLoanData();
        }        
        private void SetLoanData()
        {
            dummyLoanTitleOrderDataCheckPass = new LoanCheckInfo()
            {
                LoanGuid = "b4f0f77c-1910-4617-ae9a-851013472319",
                LoanType = "Conventional",
                LoanPurpose = "Purchase",
                PayOffRequested ="Yes",
                VestingType ="Owner",
                ProcessorOpener = "OS - Loan Specialist",
                TransDetailsEstClosingDate = "11/30/2020",
                IntentToProceedSignDate = "9/24/2020",
                LoanFolder = "Active Loans",
                CurrentMilestone = "Sent to Setup",
                PurchaseContractDocAckDate = "10-23-2020 8:22:32 AM",
                IntentToProceedDocAckDate = "09-24-2020 12:44:42 PM",
                BorrowerAuthorization = "09-24-2020 12:44:49 PM",
                TitleEscrowLoanOrdered = "Yes",
                TitleCompanyName = "test title company",
                LoanPropertyType = "Detached",
                TescSalesContDocAckDate = "10-23-2020 8:22:32 AM",
                TescLoanEstDocAckDate = "09-24-2020 12:44:08 PM",
                TescIntProcDocAckDate = "09-24-2020 12:44:42 PM",
                TescBorrAuthDocAckDate = "09-24-2020 12:44:49 PM",
                CurrentFHACaseNumberMilestone = "Sent to Setup",
                CurrentTitleEscrowMilestone = "Sent to Setup",
                PrelimCondition = "2801 - Preliminary Title Report"
            };
            dummyLoanTitleOrderDataCheckFail = new LoanCheckInfo()
            {
                LoanGuid = "b4f0f77c-1910-4617-ae9a-851013472319",
                LoanType = "Conventional",
                LoanPurpose = "Purchase",
                PayOffRequested = "",
                VestingType = "",
                ProcessorOpener = "OS - Loan Specialist",
                TransDetailsEstClosingDate = "12/30/2020",
                IntentToProceedSignDate = "9/24/2020",
                LoanFolder = "Active Loans",
                CurrentMilestone = "Sent to Setup",
                PurchaseContractDocAckDate = "10-23-2020 8:22:32 AM",
                IntentToProceedDocAckDate = "09-24-2020 12:44:42 PM",
                BorrowerAuthorization = "09-24-2020 12:44:49 PM",
                TitleEscrowLoanOrdered = "Yes",
                TitleCompanyName = "test title company",
                LoanPropertyType = "Detached",
                TescSalesContDocAckDate = "10-23-2020 8:22:32 AM",
                TescLoanEstDocAckDate = "09-24-2020 12:44:08 PM",
                TescIntProcDocAckDate = "09-24-2020 12:44:42 PM",
                TescBorrAuthDocAckDate = "09-24-2020 12:44:49 PM",
                CurrentFHACaseNumberMilestone = "Sent to Setup",
                CurrentTitleEscrowMilestone = "Sent to Setup",
                PrelimCondition = "2801 - Preliminary Title Report"
            };
            dummyLoanTitleOrderDocumentCheckPass = new LoanCheckInfo()
            {
                LoanGuid = "b4f0f77c-1910-4617-ae9a-851013472319",
                LoanType = "Conventional",
                LoanPurpose = "Purchase",
                ProcessorOpener = "OS - Loan Specialist",
                TransDetailsEstClosingDate = "12/30/2020",
                IntentToProceedSignDate = "9/24/2020",
                LoanFolder = "Active Loans",
                CurrentMilestone = "Sent to Setup",
                PurchaseContractDocAckDate = "10-23-2020 8:22:32 AM",
                IntentToProceedDocAckDate = "09-24-2020 12:44:42 PM",
                BorrowerAuthorization = "09-24-2020 12:44:49 PM",
                TitleEscrowLoanOrdered = "Yes",
                TitleCompanyName = "test title company",
                LoanPropertyType = "Detached",
                TescSalesContDocAckDate = "10-23-2020 8:22:32 AM",
                TescLoanEstDocAckDate = "09-24-2020 12:44:08 PM",
                TescIntProcDocAckDate = "09-24-2020 12:44:42 PM",
                TescBorrAuthDocAckDate = "09-24-2020 12:44:49 PM",
                CurrentFHACaseNumberMilestone = "Sent to Setup",
                CurrentTitleEscrowMilestone = "Sent to Setup",
                PrelimCondition = "2801 - Preliminary Title Report"
            };
            dummyLoanTitleOrderDocumentCheckFail = new LoanCheckInfo()
            {
                LoanGuid = "b4f0f77c-1910-4617-ae9a-851013472319",
                LoanType = "Conventional",
                LoanPurpose = "Purchase",
                ProcessorOpener = "OS - Loan Specialist",
                TransDetailsEstClosingDate = "12/30/2020",
                IntentToProceedSignDate = "9/24/2020",
                LoanFolder = "Active Loans",
                CurrentMilestone = "Sent to Setup",
                PurchaseContractDocAckDate = "",
                IntentToProceedDocAckDate = "",
                BorrowerAuthorization = "",
                TitleEscrowLoanOrdered = "Yes",
                TitleCompanyName = "test title company",
                LoanPropertyType = "Detached",
                TescSalesContDocAckDate = "10-23-2020 8:22:32 AM",
                TescLoanEstDocAckDate = "09-24-2020 12:44:08 PM",
                TescIntProcDocAckDate = "09-24-2020 12:44:42 PM",
                TescBorrAuthDocAckDate = "09-24-2020 12:44:49 PM",
                CurrentFHACaseNumberMilestone = "Sent to Setup",
                CurrentTitleEscrowMilestone = "Sent to Setup",
                PrelimCondition = "2801 - Preliminary Title Report"
            };
            dummyLoanTitleOrderMilestoneCheckPass = new LoanCheckInfo()
            {
                LoanGuid = "b4f0f77c-1910-4617-ae9a-851013472319",
                LoanType = "Conventional",
                LoanPurpose = "Purchase",
                ProcessorOpener = "OS - Loan Specialist",
                TransDetailsEstClosingDate = "12/30/2020",
                IntentToProceedSignDate = "9/24/2020",
                LoanFolder = "Active Loans",
                CurrentMilestone = "Sent to Setup",
                PurchaseContractDocAckDate = "10-23-2020 8:22:32 AM",
                IntentToProceedDocAckDate = "09-24-2020 12:44:42 PM",
                BorrowerAuthorization = "09-24-2020 12:44:49 PM",
                TitleEscrowLoanOrdered = "Yes",
                TitleCompanyName = "test title company",
                LoanPropertyType = "Detached",
                TescSalesContDocAckDate = "10-23-2020 8:22:32 AM",
                TescLoanEstDocAckDate = "09-24-2020 12:44:08 PM",
                TescIntProcDocAckDate = "09-24-2020 12:44:42 PM",
                TescBorrAuthDocAckDate = "09-24-2020 12:44:49 PM",
                CurrentFHACaseNumberMilestone = "Sent to Setup",
                CurrentTitleEscrowMilestone = "Sent to Setup",
                PrelimCondition = "2801 - Preliminary Title Report"
            };
            dummyLoanTitleOrderConditionsCheckPass = new LoanCheckInfo()
            {
                LoanGuid = "b4f0f77c-1910-4617-ae9a-851013472319",
                LoanType = "Conventional",
                LoanPurpose = "Purchase",
                ProcessorOpener = "OS - Loan Specialist",
                TransDetailsEstClosingDate = "12/30/2020",
                IntentToProceedSignDate = "9/24/2020",
                LoanFolder = "Active Loans",
                CurrentMilestone = "Sent to Setup",
                PurchaseContractDocAckDate = "10-23-2020 8:22:32 AM",
                IntentToProceedDocAckDate = "09-24-2020 12:44:42 PM",
                BorrowerAuthorization = "09-24-2020 12:44:49 PM",
                TitleEscrowLoanOrdered = "Yes",
                TitleCompanyName = "test title company",
                LoanPropertyType = "Detached",
                TescSalesContDocAckDate = "10-23-2020 8:22:32 AM",
                TescLoanEstDocAckDate = "09-24-2020 12:44:08 PM",
                TescIntProcDocAckDate = "09-24-2020 12:44:42 PM",
                TescBorrAuthDocAckDate = "09-24-2020 12:44:49 PM",
                CurrentFHACaseNumberMilestone = "Sent to Setup",
                CurrentTitleEscrowMilestone = "Sent to Setup",
                PrelimCondition = "2801 - Preliminary Title Report"
            };
        }
        [Fact]
        public void Should_Return_Success_For_Title_Order_Data_Check_Pass()
        {
            var request = new
            {
                ordertype = "Title",
                validations = "Data"
            };
            positiveTestCasePreRequisites(request, dummyLoanTitleOrderDataCheckPass, request.validations);
        }
        [Fact]
        public async Task Should_Return_Success_For_Title_Order_Document_Check_Pass_With_Actual_Loan()
        {
            var request = new
            {
                ordertype = "Title",
                validations = "Document"
            };
            var loanResponse = await getLoanWithCheckFields(loanGuidTitleOrderDocumentCheckPass).ConfigureAwait(false);
            if (loanResponse!=null)
            {
                LoanCheckInfo loanData = (LoanCheckInfo)loanResponse;
                positiveTestCasePreRequisites(request, loanData, request.validations);
            }            
        }
        [Fact]
        public void Should_Return_Success_For_Title_Order_Document_Check_Pass_With_Dummy_Loan()
        {            
            var request = new
            {
                ordertype = "Title",
                validations = "Document"
            };
            positiveTestCasePreRequisites(request, dummyLoanTitleOrderDocumentCheckPass, request.validations);            
        }
        [Fact]
        public void Should_Return_Success_For_Title_Order_Milestone_Check_Pass()
        {
            var request = new
            {
                ordertype = "Title",
                validations = "Milestone"
            };
            positiveTestCasePreRequisites(request, dummyLoanTitleOrderMilestoneCheckPass, request.validations);
        }
        [Fact]
        public void Should_Return_Success_For_Title_Order_Conditions_Check_Pass()
        {
            var request = new
            {
                ordertype = "Title",
                validations = "Conditions"
            };
            positiveTestCasePreRequisites(request, dummyLoanTitleOrderConditionsCheckPass, request.validations);
        }
        [Fact]
        public void Should_Return_Failure_For_Title_Order_Data_Check_Fail()
        {
            var request = new
            {
                ordertype = "Title",
                validations = "Data"
            };
            negativeTestCasePreRequisites(request, dummyLoanTitleOrderDataCheckFail, request.validations);
        }
        private void GetEncompassAccessToken()
        {
            AppData.AuthResponse = new AuthResponse
            {
                TokenType = "Bearer",
                AccessToken = bearerTokenValue
            };
        }
        private async Task<Object> getLoanWithCheckFields(string loanGuid)
        {
            GetEncompassAccessToken();
            Object loanData = new object();
            var orderOutService = _serviceProvider.GetService<IOrderOutService>();
            var response = await orderOutService.GetLoanWithCheckFields(loanGuid).ConfigureAwait(false);            
            return response;
        }
        private void positiveTestCasePreRequisites(object request,LoanCheckInfo loan, string checkType)
        {
            var jRequest = JObject.FromObject(request);
            var orderOutService = _serviceProvider.GetService<IOrderOutService>();
            var response = orderOutService.GetValidation(loan, jRequest);
            Assert.NotNull(response);
            if (response != null)
            {
                object objJValueTriggerCondition = response?.GetType().GetProperty("TriggerCondition")?.GetValue(response, null);
                object objJValuePreRequisites = response?.GetType().GetProperty("PreRequisites")?.GetValue(response, null);
                Assert.Equal(true, objJValueTriggerCondition);
                Assert.Contains(checkType.ToLower(),((List<ValidationResultInfo>)objJValuePreRequisites)[0].ValidationType.ToLower());
                Assert.Contains("successful", ((List<ValidationResultInfo>)objJValuePreRequisites)[0].Information.ToLower());
            }
        }
        private void negativeTestCasePreRequisites(object request, LoanCheckInfo loan, string checkType, string ruleViolationMessage = "", bool isTriggerConditionCheck = false)
        {            
            var orderOutService = _serviceProvider.GetService<IOrderOutService>();
            var jRequest = JObject.FromObject(request);
            var response = orderOutService.GetValidation(loan, jRequest);
            Assert.NotNull(response);
            if (!isTriggerConditionCheck)
            {
                if (response != null)
                {
                    object objJValueTriggerCondition = response?.GetType().GetProperty("TriggerCondition")?.GetValue(response, null);
                    object objJValuePreRequisites = response?.GetType().GetProperty("PreRequisites")?.GetValue(response, null);
                    Assert.Equal(true, objJValueTriggerCondition);
                    Assert.Contains(checkType.ToLower(),((List<ValidationResultInfo>)objJValuePreRequisites)[0].ValidationType.ToLower());
                    Assert.Contains("failed",((List<ValidationResultInfo>)objJValuePreRequisites)[0].Information.ToLower());
                    Assert.True(((List<ValidationResultInfo>)objJValuePreRequisites)[0].ruleViolationMessages.Count > 0);
                    if (!string.IsNullOrEmpty(ruleViolationMessage) && ((List<ValidationResultInfo>)objJValuePreRequisites)[0].ruleViolationMessages.Count == 1)
                    {
                        Assert.Contains(ruleViolationMessage, ((List<ValidationResultInfo>)objJValuePreRequisites)[0].ruleViolationMessages[0]);
                    }
                }
            }
            else
            {
                if (response != null)
                {
                    object objJValueTriggerCondition = response?.GetType().GetProperty("TriggerCondition")?.GetValue(response, null);
                    object objJValuePreRequisites = response?.GetType().GetProperty("PreRequisites")?.GetValue(response, null);
                    Assert.Equal(false, objJValueTriggerCondition);
                    Assert.Null(objJValuePreRequisites);
                }
            }

        }
    }
}
