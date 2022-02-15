using Brimma.LOSService.Common;
using Brimma.LOSService.DTO;
using Brimma.LOSService.Services;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
/*
    Positive test case – Pass data check
    Positive test case – Pass document check
    Positive test case – Pass milestone check
    Positive test case – Pass conditions check
    Positive test case – Est closing date<45
    Positive test case – Est closing date=45       
    Negative test case – Fail data check
    Negative test case – Fail document check
    Negative test case – Fail milestone check
    Negative test case – Fail conditions check
    Negative test case – Est closing date>45
    Negative test case – Trigger Condition Pass return PreRequisites Check with TriggerCondition as true
    Positive test case – Trigger Condition Fail only TriggerCondition false and no PreRequisites
    If Loan Purpose is other than purchase the returned payload should not have the resources section
    If Loan Purpose is purchase and Purchase contract document exists without an attachment the payload should not have the resources section with only the document entity information
    If Loan Purpose is purchase and Purchase contract document and its attachment exists the payload should not have the resources section with both the document and attachment entity information
*/
namespace Brimma.LOSService.Test
{
    [TestFixture]
    public class OrderOutPreRequisitesChecksTest
    {
        private DependencyResolverHelpercs _serviceProvider;
        private string bearerTokenValue = "0007FKXqHUhNilSlcM7xIvGmxErv";
        private string partnerID = "11158174";
        private string loanGuidDataCheckPass = "4eeb3589-5b66-41be-8822-400a1322fde5";
        private string loanGuidPurchasePurposeDataCheckPass = "4eeb3589-5b66-41be-8822-400a1322fde5";
        private string loanGuidNotPurchasePurposeDataCheckPass = "ecbc32e2-be11-46fe-8ceb-612cce2973ef";        
        private string loanGuidDataEstClosingDateCheckPass = "e21439c6-79bb-4105-bbc7-7ea70e156279";
        private string loanGuidDataEstClosingDateCheckBoundaryPass = "e21439c6-79bb-4105-bbc7-7ea70e156279";
        private string loanGuidDocumentCheckPass = "4eeb3589-5b66-41be-8822-400a1322fde5";
        private string loanGuidMilestoneCheckPass = "4eeb3589-5b66-41be-8822-400a1322fde5";
        private string loanGuidConditionsCheckPass = "4eeb3589-5b66-41be-8822-400a1322fde5";
        private string loanGuidTitleEscrowToCCEmailPass = "b4f0f77c-1910-4617-ae9a-851013472319";
        private string loanGuidDataCheckFail = "4eeb3589-5b66-41be-8822-400a1322fde5";
        private string loanGuidDataEstClosingDateCheckFail = "25d856f2-f261-409e-b729-001232b25e0c";
        private string loanGuidDataTriggerConditionTrueEstClosingDateCheckFail = "25d856f2-f261-409e-b729-001232b25e0c";
        private string loanGuidDataTriggerConditionFailNoPreRequisitesCheck = "4a264135-c041-4cbf-9a26-07455f3303ca";
        private string loanGuidDocumentCheckFail = "4eeb3589-5b66-41be-8822-400a1322fde5";
        private string loanGuidMilestoneCheckFail = "4eeb3589-5b66-41be-8822-400a1322fde5";
        private string loanGuidConditionsCheckFail = "4eeb3589-5b66-41be-8822-400a1322fde5";        
        public OrderOutPreRequisitesChecksTest()
        {
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Development");
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var webHost = WebHost.CreateDefaultBuilder()
                .UseStartup<Startup>()                
                .UseEnvironment(env)               
                .Build();
            _serviceProvider = new DependencyResolverHelpercs(webHost);
        }
        #region Positive test cases
        [Test]
        public async Task Should_Return_Success_For_Appraisal_Order_DataCheck_Pass()
        {
            var request = new
            {
                ordertype = "Appraisal",
                validations = "Data"
            };            
            await positiveTestCase(request, loanGuidDataCheckPass, "data");
        }
        [Test]
        public async Task Should_Return_Success_For_Title_Order_EstClosingDate_DataCheck_Pass()
        {
            var request = new
            {
                ordertype = "Title",
                validations = "Data"
            };
            await positiveTestCase(request, loanGuidDataEstClosingDateCheckPass, "data");
        }
        [Test]
        public async Task Should_Return_Success_For_Title_Order_EstClosingDateBorder_DataCheck_Pass()
        {
            var request = new
            {
                ordertype = "Title",
                validations = "Data"
            };
            await positiveTestCase(request, loanGuidDataEstClosingDateCheckBoundaryPass, "data");
        }
        [Test]
        public async Task Should_Return_Success_For_Appraisal_Order_DocumentCheck_Pass()
        {
            var request = new
            {
                ordertype = "Appraisal",
                validations = "Document"
            };
            await positiveTestCase(request, loanGuidDocumentCheckPass, "document");
        }
        public async Task Should_Return_Success_For_Title_Order_MilestoneCheck_Pass()
        {
            var request = new
            {
                ordertype = "Title",
                validations = "Milestone"
            };
            await positiveTestCase(request, loanGuidMilestoneCheckPass, "milestone");
        }
        public async Task Should_Return_Success_For_Title_Order_ConditionsCheck_Pass()
        {
            var request = new
            {
                ordertype = "Title",
                validations = "Conditions"
            };
            await positiveTestCase(request, loanGuidConditionsCheckPass, "conditions");
        }
        [Test]
        public async Task Should_Return_Payload_WithResourcesSection_For_LoanPurpose_Purchase()
        {
            var request = new
            {
                loanGuid = loanGuidPurchasePurposeDataCheckPass,
                ordertype = "Appraisal",
                requestType = "newOrder"
            };
            await loanPayLoadTestCase(request, partnerID, true);
        }
        [Test]
        public async Task Should_Return_Payload_WithResourcesSection_WithDocumentAndAttachment_For_LoanPurpose_Purchase()
        {
            var request = new
            {
                loanGuid = loanGuidPurchasePurposeDataCheckPass,
                ordertype = "Appraisal",
                requestType = "newOrder"
            };
            await loanPayLoadTestCase(request, partnerID, true,true,true);
        }
        [Test]
        public async Task Should_Return_Order_SubTypes_Email_Data_For_A_Loan()
        {
            GetEncompassAccessToken();
            var orderOutService = _serviceProvider.GetService<IOrderOutService>();
            var response = await orderOutService.GetToCCEmailsForTitleEscrow(loanGuidTitleEscrowToCCEmailPass);
            Assert.IsNotNull(response);
        }
        #endregion
        #region Negative test cases        
        [Test]
        public async Task Should_Return_FailureAndMessages_For_Appraisal_Order_DataCheck_Fail()
        {
            var request = new
            {
                ordertype = "Appraisal",
                validations = "Data"
            };
            await negativeTestCase(request, loanGuidDataCheckFail, "data");
        }
        [Test]
        public async Task Should_Return_FailureAndMessage_For_Title_Order_EstClosingDate_DataCheck_Fail()
        {
            var request = new
            {
                ordertype = "Title",
                validations = "Data"
            };
            await negativeTestCase(request, loanGuidDataEstClosingDateCheckFail, "data", "Estimated Closing Date is not <= 45 days from today");
        }       
        [Test]
        public async Task Should_Return_FailureAndMessages_For_Appraisal_Order_DocumentCheck_Fail()
        {
            var request = new
            {
                ordertype = "Appraisal",
                validations = "Document"
            };
            await negativeTestCase(request, loanGuidDocumentCheckFail, "document");
        }
        [Test]
        public async Task Should_Return_FailureAndMessages_For_Title_Order_MilestoneCheck_Fail()
        {
            var request = new
            {
                ordertype = "Title",
                validations = "Milestone"
            };
            await negativeTestCase(request, loanGuidMilestoneCheckFail, "milestone");
        }
        [Test]
        public async Task Should_Return_FailureAndMessages_For_Title_Order_ConditionsCheck_Fail()
        {
            var request = new
            {
                ordertype = "Title",
                validations = "Conditions"
            };
            await negativeTestCase(request, loanGuidConditionsCheckFail, "conditions");
        }
        [Test]
        public async Task Should_Return_TriggerCondition_True_And_FailureMessage_DataCheck_Fail()
        {
            var request = new
            {
                ordertype = "Title",
                validations = "Data"
            };
            await negativeTestCase(request, loanGuidDataTriggerConditionTrueEstClosingDateCheckFail, "data", "Estimated Closing Date is not <= 45 days from today");
        }
        [Test]
        public async Task Should_Return_TriggerCondition_Fail_And_NoPreRequisitesCheck()
        {
            var request = new
            {
                ordertype = "Title",
                validations = "Data"
            };
            await negativeTestCase(request, loanGuidDataTriggerConditionFailNoPreRequisitesCheck, "data",isTriggerConditionCheck:true);
        }
        [Test]
        public async Task Should_Return_Payload_WithoutResourcesSection_For_LoanPurpose_NotPurchase()
        {
            var request = new
            {
                loanGuid = loanGuidNotPurchasePurposeDataCheckPass,
                ordertype = "Appraisal",
                requestType = "newOrder"
            };
            await loanPayLoadTestCase(request, partnerID, false);
        }
        #endregion
        #region Private methods
        private void GetEncompassAccessToken()
        {
            AppData.AuthResponse = new AuthResponse
            {
                TokenType = "Bearer",
                AccessToken = bearerTokenValue
            };
        }        private async Task positiveTestCase(object request, string loanGuid, string checkType)
        {
            GetEncompassAccessToken();
            var orderOutService = _serviceProvider.GetService<IOrderOutService>();
            var jRequest = JObject.FromObject(request);
            var response = await orderOutService.PlaceHolderMilestoneCheckValidation(loanGuid, jRequest);
            Assert.IsNotNull(response);
            if (response != null)
            {
                object objJValueTriggerCondition = response?.GetType().GetProperty("TriggerCondition")?.GetValue(response, null);
                object objJValuePreRequisites = response?.GetType().GetProperty("PreRequisites")?.GetValue(response, null);
                Assert.AreEqual(objJValueTriggerCondition, true);                     
                Assert.AreEqual(((List<ValidationResultInfo>)objJValuePreRequisites)[0].ValidationType.ToLower(), checkType.ToLower());
                Assert.IsTrue(((List<ValidationResultInfo>)objJValuePreRequisites)[0].Information.ToLower().Contains("successful"));                               
            }
        }
        private async Task negativeTestCase(object request, string loanGuid, string checkType, string ruleViolationMessage="", bool isTriggerConditionCheck=false)
        {
            GetEncompassAccessToken();
            var orderOutService = _serviceProvider.GetService<IOrderOutService>();
            var jRequest = JObject.FromObject(request);
            var response = await orderOutService.PlaceHolderMilestoneCheckValidation(loanGuid, jRequest);
            Assert.IsNotNull(response);
            if (!isTriggerConditionCheck)
            {
                if (response != null)
                {
                    object objJValueTriggerCondition = response?.GetType().GetProperty("TriggerCondition")?.GetValue(response, null);
                    object objJValuePreRequisites = response?.GetType().GetProperty("PreRequisites")?.GetValue(response, null);
                    Assert.AreEqual(objJValueTriggerCondition, true);
                    Assert.AreEqual(((List<ValidationResultInfo>)objJValuePreRequisites)[0].ValidationType.ToLower(), checkType.ToLower());
                    Assert.IsTrue(((List<ValidationResultInfo>)objJValuePreRequisites)[0].Information.ToLower().Contains("failed"));
                    Assert.IsTrue(((List<ValidationResultInfo>)objJValuePreRequisites)[0].ruleViolationMessages.Count > 0);
                    if (!string.IsNullOrEmpty(ruleViolationMessage) && ((List<ValidationResultInfo>)objJValuePreRequisites)[0].ruleViolationMessages.Count == 1)
                    {
                        Assert.IsTrue(((List<ValidationResultInfo>)objJValuePreRequisites)[0].ruleViolationMessages[0].Contains(ruleViolationMessage));
                    }
                }
            }
            else
            {
                if (response != null)
                {
                    object objJValueTriggerCondition = response?.GetType().GetProperty("TriggerCondition")?.GetValue(response, null);
                    object objJValuePreRequisites = response?.GetType().GetProperty("PreRequisites")?.GetValue(response, null);
                    Assert.AreEqual(objJValueTriggerCondition, false);
                    Assert.AreEqual(objJValuePreRequisites, null);                    
                }
            }
            
        }
        private async Task loanPayLoadTestCase(object request, string partnerId, bool isPositiveTestCase=true, bool isDocumentCheck=false, bool isAttachmentCheck=false)
        {
            GetEncompassAccessToken();
            var orderOutService = _serviceProvider.GetService<IOrderOutService>();
            var jRequest = JObject.FromObject(request);
            var response = await orderOutService.PlaceOrder(partnerId, string.Empty, jRequest);
            Assert.IsNotNull(response);                        
            if (response != null)
            {
                bool hasDocument = false;
                bool hasAttachment = false;
                if (isPositiveTestCase)
                {
                    Assert.IsNotNull(((dynamic)response).product.resources);
                    if (isDocumentCheck || isAttachmentCheck)
                    {
                        if (((dynamic)response).product.resources != null)
                        {
                            JArray jArray = (JArray)((dynamic)response).product.resources;
                            if (jArray.Count > 0)
                            {
                                foreach (var item in jArray)
                                {
                                    if (isDocumentCheck)
                                    {
                                        if (((JValue)item["entityType"]).Value.ToString().Contains("document"))
                                        {
                                            hasDocument = true;
                                        }
                                    }
                                    if (isAttachmentCheck)
                                    {
                                        if (((JValue)item["entityType"]).Value.ToString().Contains("attachment"))
                                        {
                                            hasAttachment = true;
                                        }
                                    }                                    
                                }
                            }
                        }
                        if (isDocumentCheck)
                        {
                            Assert.IsTrue(hasDocument);
                        }
                        if (isAttachmentCheck)
                        {
                            Assert.IsTrue(hasAttachment);
                        }
                    }    
                    else
                    {
                        Assert.IsNotNull(((dynamic)response).product.resources);
                    }
                }                
                else
                {
                    Assert.IsNull(((dynamic)response).product.resources);
                }
            }            
        }
        #endregion
    }
}