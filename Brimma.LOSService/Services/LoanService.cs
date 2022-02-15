using Azure.Storage.Queues;
using Brimma.LOSService.Config;
using Brimma.LOSService.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Brimma.LOSService.Common;
using System.Globalization;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;

namespace Brimma.LOSService.Services
{
    public class LoanService : ControllerBase, ILoanService
    {
        private readonly IHttpService httpService;
        private readonly EncompassAPIs encompassAPIs;
        private readonly AppSettings appSettings;
        private readonly AzureQueue azureQueue;
        public IConfiguration configuration { get; set; }
        private readonly ILogger<LoanService> logger;
        private readonly SaveNLog saveNLogger;
        private CultureInfo cultureInfo;
        private readonly ErrorMessages errorMessages;
        private readonly LoanConfiguration loanConfiguration;
        private readonly EPPSConfiguration ePPSConfiguration;

        public LoanService(IHttpService httpService, IOptions<EncompassAPIs> options, IOptions<AppSettings> appSettingsOptions, IOptions<AzureQueue> azureQueueOptions,
             IConfiguration configuration, ILogger<LoanService> logger, SaveNLog saveNLogger, IOptions<ErrorMessages> errorMessagesoptions,
             IOptions<LoanConfiguration> loanConfigurationOptions, IOptions<EPPSConfiguration> ePPSConfigurationoptions)
        {
            this.httpService = httpService;
            encompassAPIs = options.Value;
            appSettings = appSettingsOptions.Value;
            azureQueue = azureQueueOptions.Value;
            this.configuration = configuration;
            this.logger = logger;
            this.saveNLogger = saveNLogger;
            this.cultureInfo = new CultureInfo("en-US");
            errorMessages = errorMessagesoptions.Value;
            this.loanConfiguration = loanConfigurationOptions.Value;
            ePPSConfiguration = ePPSConfigurationoptions.Value;
        }

        public async Task<Object> GetLoan(string loanGuid, string entities)
        {
            Object response = new Object();
            string apiUrl = string.Empty;
            try
            {
                if (string.IsNullOrEmpty(entities))
                {
                    apiUrl = string.Format(encompassAPIs.RetrieveLoan, loanGuid);
                }
                else
                {
                    apiUrl = string.Format(encompassAPIs.RetrieveLoanWithSpecificEntities, loanGuid, entities);
                }
                ApiResponse<Object> apiResponse = await httpService.GetAsync<Object>(apiUrl).ConfigureAwait(false);
                if (apiResponse.Success)
                {
                    if (!string.IsNullOrEmpty(entities))
                    {
                        response = apiResponse.Data;
                    }
                    else
                    {
                        JObject result = (JObject)apiResponse.Data;
                        response = ExtractLoanInfoFromResponse(result);
                    }
                }
                else if (apiResponse.ErrorResponse != null && apiResponse.ErrorResponse.Error != null)
                {
                    logger.LogError(string.Format("Error Occued at GetLoan. Error Response= {0};", apiResponse.ErrorResponse));
                    return StatusCode(apiResponse.ErrorResponse.Error.Code, apiResponse.ErrorResponse);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(string.Format("Error Occued at GetLoan. Error Message= {0}; Stack Trace={1}", ex.Message, ex.StackTrace));
                return StatusCode(500, ErrorHandling.GetErrorResponse(500, string.Format("Error Occued at GetLoan. Error Message= {0}; Stack Trace={1}", ex.Message, ex.StackTrace)));
            }
            return response;
        }


        private dynamic ExtractLoanInfoFromResponse(JObject loanInfo)
        {
            var applications = new List<object>();
            for (int i = 0; i < loanInfo["borrowerPairCount"].ToObject<int>(); i++)
            {
                var application = new
                {
                    assets = loanInfo["applications"][i]["assets"] != null ? ExtractAssetInfoFromLoanResponse(loanInfo, i) : null,
                    borrower = loanInfo["applications"][i]["borrower"] != null ? ExtractBorrowerInfoFromLoanResponse(loanInfo, i) : null,
                    coborrower = (loanInfo["applications"][i]["coborrower"]["firstName"] != null && loanInfo["applications"][i]["coborrower"]["lastName"] != null) ? ExtractCoBorrowerInfoFromLoanResponse(loanInfo, i) : null,
                    employment = loanInfo["applications"][i]["employment"] != null ? ExtractEmploymentInfoFromLoanResponse(loanInfo, i) : null,
                    income = loanInfo["applications"][i]["income"] != null ? ExtractIncomeInfoFromLoanResponse(loanInfo, i) : null,
                    propertyUsageType = loanInfo["applications"][i]["propertyUsageType"],
                    residences = loanInfo["applications"][i]["residences"] != null ? ExtractResidencesInfoFromLoanResponse(loanInfo, i) : null,
                    otherAssets = loanInfo["applications"][i]["otherAssets"] != null ? ExtractOtherAssestInfoFromLoanResponse(loanInfo, i) : Array.Empty<object>(),
                    totalIncomeAmount = loanInfo["applications"][i]["totalIncomeAmount"],
                    reoProperties = loanInfo["applications"][i]["reoProperties"] != null ? ExtractReoPropertiesInfoFromLoanResponse(loanInfo, i) : null,
                    totalURLA2020AssetsAmount = loanInfo["applications"][i]["totalURLA2020AssetsAmount"],
                    totalOtherAssetsAmount = loanInfo["applications"][i]["totalOtherAssetsAmount"],
                    liabilities = loanInfo["applications"][i]["liabilities"] != null ? ExtractLiabilitiesInfoFromLoanResponse(loanInfo, i) : null,
                    otherLiabilities = loanInfo["applications"][i]["otherLiabilities"] != null ? ExtractOtherLiabilitiesInfoFromLoanResponse(loanInfo, i) : null,
                    jointAssetLiabilityReportingIndicator = loanInfo["applications"][i]["jointAssetLiabilityReportingIndicator"] != null ? loanInfo["applications"][i]["jointAssetLiabilityReportingIndicator"] : null,
                    incomeOtherThanBorrowerUsedIndicator = loanInfo["applications"][i]["incomeOtherThanBorrowerUsedIndicator"] != null ? loanInfo["applications"][i]["incomeOtherThanBorrowerUsedIndicator"] : null,
                    incomeOfBorrowersSpouseUsedIndicator = loanInfo["applications"][i]["incomeOfBorrowersSpouseUsedIndicator"] != null ? loanInfo["applications"][i]["incomeOfBorrowersSpouseUsedIndicator"] : null,
                };
                applications.Add(application);
            }
            var disclosureTracking2015Log = new List<object>();
            object disclosedForLE = loanInfo["disclosureTracking2015Logs"] != null ? (loanInfo["disclosureTracking2015Logs"][0] != null ? loanInfo["disclosureTracking2015Logs"][0]["disclosedForLE"] : null) : null;
            if(disclosedForLE != null)
            {
                disclosureTracking2015Log.Add(new { disclosedForLE } );
            }
            dynamic loan = new
            {
                encompassId = loanInfo["encompassId"],
                applications = applications,
                borrowerPairCount = loanInfo["borrowerPairCount"],
                borrowerRequestedLoanAmount = loanInfo["borrowerRequestedLoanAmount"],
                closingCost = new 
                {
                    loanEstimate1 = new
                    {
                        disclosureLastSentDate = loanInfo["closingCost"]["loanEstimate1"]["disclosureLastSentDate"]
                    }
                },
                closingCostProgram = loanInfo["closingCostProgram"],
                commitmentTerms = new
                {
                    manufacturedHousing = loanInfo["commitmentTerms"]["manufacturedHousing"]
                },
                contacts = ExtractContactsInfoFromLoanResponse(loanInfo),
                closingDate = loanInfo["fhaVaLoan"]["closingDate"],
                disclosureTracking2015Logs = disclosureTracking2015Log.Count > 0 ? disclosureTracking2015Log : null,
                interviewerLicenseIdentifier = loanInfo["interviewerLicenseIdentifier"],
                interviewersCompanyStateLicense = loanInfo["interviewersCompanyStateLicense"],
                inverviewerName = loanInfo["inverviewerName"],
                loanProductData = new
                {
                    nmlsRefinancePurposeType = loanInfo["loanProductData"]["nmlsRefinancePurposeType"],
                    gsePropertyType = loanInfo["loanProductData"]["gsePropertyType"],
                    scheduledFirstPaymentDate = loanInfo["loanProductData"]["scheduledFirstPaymentDate"]
                },
                loanProgramName = loanInfo["loanProgramName"],
                mortgageType = loanInfo["mortgageType"],
                nmlsLoanOriginatorId = loanInfo["nmlsLoanOriginatorId"],
                print2003Application = loanConfiguration.URLAVersion,
                property = ExtractPropertyInfoFromLoanResponse(loanInfo),
                propertyEstimatedValueAmount = loanInfo["propertyEstimatedValueAmount"],
                propertyUsageType = loanInfo["propertyUsageType"],
                regulationZ = new
                {
                    constructionRefinanceIndicator = loanInfo["regulationZ"]["constructionRefinanceIndicator"]
                },
                requestedInterestRatePercent = loanInfo["requestedInterestRatePercent"],
                subjectPropertyGrossRentalIncomeAmount = loanInfo["subjectPropertyGrossRentalIncomeAmount"],
                subjectPropertyOccupancyPercent = loanInfo["subjectPropertyOccupancyPercent"],
                interviewerEmail = loanInfo["interviewerEmail"],
                loanNumber = loanInfo["loanNumber"],
                loanAmortizationTermMonths = loanInfo["loanAmortizationTermMonths"],
                originatorFirstName = loanInfo["originatorFirstName"],
                originatorLastName = loanInfo["originatorLastName"],
                originatorMiddleName = loanInfo["originatorMiddleName"],
                originatorSuffixName = loanInfo["originatorSuffixName"]
            };

            return loan;
        }
        private dynamic ExtractAssetInfoFromLoanResponse(JObject loanInfo, int i)
        {
            var assetsList = new List<Object>();
            var assets = JsonConvert.DeserializeObject<dynamic>(loanInfo["applications"][i]["assets"].ToString());
            foreach (var assetItem in assets)
            {
                if (assetItem["altId"] != null)
                {
                    if (assetItem["id"] != null)
                    {
                        assetItem.Property("id").Remove();
                    }
                    assetItem.Property("altId").Remove();
                    if (assetItem["borrowerId"] != null)
                    {
                        assetItem.Property("borrowerId").Remove();
                    }
                    if (assetItem["assetIndex"] != null)
                    {
                        assetItem.Property("assetIndex").Remove();
                    }
                    if (assetItem["owner"] == null || (assetItem["owner"] != null && assetItem["owner"].ToString() == ""))
                    {
                        assetItem["owner"] = "Borrower";
                    }
                    assetsList.Add(assetItem);
                }
            }
            return assetsList;
        }

        private dynamic ExtractOtherAssestInfoFromLoanResponse(JObject loanInfo, int i)
        {
            var otherAssets = JsonConvert.DeserializeObject<dynamic>(loanInfo["applications"][i]["otherAssets"].ToString());
            foreach (var otherAssetItem in otherAssets)
            {
                if (otherAssetItem["id"] != null)
                {
                    otherAssetItem.Property("id").Remove();
                }
                if (otherAssetItem["altId"] != null)
                {
                    otherAssetItem.Property("altId").Remove();
                }
                if (otherAssetItem["borrowerType"] == null || (otherAssetItem["borrowerType"] != null && otherAssetItem["borrowerType"].ToString() == ""))
                {
                    otherAssetItem["borrowerType"] = "Borrower";
                }
            }
            return otherAssets;
        }

        private dynamic ExtractLiabilitiesInfoFromLoanResponse(JObject loanInfo, int i)
        {
            var liability = new List<Object>();
            var liabilities = JsonConvert.DeserializeObject<dynamic>(loanInfo["applications"][i]["liabilities"].ToString());
            foreach (var liabilityItem in liabilities)
            {
                if (liabilityItem["id"] != null)
                {
                    liabilityItem.Property("id").Remove();
                }
                if (liabilityItem["owner"] == null || (liabilityItem["owner"] != null && liabilityItem["owner"].ToString() == ""))
                {
                    liabilityItem["owner"] = "Borrower";
                }
                liability.Add(liabilityItem);
            }
            return liability;
        }

        private dynamic ExtractOtherLiabilitiesInfoFromLoanResponse(JObject loanInfo, int i)
        {
            var otherLiability = new List<Object>();
            var otherLiabilities = JsonConvert.DeserializeObject<dynamic>(loanInfo["applications"][i]["otherLiabilities"].ToString());
            foreach (var otherLiabilityItem in otherLiabilities)
            {
                if (otherLiabilityItem["id"] != null)
                {
                    otherLiabilityItem.Property("id").Remove();
                }
                if (otherLiabilityItem["altId"] != null)
                {
                    otherLiabilityItem.Property("altId").Remove();
                }
                if (otherLiabilityItem["borrowerType"] == null || (otherLiabilityItem["borrowerType"] != null && otherLiabilityItem["borrowerType"].ToString() == ""))
                {
                    otherLiabilityItem["borrowerType"] = "Borrower";
                }
                otherLiability.Add(otherLiabilityItem);
            }
            return otherLiability;
        }

        private dynamic ExtractBorrowerInfoFromLoanResponse(JObject loanInfo, int i)
        {
            string borrowerMiddleName = string.Empty;
            string borrowerSuffixName = string.Empty;
            if (loanInfo["applications"][i]["borrower"]["firstName"] != null && loanInfo["applications"][i]["borrower"]["firstNameWithMiddleName"] != null)
            {
                string borrowerFirstAndMiddleName = loanInfo["applications"][i]["borrower"]["firstNameWithMiddleName"].ToString();
                string borrowerFirstName = loanInfo["applications"][i]["borrower"]["firstName"].ToString();
                borrowerMiddleName = borrowerFirstAndMiddleName.Replace(borrowerFirstName.Trim(), "").Trim();
            }
            if (loanInfo["applications"][i]["borrower"]["lastName"] != null && loanInfo["applications"][i]["borrower"]["lastNameWithSuffix"] != null)
            {
                string borrowerLastNameandSuffixName = loanInfo["applications"][i]["borrower"]["lastNameWithSuffix"].ToString();
                string borrowerLastName = loanInfo["applications"][i]["borrower"]["lastName"].ToString();
                borrowerSuffixName = borrowerLastNameandSuffixName.Replace(borrowerLastName.Trim(), "").Trim();
            }

            var borrower = new
            {
                applicantType = loanInfo["applications"][i]["borrower"]["applicantType"],
                birthDate = loanInfo["applications"][i]["borrower"]["birthDate"], // != null ? loanInfo["applications"][i]["borrower"]["birthDate"].ToObject<DateTime>().ToString("yyyy-MM-dd", cultureInfo): null,
                borrowerType = loanInfo["applications"][i]["borrower"]["borrowerType"],
                urla2020CitizenshipResidencyType = loanInfo["applications"][i]["borrower"]["urla2020CitizenshipResidencyType"],
                dependentCount = loanInfo["applications"][i]["borrower"]["dependentCount"],
                dependentsAgesDescription = loanInfo["applications"][i]["borrower"]["dependentsAgesDescription"],
                emailAddressText = loanInfo["applications"][i]["borrower"]["emailAddressText"],
                firstName = loanInfo["applications"][i]["borrower"]["firstName"],
                firstNameWithMiddleName = loanInfo["applications"][i]["borrower"]["firstNameWithMiddleName"],
                middleName = borrowerMiddleName,
                applicationTakenMethodType = loanInfo["applications"][i]["borrower"]["applicationTakenMethodType"],
                hmdaGenderType = loanInfo["applications"][i]["borrower"]["hmdaGenderType"],
                hmdaGendertypeDoNotWishIndicator = loanInfo["applications"][i]["borrower"]["hmdaGendertypeDoNotWishIndicator"],
                hmdaOtherHispanicLatinoOrigin = loanInfo["applications"][i]["borrower"]["hmdaOtherHispanicLatinoOrigin"],
                hmdaEthnicityType = loanInfo["applications"][i]["borrower"]["hmdaEthnicityType"],
                hmdaMexicanIndicator = loanInfo["applications"][i]["borrower"]["hmdaMexicanIndicator"],
                hmdaPuertoRicanIndicator = loanInfo["applications"][i]["borrower"]["hmdaPuertoRicanIndicator"],
                hmdaCubanIndicator = loanInfo["applications"][i]["borrower"]["hmdaCubanIndicator"],
                hmdaHispanicLatinoOtherOriginIndicator = loanInfo["applications"][i]["borrower"]["hmdaHispanicLatinoOtherOriginIndicator"],
                hmdaEthnicityReportedField1 = loanInfo["applications"][i]["borrower"]["hmdaEthnicityReportedField1"],
                hmdaEthnicityReportedField2 = loanInfo["applications"][i]["borrower"]["hmdaEthnicityReportedField2"],
                hmdaEthnicityReportedField3 = loanInfo["applications"][i]["borrower"]["hmdaEthnicityReportedField3"],
                hmdaEthnicityReportedField4 = loanInfo["applications"][i]["borrower"]["hmdaEthnicityReportedField4"],
                hmdaEthnicityReportedField5 = loanInfo["applications"][i]["borrower"]["hmdaEthnicityReportedField5"],
                hmdaEthnicityReportedFields = loanInfo["applications"][i]["borrower"]["hmdaEthnicityReportedFields"],
                hmdaEthnicityDoNotWishIndicator = loanInfo["applications"][i]["borrower"]["hmdaEthnicityDoNotWishIndicator"],
                hmdaEthnicityHispanicLatinoIndicator = loanInfo["applications"][i]["borrower"]["hmdaEthnicityHispanicLatinoIndicator"],
                hmdaEthnicityNotHispanicLatinoIndicator = loanInfo["applications"][i]["borrower"]["hmdaEthnicityNotHispanicLatinoIndicator"],
                hmdaEthnicityNotApplicableIndicator = loanInfo["applications"][i]["borrower"]["hmdaEthnicityNotApplicableIndicator"],
                hmdaEthnicityInfoNotProvided = loanInfo["applications"][i]["borrower"]["hmdaEthnicityInfoNotProvided"],
                hmdaOtherPacificIslanderRace = loanInfo["applications"][i]["borrower"]["hmdaOtherPacificIslanderRace"],
                hmdaOtherAsianRace = loanInfo["applications"][i]["borrower"]["hmdaOtherAsianRace"],
                hmdaAfricanAmericanIndicator = loanInfo["applications"][i]["borrower"]["hmdaAfricanAmericanIndicator"],
                hmdaAmericanIndianIndicator = loanInfo["applications"][i]["borrower"]["hmdaAmericanIndianIndicator"],
                hmdaAsianIndicator = loanInfo["applications"][i]["borrower"]["hmdaAsianIndicator"],
                hmdaAsianIndianIndicator = loanInfo["applications"][i]["borrower"]["hmdaAsianIndianIndicator"],
                hmdaChineseIndicator = loanInfo["applications"][i]["borrower"]["hmdaChineseIndicator"],
                hmdaFilipinoIndicator = loanInfo["applications"][i]["borrower"]["hmdaFilipinoIndicator"],
                hmdaJapaneseIndicator = loanInfo["applications"][i]["borrower"]["hmdaJapaneseIndicator"],
                hmdaKoreanIndicator = loanInfo["applications"][i]["borrower"]["hmdaKoreanIndicator"],
                hmdaVietnameseIndicator = loanInfo["applications"][i]["borrower"]["hmdaVietnameseIndicator"],
                hmdaAsianOtherRaceIndicator = loanInfo["applications"][i]["borrower"]["hmdaAsianOtherRaceIndicator"],
                hmdaPacificIslanderIndicator = loanInfo["applications"][i]["borrower"]["hmdaPacificIslanderIndicator"],
                hmdaNativeHawaiianIndicator = loanInfo["applications"][i]["borrower"]["hmdaNativeHawaiianIndicator"],
                hmdaGuamanianOrChamorroIndicator = loanInfo["applications"][i]["borrower"]["hmdaGuamanianOrChamorroIndicator"],
                hmdaSamoanIndicator = loanInfo["applications"][i]["borrower"]["hmdaSamoanIndicator"],
                hmdaPacificIslanderOtherIndicator = loanInfo["applications"][i]["borrower"]["hmdaPacificIslanderOtherIndicator"],
                hmdaWhiteIndicator = loanInfo["applications"][i]["borrower"]["hmdaWhiteIndicator"],
                hmdaRaceDoNotWishProvideIndicator = loanInfo["applications"][i]["borrower"]["hmdaRaceDoNotWishProvideIndicator"],
                hmdaRaceReportedField1 = loanInfo["applications"][i]["borrower"]["hmdaRaceReportedField1"],
                hmdaRaceReportedField2 = loanInfo["applications"][i]["borrower"]["hmdaRaceReportedField2"],
                hmdaRaceReportedField3 = loanInfo["applications"][i]["borrower"]["hmdaRaceReportedField3"],
                hmdaRaceReportedField4 = loanInfo["applications"][i]["borrower"]["hmdaRaceReportedField4"],
                hmdaRaceReportedField5 = loanInfo["applications"][i]["borrower"]["hmdaRaceReportedField5"],
                hmdaRaceReportedFields = loanInfo["applications"][i]["borrower"]["hmdaRaceReportedFields"],
                homePhoneNumber = loanInfo["applications"][i]["borrower"]["homePhoneNumber"],
                lastName = loanInfo["applications"][i]["borrower"]["lastName"],
                lastNameWithSuffix = loanInfo["applications"][i]["borrower"]["lastNameWithSuffix"],
                suffixToName = borrowerSuffixName,
                maritalStatusType = loanInfo["applications"][i]["borrower"]["maritalStatusType"],
                mobilePhone = loanInfo["applications"][i]["borrower"]["mobilePhone"],
                taxIdentificationIdentifier = loanInfo["applications"][i]["borrower"]["taxIdentificationIdentifier"],
                workPhoneNumber = loanInfo["applications"][i]["borrower"]["workPhoneNumber"],
                jointAssetLiabilityReportingIndicator1 = loanInfo["applications"][i]["borrower"]["jointAssetLiabilityReportingIndicator1"],
                previousGrossMonthlyIncome = loanInfo["applications"][i]["borrower"]["previousGrossMonthlyIncome"],
                borrowerTypeInSummary = loanInfo["applications"][i]["borrower"]["borrowerTypeInSummary"],
                experianCreditScore = loanInfo["applications"][i]["borrower"]["experianCreditScore"],
                transUnionScore = loanInfo["applications"][i]["borrower"]["transUnionScore"],
                equifaxScore = loanInfo["applications"][i]["borrower"]["equifaxScore"],
                totalAssets = loanInfo["applications"][i]["borrower"]["totalAssets"],
                otherAssetsDoesNotApply = loanInfo["applications"][i]["borrower"]["otherAssetsDoesNotApply"],
                totalOtherAssets = loanInfo["applications"][i]["borrower"]["totalOtherAssets"],
                realEstateDoesNotApply = loanInfo["applications"][i]["borrower"]["realEstateDoesNotApply"],
                intentToOccupyIndicator = loanInfo["applications"][i]["borrower"]["intentToOccupyIndicator"],
                sectionAExplanation = loanInfo["applications"][i]["borrower"]["sectionAExplanation"],
                homeownerPastThreeYearsIndicator = loanInfo["applications"][i]["borrower"]["homeownerPastThreeYearsIndicator"],
                priorPropertyUsageType = loanInfo["applications"][i]["borrower"]["priorPropertyUsageType"],
                priorPropertyTitleType = loanInfo["applications"][i]["borrower"]["priorPropertyTitleType"],
                specialBorrowerSellerRelationshipIndicator = loanInfo["applications"][i]["borrower"]["specialBorrowerSellerRelationshipIndicator"],
                sectionBExplanation = loanInfo["applications"][i]["borrower"]["sectionBExplanation"],
                undisclosedBorrowedFundsIndicator = loanInfo["applications"][i]["borrower"]["undisclosedBorrowedFundsIndicator"],
                sectionCExplanation = loanInfo["applications"][i]["borrower"]["sectionCExplanation"],
                undisclosedBorrowedFundsAmount = loanInfo["applications"][i]["borrower"]["undisclosedBorrowedFundsAmount"],
                undisclosedMortgageApplicationIndicator = loanInfo["applications"][i]["borrower"]["undisclosedMortgageApplicationIndicator"],
                sectionDExplanation = loanInfo["applications"][i]["borrower"]["sectionDExplanation"],
                undisclosedCreditApplicationIndicator = loanInfo["applications"][i]["borrower"]["undisclosedCreditApplicationIndicator"],
                sectionD2Explanation = loanInfo["applications"][i]["borrower"]["sectionD2Explanation"],
                propertyProposedCleanEnergyLienIndicator = loanInfo["applications"][i]["borrower"]["propertyProposedCleanEnergyLienIndicator"],
                sectionEExplanation = loanInfo["applications"][i]["borrower"]["sectionEExplanation"],
                undisclosedComakerOfNoteIndicator = loanInfo["applications"][i]["borrower"]["undisclosedComakerOfNoteIndicator"],
                sectionFExplanation = loanInfo["applications"][i]["borrower"]["sectionFExplanation"],
                outstandingJudgementsIndicator = loanInfo["applications"][i]["borrower"]["outstandingJudgementsIndicator"],
                sectionGExplanation = loanInfo["applications"][i]["borrower"]["sectionGExplanation"],
                presentlyDelinquentIndicatorURLA = loanInfo["applications"][i]["borrower"]["presentlyDelinquentIndicatorURLA"],
                sectionHExplanation = loanInfo["applications"][i]["borrower"]["sectionHExplanation"],
                partyToLawsuitIndicatorURLA = loanInfo["applications"][i]["borrower"]["partyToLawsuitIndicatorURLA"],
                sectionIExplanation = loanInfo["applications"][i]["borrower"]["sectionIExplanation"],
                priorPropertyDeedInLieuConveyedIndicator = loanInfo["applications"][i]["borrower"]["priorPropertyDeedInLieuConveyedIndicator"],
                sectionJExplanation = loanInfo["applications"][i]["borrower"]["sectionJExplanation"],
                priorPropertyShortSaleCompletedIndicator = loanInfo["applications"][i]["borrower"]["priorPropertyShortSaleCompletedIndicator"],
                sectionKExplanation = loanInfo["applications"][i]["borrower"]["sectionKExplanation"],
                priorPropertyForeclosureCompletedIndicator = loanInfo["applications"][i]["borrower"]["priorPropertyForeclosureCompletedIndicator"],
                sectionLExplanation = loanInfo["applications"][i]["borrower"]["sectionLExplanation"],
                bankruptcyIndicator = loanInfo["applications"][i]["borrower"]["bankruptcyIndicator"],
                sectionMExplanation = loanInfo["applications"][i]["borrower"]["sectionMExplanation"],
                bankruptcyIndicatorChapterSeven = loanInfo["applications"][i]["borrower"]["bankruptcyIndicatorChapterSeven"],
                bankruptcyIndicatorChapterEleven = loanInfo["applications"][i]["borrower"]["bankruptcyIndicatorChapterEleven"],
                bankruptcyIndicatorChapterTwelve = loanInfo["applications"][i]["borrower"]["bankruptcyIndicatorChapterTwelve"],
                bankruptcyIndicatorChapterThirteen = loanInfo["applications"][i]["borrower"]["bankruptcyIndicatorChapterThirteen"]
            };
            return borrower;
        }

        private dynamic ExtractCoBorrowerInfoFromLoanResponse(JObject loanInfo, int i)
        {
            string coBorrowerMiddleName = string.Empty;
            string coBorrowerSuffixName = string.Empty;
            if (loanInfo["applications"][i]["coborrower"]["firstName"] != null && loanInfo["applications"][i]["coborrower"]["firstNameWithMiddleName"] != null)
            {
                string coBorrowerFirstAndMiddleName = loanInfo["applications"][i]["coborrower"]["firstNameWithMiddleName"].ToString();
                string coBorrowerFirstName = loanInfo["applications"][i]["coborrower"]["firstName"].ToString();
                coBorrowerMiddleName = coBorrowerFirstAndMiddleName.Replace(coBorrowerFirstName.Trim(), "").Trim();
            }
            if (loanInfo["applications"][i]["coborrower"]["lastName"] != null && loanInfo["applications"][i]["coborrower"]["lastNameWithSuffix"] != null)
            {
                string coBorrowerLastNameandSuffixName = loanInfo["applications"][i]["coborrower"]["lastNameWithSuffix"].ToString();
                string coBorrowerLastName = loanInfo["applications"][i]["coborrower"]["lastName"].ToString();
                coBorrowerSuffixName = coBorrowerLastNameandSuffixName.Replace(coBorrowerLastName.Trim(), "").Trim();
            }

            var coborrower = new
            {
                applicantType = loanInfo["applications"][i]["coborrower"]["applicantType"],
                birthDate = loanInfo["applications"][i]["coborrower"]["birthDate"], // != null ? loanInfo["applications"][i]["coborrower"]["birthDate"].ToObject<DateTime>().ToString("yyyy-MM-dd", cultureInfo) : null,
                borrowerType = loanInfo["applications"][i]["coborrower"]["borrowerType"],
                urla2020CitizenshipResidencyType = loanInfo["applications"][i]["coborrower"]["urla2020CitizenshipResidencyType"],
                dependentCount = loanInfo["applications"][i]["coborrower"]["dependentCount"],
                dependentsAgesDescription = loanInfo["applications"][i]["coborrower"]["dependentsAgesDescription"],
                emailAddressText = loanInfo["applications"][i]["coborrower"]["emailAddressText"],
                firstName = loanInfo["applications"][i]["coborrower"]["firstName"],
                firstNameWithMiddleName = loanInfo["applications"][i]["coborrower"]["firstNameWithMiddleName"],
                middleName = coBorrowerMiddleName,
                applicationTakenMethodType = loanInfo["applications"][i]["coborrower"]["applicationTakenMethodType"],
                hmdaGenderType = loanInfo["applications"][i]["coborrower"]["hmdaGenderType"],
                hmdaGendertypeDoNotWishIndicator = loanInfo["applications"][i]["coborrower"]["hmdaGendertypeDoNotWishIndicator"],
                hmdaOtherHispanicLatinoOrigin = loanInfo["applications"][i]["coborrower"]["hmdaOtherHispanicLatinoOrigin"],
                hmdaEthnicityType = loanInfo["applications"][i]["coborrower"]["hmdaEthnicityType"],
                hmdaMexicanIndicator = loanInfo["applications"][i]["coborrower"]["hmdaMexicanIndicator"],
                hmdaPuertoRicanIndicator = loanInfo["applications"][i]["coborrower"]["hmdaPuertoRicanIndicator"],
                hmdaCubanIndicator = loanInfo["applications"][i]["coborrower"]["hmdaCubanIndicator"],
                hmdaHispanicLatinoOtherOriginIndicator = loanInfo["applications"][i]["coborrower"]["hmdaHispanicLatinoOtherOriginIndicator"],
                hmdaEthnicityReportedField1 = loanInfo["applications"][i]["coborrower"]["hmdaEthnicityReportedField1"],
                hmdaEthnicityReportedField2 = loanInfo["applications"][i]["coborrower"]["hmdaEthnicityReportedField2"],
                hmdaEthnicityReportedField3 = loanInfo["applications"][i]["coborrower"]["hmdaEthnicityReportedField3"],
                hmdaEthnicityReportedField4 = loanInfo["applications"][i]["coborrower"]["hmdaEthnicityReportedField4"],
                hmdaEthnicityReportedField5 = loanInfo["applications"][i]["coborrower"]["hmdaEthnicityReportedField5"],
                hmdaEthnicityReportedFields = loanInfo["applications"][i]["coborrower"]["hmdaEthnicityReportedFields"],
                hmdaEthnicityDoNotWishIndicator = loanInfo["applications"][i]["coborrower"]["hmdaEthnicityDoNotWishIndicator"],
                hmdaEthnicityHispanicLatinoIndicator = loanInfo["applications"][i]["coborrower"]["hmdaEthnicityHispanicLatinoIndicator"],
                hmdaEthnicityNotHispanicLatinoIndicator = loanInfo["applications"][i]["coborrower"]["hmdaEthnicityNotHispanicLatinoIndicator"],
                hmdaEthnicityNotApplicableIndicator = loanInfo["applications"][i]["coborrower"]["hmdaEthnicityNotApplicableIndicator"],
                hmdaEthnicityInfoNotProvided = loanInfo["applications"][i]["coborrower"]["hmdaEthnicityInfoNotProvided"],
                hmdaOtherPacificIslanderRace = loanInfo["applications"][i]["coborrower"]["hmdaOtherPacificIslanderRace"],
                hmdaOtherAsianRace = loanInfo["applications"][i]["coborrower"]["hmdaOtherAsianRace"],
                hmdaAfricanAmericanIndicator = loanInfo["applications"][i]["coborrower"]["hmdaAfricanAmericanIndicator"],
                hmdaAmericanIndianIndicator = loanInfo["applications"][i]["coborrower"]["hmdaAmericanIndianIndicator"],
                hmdaAsianIndicator = loanInfo["applications"][i]["coborrower"]["hmdaAsianIndicator"],
                hmdaAsianIndianIndicator = loanInfo["applications"][i]["coborrower"]["hmdaAsianIndianIndicator"],
                hmdaChineseIndicator = loanInfo["applications"][i]["coborrower"]["hmdaChineseIndicator"],
                hmdaFilipinoIndicator = loanInfo["applications"][i]["coborrower"]["hmdaFilipinoIndicator"],
                hmdaJapaneseIndicator = loanInfo["applications"][i]["coborrower"]["hmdaJapaneseIndicator"],
                hmdaKoreanIndicator = loanInfo["applications"][i]["coborrower"]["hmdaKoreanIndicator"],
                hmdaVietnameseIndicator = loanInfo["applications"][i]["coborrower"]["hmdaVietnameseIndicator"],
                hmdaAsianOtherRaceIndicator = loanInfo["applications"][i]["coborrower"]["hmdaAsianOtherRaceIndicator"],
                hmdaPacificIslanderIndicator = loanInfo["applications"][i]["coborrower"]["hmdaPacificIslanderIndicator"],
                hmdaNativeHawaiianIndicator = loanInfo["applications"][i]["coborrower"]["hmdaNativeHawaiianIndicator"],
                hmdaGuamanianOrChamorroIndicator = loanInfo["applications"][i]["coborrower"]["hmdaGuamanianOrChamorroIndicator"],
                hmdaSamoanIndicator = loanInfo["applications"][i]["coborrower"]["hmdaSamoanIndicator"],
                hmdaPacificIslanderOtherIndicator = loanInfo["applications"][i]["coborrower"]["hmdaPacificIslanderOtherIndicator"],
                hmdaWhiteIndicator = loanInfo["applications"][i]["coborrower"]["hmdaWhiteIndicator"],
                hmdaRaceDoNotWishProvideIndicator = loanInfo["applications"][i]["coborrower"]["hmdaRaceDoNotWishProvideIndicator"],
                hmdaRaceReportedField1 = loanInfo["applications"][i]["coborrower"]["hmdaRaceReportedField1"],
                hmdaRaceReportedField2 = loanInfo["applications"][i]["coborrower"]["hmdaRaceReportedField2"],
                hmdaRaceReportedField3 = loanInfo["applications"][i]["coborrower"]["hmdaRaceReportedField3"],
                hmdaRaceReportedField4 = loanInfo["applications"][i]["coborrower"]["hmdaRaceReportedField4"],
                hmdaRaceReportedField5 = loanInfo["applications"][i]["coborrower"]["hmdaRaceReportedField5"],
                hmdaRaceReportedFields = loanInfo["applications"][i]["coborrower"]["hmdaRaceReportedFields"],
                homePhoneNumber = loanInfo["applications"][i]["coborrower"]["homePhoneNumber"],
                lastName = loanInfo["applications"][i]["coborrower"]["lastName"],
                lastNameWithSuffix = loanInfo["applications"][i]["coborrower"]["lastNameWithSuffix"],
                suffixToName = coBorrowerSuffixName,
                maritalStatusType = loanInfo["applications"][i]["coborrower"]["maritalStatusType"],
                mobilePhone = loanInfo["applications"][i]["coborrower"]["mobilePhone"],
                taxIdentificationIdentifier = loanInfo["applications"][i]["coborrower"]["taxIdentificationIdentifier"],
                workPhoneNumber = loanInfo["applications"][i]["coborrower"]["workPhoneNumber"],
                previousGrossMonthlyIncome = loanInfo["applications"][i]["coborrower"]["previousGrossMonthlyIncome"],
                borrowerTypeInSummary = loanInfo["applications"][i]["coborrower"]["borrowerTypeInSummary"],
                experianCreditScore = loanInfo["applications"][i]["coborrower"]["experianCreditScore"],
                transUnionScore = loanInfo["applications"][i]["coborrower"]["transUnionScore"],
                equifaxScore = loanInfo["applications"][i]["coborrower"]["equifaxScore"],
                totalAssets = loanInfo["applications"][i]["coborrower"]["totalAssets"],
                otherAssetsDoesNotApply = loanInfo["applications"][i]["coborrower"]["otherAssetsDoesNotApply"],
                totalOtherAssets = loanInfo["applications"][i]["coborrower"]["totalOtherAssets"],
                realEstateDoesNotApply = loanInfo["applications"][i]["coborrower"]["realEstateDoesNotApply"],
                intentToOccupyIndicator = loanInfo["applications"][i]["coborrower"]["intentToOccupyIndicator"],
                homeownerPastThreeYearsIndicator = loanInfo["applications"][i]["coborrower"]["homeownerPastThreeYearsIndicator"],
                priorPropertyUsageType = loanInfo["applications"][i]["coborrower"]["priorPropertyUsageType"],
                priorPropertyTitleType = loanInfo["applications"][i]["coborrower"]["priorPropertyTitleType"],
                specialBorrowerSellerRelationshipIndicator = loanInfo["applications"][i]["coborrower"]["specialBorrowerSellerRelationshipIndicator"],
                undisclosedBorrowedFundsIndicator = loanInfo["applications"][i]["coborrower"]["undisclosedBorrowedFundsIndicator"],
                undisclosedBorrowedFundsAmount = loanInfo["applications"][i]["coborrower"]["undisclosedBorrowedFundsAmount"],
                undisclosedMortgageApplicationIndicator = loanInfo["applications"][i]["coborrower"]["undisclosedMortgageApplicationIndicator"],
                undisclosedCreditApplicationIndicator = loanInfo["applications"][i]["coborrower"]["undisclosedCreditApplicationIndicator"],
                propertyProposedCleanEnergyLienIndicator = loanInfo["applications"][i]["coborrower"]["propertyProposedCleanEnergyLienIndicator"],
                undisclosedComakerOfNoteIndicator = loanInfo["applications"][i]["coborrower"]["undisclosedComakerOfNoteIndicator"],
                outstandingJudgementsIndicator = loanInfo["applications"][i]["coborrower"]["outstandingJudgementsIndicator"],
                presentlyDelinquentIndicatorURLA = loanInfo["applications"][i]["coborrower"]["presentlyDelinquentIndicatorURLA"],
                partyToLawsuitIndicatorURLA = loanInfo["applications"][i]["coborrower"]["partyToLawsuitIndicatorURLA"],
                priorPropertyDeedInLieuConveyedIndicator = loanInfo["applications"][i]["coborrower"]["priorPropertyDeedInLieuConveyedIndicator"],
                priorPropertyShortSaleCompletedIndicator = loanInfo["applications"][i]["coborrower"]["priorPropertyShortSaleCompletedIndicator"],
                priorPropertyForeclosureCompletedIndicator = loanInfo["applications"][i]["coborrower"]["priorPropertyForeclosureCompletedIndicator"],
                bankruptcyIndicator = loanInfo["applications"][i]["coborrower"]["bankruptcyIndicator"],
                bankruptcyIndicatorChapterSeven = loanInfo["applications"][i]["coborrower"]["bankruptcyIndicatorChapterSeven"],
                bankruptcyIndicatorChapterEleven = loanInfo["applications"][i]["coborrower"]["bankruptcyIndicatorChapterEleven"],
                bankruptcyIndicatorChapterTwelve = loanInfo["applications"][i]["coborrower"]["bankruptcyIndicatorChapterTwelve"],
                bankruptcyIndicatorChapterThirteen = loanInfo["applications"][i]["coborrower"]["bankruptcyIndicatorChapterThirteen"]
            };
            return coborrower;
        }

        private dynamic ExtractEmploymentInfoFromLoanResponse(JObject loanInfo, int i)
        {
            var employment = new List<Object>();
            string addressStreetLine = string.Empty;
            foreach (var item in loanInfo["applications"][i]["employment"])
            {
                string address1 = item["addressStreetLine1"] != null ? item["addressStreetLine1"].ToString() : null;
                string unit = item["addressUnitIdentifier"] != null ? item["addressUnitIdentifier"].ToString() : null;
                if (unit != null && address1 != null)
                {
                    char[] trimChars = unit.ToCharArray();
                    addressStreetLine = address1.TrimEnd(trimChars);
                }
                else
                {
                    addressStreetLine = address1;
                }
                if (item["owner"] != null && item["owner"].ToString().ToLower() == "coborrower" && loanInfo["applications"][i]["coborrower"]["firstName"] == null && loanInfo["applications"][i]["coborrower"]["lastName"] == null)
                {
                    dynamic emp = null;
                }
                else
                {
                    var emp = new
                    {
                        id = item["id"],
                        addressCity = item["addressCity"],
                        addressPostalCode = item["addressPostalCode"],
                        addressState = item["addressState"],
                        addressStreetLine1 = addressStreetLine,
                        basePayAmount = item["basePayAmount"],
                        bonusAmount = item["bonusAmount"],
                        commissionsAmount = item["commissionsAmount"],
                        currentEmploymentIndicator = item["currentEmploymentIndicator"],
                        employerName = item["employerName"],
                        monthlyIncomeAmount = item["monthlyIncomeAmount"],
                        otherAmount = item["otherAmount"],
                        overtimeAmount = item["overtimeAmount"],
                        owner = ((item["owner"] != null && item["owner"].ToString().Trim() == "") || item["owner"] == null) ? "Borrower" : item["owner"],
                        phoneNumber = item["phoneNumber"],
                        positionDescription = item["positionDescription"],
                        selfEmployedIndicator = item["selfEmployedIndicator"],
                        timeInLineOfWorkYears = item["timeInLineOfWorkYears"],
                        employmentStartDate = item["employmentStartDate"], // != null ? item["employmentStartDate"].ToObject<DateTime>().ToString("MM-dd-yyyy") : null,
                        startDate = item["startDate"], // != null ? item["startDate"].ToObject<DateTime>().ToString("MM-dd-yyyy") : null,
                        endDate = item["endDate"], // != null ? item["endDate"].ToObject<DateTime>().ToString("MM-dd-yyyy") : null,
                        jobTermMonths = item["jobTermMonths"]
                    };
                    employment.Add(emp);
                }
            }
            return employment;
        }

        private dynamic ExtractIncomeInfoFromLoanResponse(JObject loanInfo, int i)
        {
            var income = new List<Object>();
            foreach (var item in loanInfo["applications"][i]["income"])
            {
                if (item["incomeType"].ToString() != "OtherIncome")
                {
                    if (item["owner"] != null && item["owner"].ToString().ToLower() == "coborrower" && loanInfo["applications"][i]["coborrower"]["firstName"] == null && loanInfo["applications"][i]["coborrower"]["lastName"] == null)
                    {
                        dynamic incomeAmount = null;
                    }
                    else
                    {
                        if (item["id"].ToString() != "Borrower_DividendsInterest" && item["id"].ToString() != "Borrower_NetRentalIncome" && item["id"].ToString() != "CoBorrower_DividendsInterest" && item["id"].ToString() != "CoBorrower_NetRentalIncome")
                        {
                            var incomeAmount = new
                            {
                                id = item["id"],
                                incomeType = item["incomeType"],
                                owner = ((item["owner"] != null && item["owner"].ToString().Trim() == "") || item["owner"] == null) ? "Borrower" : item["owner"],
                                amount = item["amount"],
                                currentIndicator = item["currentIndicator"]
                            };
                            income.Add(incomeAmount);
                        }
                    }
                }
            }
            return income;
        }

        private dynamic ExtractResidencesInfoFromLoanResponse(JObject loanInfo, int i)
        {
            var residences = new List<Object>();
            string addressStreetLine = string.Empty;
            foreach (var item in loanInfo["applications"][i]["residences"])
            {
                string address1 = item["addressStreetLine1"] != null ? item["addressStreetLine1"].ToString() : null;
                string unit = item["addressUnitIdentifier"] != null ? item["addressUnitIdentifier"].ToString() : null;
                if (unit != null && address1 != null)
                {
                    char[] trimChars = unit.ToCharArray();
                    addressStreetLine = address1.TrimEnd(trimChars);
                }
                else
                {
                    addressStreetLine = address1;
                }
                if (item["applicantType"] != null && item["applicantType"].ToString().ToLower() == "coborrower" && loanInfo["applications"][i]["coborrower"]["firstName"] == null && loanInfo["applications"][i]["coborrower"]["lastName"] == null)
                {
                    dynamic residence = null;
                }
                else
                {
                    var residence = new
                    {
                        id = item["id"],
                        addressCity = item["addressCity"],
                        addressPostalCode = item["addressPostalCode"],
                        addressState = item["addressState"],
                        addressStreetLine1 = addressStreetLine,
                        applicantType = ((item["applicantType"] != null && item["applicantType"].ToString().Trim() == "") || item["applicantType"] == null) ? "Borrower" : item["applicantType"],
                        durationTermMonths = item["durationTermMonths"],
                        durationTermYears = item["durationTermYears"],
                        mailingAddressIndicator = item["mailingAddressIndicator"] != null ? item["mailingAddressIndicator"] : false,
                        residencyBasisType = item["residencyBasisType"],
                        residencyType = item["residencyType"],
                        addressUnitIdentifier = item["addressUnitIdentifier"],
                        countryCode = item["countryCode"],
                    };
                    residences.Add(residence);
                }
            }
            return residences;
        }

        private dynamic ExtractReoPropertiesInfoFromLoanResponse(JObject loanInfo, int i)
        {
            var reoProperties = JsonConvert.DeserializeObject<dynamic>(loanInfo["applications"][i]["reoProperties"].ToString());
            foreach (var reoPropertiesItem in reoProperties)
            {
                if (reoPropertiesItem["id"] != null)
                {
                    reoPropertiesItem.Property("id").Remove();
                }
                if (reoPropertiesItem["owner"] == null || (reoPropertiesItem["owner"] != null && reoPropertiesItem["owner"].ToString() == ""))
                {
                    reoPropertiesItem["owner"] = "Borrower";
                }
            }
            return reoProperties;
        }

        private dynamic ExtractContactsInfoFromLoanResponse(JObject loanInfo)
        {
            int i = 0;
            var contacts = new List<Object>();
            foreach (var item in loanInfo["contacts"])
            {
                if (item["contactType"].ToString() == "BROKER_LENDER")
                {
                    var contact = new
                    {
                        contactType = item["contactType"],
                        address = item["address"],
                        city = item["city"],
                        companyId = item["companyId"],
                        name = item["name"],
                        phone = item["phone"],
                        postalCode = item["postalCode"],
                        state = item["state"],
                    };
                    contacts.Add(contact);
                }
                i++;
            }
            return contacts;
        }

        private dynamic ExtractPropertyInfoFromLoanResponse(JObject loanInfo)
        {
            string stAddress = string.Empty;
            string address1 = loanInfo["property"]["streetAddress"] != null ? loanInfo["property"]["streetAddress"].ToString() : null;
            string unit = loanInfo["property"]["addressUnitIdentifier"] != null ? loanInfo["property"]["addressUnitIdentifier"].ToString() : null;
            if (unit != null && address1 != null)
            {
                char[] trimChars = unit.ToCharArray();
                stAddress = address1.TrimEnd(trimChars);
            }
            else
            {
                stAddress = address1;
            }
            var property = new
            {
                city = loanInfo["property"]["city"],
                county = loanInfo["property"]["county"],
                financedNumberOfUnits = loanInfo["property"]["financedNumberOfUnits"],
                loanPurposeType = loanInfo["property"]["loanPurposeType"],
                postalCode = loanInfo["property"]["postalCode"],
                propertyUsageType = loanInfo["property"]["propertyUsageType"],
                occupancyDisplayField = loanInfo["property"]["occupancyDisplayField"],
                state = loanInfo["property"]["state"],
                loanPurposeTypeURLA = loanInfo["property"]["loanPurposeTypeURLA"],
                streetAddress = stAddress,
                addressUnitIdentifier = loanInfo["property"]["addressUnitIdentifier"],
                propertyMixedUsageIndicator = loanInfo["property"]["propertyMixedUsageIndicator"],
            };
            return property;
        }
        private Object GetLoanDataAndSendMessageToQueue(JObject result, string loEmail, bool createFlow)
        {
            if (createFlow)
            {
                // Send loan to Queue to create Flows in Floify.
                PushLoanToFloifyFlowsQueue(result, loEmail);
            }

            return new
            {
                loanGuid = result["encompassId"],
                loanNumber = result["loanNumber"],
            };
        }
        public async Task<Object> CreateLoan(string loanTemplate, string loId, string loaId, object request, string userId, string loanProgramPath, string closingCostPath, bool createFlow)
        {
            Object response = new Object();
            string apiUrl;
            //Object requestObj = ExtractAssignDecryptLoanInfo((JObject)request);
            JObject loanCreateRequest = (JObject)request;
            try
            {
                if (loanCreateRequest.ContainsKey("contacts"))
                {
                    loanCreateRequest.Remove("contacts");
                }
                //if (requestObj != null)
                //{
                if (string.IsNullOrEmpty(loId) || loId == "undefined" || loId == null)
                {
                    apiUrl = string.Format(encompassAPIs.CreateLoanInFolder, loanTemplate);
                }
                else
                {
                    apiUrl = string.Format(encompassAPIs.CreateLoanInFolderAssignLO, loanTemplate, loId);
                }
                ApiResponse<Object> apiResponse = await httpService.PostAsync<Object>(apiUrl, loanCreateRequest).ConfigureAwait(false);
                if (apiResponse.ErrorResponse != null && apiResponse.ErrorResponse.Error != null)
                {
                    return StatusCode(apiResponse.ErrorResponse.Error.Code, apiResponse.ErrorResponse);
                }
                else if (apiResponse.Success)
                {
                    var loanGuid = ((JObject)apiResponse.Data)["encompassId"].ToString();
                    if (!(string.IsNullOrEmpty(loaId) || loaId == "undefined" || loaId == null))
                    {
                        string apiURLMSFRoles = string.Format(encompassAPIs.RetrieveAllMilestoneFreeRoles, loanGuid);
                        ApiResponse<Object> apiResponseMSFRoles = await httpService.GetAsync<Object>(apiURLMSFRoles).ConfigureAwait(false);
                        if (apiResponseMSFRoles.Success)
                        {
                            JArray resultMSFRoles = (JArray)apiResponseMSFRoles.Data;
                            string MSFLogId = string.Empty;
                            for (int i = 0; i < resultMSFRoles.Count; i++)
                            {
                                JObject MSFRole = (JObject)resultMSFRoles[i];
                                if (MSFRole["loanAssociate"]["roleName"].ToString() == "Loan Officer Asst")
                                {
                                    MSFLogId = MSFRole["id"].ToString();
                                    break;
                                }
                            }
                            if (!string.IsNullOrEmpty(MSFLogId))
                            {
                                string apiAssignAssociateLOAURL = string.Format(encompassAPIs.AssignAssociate, loanGuid, MSFLogId);
                                var requestAssignAssociateLOARequest = new
                                {
                                    loanAssociateType = "User",
                                    id = loaId
                                };
                                ApiResponse<Object> apiAssignAssociateLOAResponse = await httpService.PutAsync<Object>(apiAssignAssociateLOAURL, requestAssignAssociateLOARequest).ConfigureAwait(false);
                            }
                        }
                    }
                    if (!(string.IsNullOrEmpty(loanProgramPath) || string.IsNullOrEmpty(closingCostPath)))
                    {
                        string applyTemplateUrl = String.Format(encompassAPIs.ApplyTemplate);
                        var templateUpdateRequest = new
                        {
                            loanGuid,
                            userId,
                            loanProgramPath,
                            closingCostPath,
                            loanConfiguration.URLAVersion
                        };
                        ApiResponse<object> apiApplyLoanProgramResponse = await httpService.PostAsync<object>(applyTemplateUrl, templateUpdateRequest, false, true).ConfigureAwait(false);
                        if (apiApplyLoanProgramResponse != null && !apiApplyLoanProgramResponse.Success && apiApplyLoanProgramResponse.ErrorResponse != null && apiApplyLoanProgramResponse.ErrorResponse.Error != null)
                        {
                            // response = GetLoanGuidAndNumberFromResponse((JObject)apiResponse.Data);
                            logger.LogError(apiApplyLoanProgramResponse.ErrorResponse.Error.Message, templateUpdateRequest);
                        }
                    }
                    //JObject loanCreateRequest = loanReqData;
                    string loEmail = string.Empty;
                    if (loanCreateRequest != null && loanCreateRequest.ContainsKey("loEmail") && loanCreateRequest["loEmail"] != null)
                    {
                        loEmail = loanCreateRequest["loEmail"].ToString();
                    }
                    response = GetLoanDataAndSendMessageToQueue((JObject)apiResponse.Data, loEmail, createFlow);
                }
                logger.LogError("request Payload: " + loanCreateRequest + System.Environment.NewLine + "response from encompass: " + (JObject)apiResponse.Data, "request Payload: " + loanCreateRequest + System.Environment.NewLine + "response from encompass: " + (JObject)apiResponse.Data);
                saveNLogger.SaveLogFile("LoanService", "CreateLoan", "request Payload: " + loanCreateRequest + System.Environment.NewLine + "response from encompass: " + (JObject)apiResponse.Data, "request Payload: " + loanCreateRequest + System.Environment.NewLine + "response from encompass: " + (JObject)apiResponse.Data);
                //}
                //else
                //{
                //    saveNLogger.SaveLogFile("LoanService", "CreateLoan", "Error occured at line no 634.Request Payload is Empty", "Request Payload is Empty");
                //    ApiResponse<object> errresponse = new ApiResponse<object>
                //    {
                //        Success = false
                //    };
                //    errresponse.ErrorResponse = new ErrorResponse
                //    {
                //        Error = new Error
                //        {
                //            Code = 400,
                //            Message = "Bad Request"
                //        }
                //    };
                //    return StatusCode(errresponse.ErrorResponse.Error.Code, errresponse.ErrorResponse);
                //}
            }
            catch (Exception ex)
            {
                saveNLogger.SaveLogFile("LoanService", "CreateLoan", ex.StackTrace, ex.Message);
            }

            return response;
        }
        public async Task<Object> UpdateLoan(string loanGuid, Object request)
        {
            Object response = new Object();
            try
            {
                string apiURL = string.Format(encompassAPIs.UpdateLoan, loanGuid);
                ApiResponse<Object> apiResponse = await httpService.PatchAsync<Object>(apiURL, request).ConfigureAwait(false);
                AddCustomPropertiesInAppInsights(JsonConvert.SerializeObject(request), JsonConvert.SerializeObject(apiResponse.Data), apiURL, loanGuid, "UpdateLoan", "Update Loan in Encompass");
                if (apiResponse.Success)
                {
                    response = apiResponse.Data;
                }
                else if (apiResponse.ErrorResponse != null && apiResponse.ErrorResponse.Error != null)
                {
                    logger.LogError(apiResponse.ErrorResponse.Error.Message);
                    return StatusCode(apiResponse.ErrorResponse.Error.Code, apiResponse.ErrorResponse);
                }
            }
            catch (Exception ex)
            {
                saveNLogger.SaveLogFile("LoanService", "UpdateLoan", ex.StackTrace, ex.Message);
            }
            return response;
        }

        public async Task PushLoanToFloifyFlowsQueue(JObject loan, string loEmail)
        {
            List<Object> applications = new List<Object>();
            try
            {
                foreach (var borrowerPair in loan["applications"])
                {
                    var borrowerPairInfo = new
                    {
                        id = borrowerPair["id"],
                        applicationId = borrowerPair["applicationId"],
                        borrower = borrowerPair["borrower"],
                        coBorrower = borrowerPair["coborrower"],
                    };
                    applications.Add(borrowerPairInfo);
                }

                var request = new
                {
                    loanGuid = loan["encompassId"],
                    loanNumber = loan["loanNumber"],
                    loEmail,
                    applications = loan["applications"]
                };

                // Instantiate a QueueClient which will be used to create and manipulate the queue
                QueueClient floifyFlowsQueueClient = new QueueClient(configuration["bpabgprocessor"], azureQueue.Queue);

                // Create the queue
                floifyFlowsQueueClient.CreateIfNotExists();

                if (floifyFlowsQueueClient.Exists())
                {
                    // Send a message to the queue
                    await floifyFlowsQueueClient.SendMessageAsync(JsonConvert.SerializeObject(request)).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                saveNLogger.SaveLogFile("LoanService", "PushLoanToFloifyFlowsQueue", ex.StackTrace, ex.Message);
            }
        }

        private dynamic ExtractAssignDecryptLoanInfo(JObject loanInfo)
        {
            try
            {
                for (int i = 0; i < loanInfo["borrowerPairCount"].ToObject<int>(); i++)
                {
                    loanInfo["applications"][i]["borrower"]["firstName"] = AsymmetricRSAAlgorithm.Decrypt(loanInfo["applications"][i]["borrower"]["firstName"]);
                    loanInfo["applications"][i]["borrower"]["firstNameWithMiddleName"] = AsymmetricRSAAlgorithm.Decrypt(loanInfo["applications"][i]["borrower"]["firstNameWithMiddleName"]);
                    loanInfo["applications"][i]["borrower"]["middleName"] = AsymmetricRSAAlgorithm.Decrypt(loanInfo["applications"][i]["borrower"]["middleName"]);
                    loanInfo["applications"][i]["borrower"]["lastName"] = AsymmetricRSAAlgorithm.Decrypt(loanInfo["applications"][i]["borrower"]["lastName"]);
                    loanInfo["applications"][i]["borrower"]["lastNameWithSuffix"] = AsymmetricRSAAlgorithm.Decrypt(loanInfo["applications"][i]["borrower"]["lastNameWithSuffix"]);
                    loanInfo["applications"][i]["borrower"]["taxIdentificationIdentifier"] = AsymmetricRSAAlgorithm.Decrypt(loanInfo["applications"][i]["borrower"]["taxIdentificationIdentifier"]);
                    loanInfo["applications"][i]["borrower"]["birthDate"] = AsymmetricRSAAlgorithm.Decrypt(loanInfo["applications"][i]["borrower"]["birthDate"]);
                    loanInfo["applications"][i]["borrower"]["homePhoneNumber"] = AsymmetricRSAAlgorithm.Decrypt(loanInfo["applications"][i]["borrower"]["homePhoneNumber"]);
                    loanInfo["applications"][i]["borrower"]["mobilePhone"] = AsymmetricRSAAlgorithm.Decrypt(loanInfo["applications"][i]["borrower"]["mobilePhone"]);
                    loanInfo["applications"][i]["borrower"]["emailAddressText"] = AsymmetricRSAAlgorithm.Decrypt(loanInfo["applications"][i]["borrower"]["emailAddressText"]);

                    loanInfo["applications"][i]["coborrower"]["firstName"] = AsymmetricRSAAlgorithm.Decrypt(loanInfo["applications"][i]["coborrower"]["firstName"]);
                    loanInfo["applications"][i]["coborrower"]["firstNameWithMiddleName"] = AsymmetricRSAAlgorithm.Decrypt(loanInfo["applications"][i]["coborrower"]["firstNameWithMiddleName"]);
                    loanInfo["applications"][i]["coborrower"]["middleName"] = AsymmetricRSAAlgorithm.Decrypt(loanInfo["applications"][i]["coborrower"]["middleName"]);
                    loanInfo["applications"][i]["coborrower"]["lastName"] = AsymmetricRSAAlgorithm.Decrypt(loanInfo["applications"][i]["coborrower"]["lastName"]);
                    loanInfo["applications"][i]["coborrower"]["lastNameWithSuffix"] = AsymmetricRSAAlgorithm.Decrypt(loanInfo["applications"][i]["coborrower"]["lastNameWithSuffix"]);
                    loanInfo["applications"][i]["coborrower"]["taxIdentificationIdentifier"] = AsymmetricRSAAlgorithm.Decrypt(loanInfo["applications"][i]["coborrower"]["taxIdentificationIdentifier"]);
                    loanInfo["applications"][i]["coborrower"]["birthDate"] = AsymmetricRSAAlgorithm.Decrypt(loanInfo["applications"][i]["coborrower"]["birthDate"]);
                    loanInfo["applications"][i]["coborrower"]["homePhoneNumber"] = AsymmetricRSAAlgorithm.Decrypt(loanInfo["applications"][i]["coborrower"]["homePhoneNumber"]);
                    loanInfo["applications"][i]["coborrower"]["mobilePhone"] = AsymmetricRSAAlgorithm.Decrypt(loanInfo["applications"][i]["coborrower"]["mobilePhone"]);
                    loanInfo["applications"][i]["coborrower"]["emailAddressText"] = AsymmetricRSAAlgorithm.Decrypt(loanInfo["applications"][i]["coborrower"]["emailAddressText"]);

                    for (int j = 0; j < ((JContainer)loanInfo["applications"][i]["residences"]).Count; j++)
                    {
                        loanInfo["applications"][i]["residences"][j]["addressPostalCode"] = AsymmetricRSAAlgorithm.Decrypt(loanInfo["applications"][i]["residences"][j]["addressPostalCode"]);
                        loanInfo["applications"][i]["residences"][j]["addressStreetLine1"] = AsymmetricRSAAlgorithm.Decrypt(loanInfo["applications"][i]["residences"][j]["addressStreetLine1"]);
                    }
                }
                return (Object)loanInfo;
            }
            catch (Exception ex)
            {
                saveNLogger.SaveLogFile("LoanService", "ExtractAssignDecryptLoanInfo", ex.StackTrace, ex.Message);
                return null;
            }
        }

        public async Task<Object> GetLoanFieldValues(string loanGuid, string fieldIds)
        {
            if (string.IsNullOrEmpty(loanGuid) || (!string.IsNullOrEmpty(loanGuid) && !Guid.TryParse(loanGuid, out _)))
            {
                return BadRequest(GetErrorResponse(400, "Invalid Loan Guid"));
            }

            Object response = new Object();
            var requestObject = new Object();

            try
            {
                if (!string.IsNullOrEmpty(fieldIds))
                {
                    requestObject = fieldIds.Trim().Split(",");
                }


                string apiURL = string.Format(encompassAPIs.FieldReader, loanGuid);
                ApiResponse<Object> apiResponse = await httpService.PostAsync<Object>(apiURL, requestObject).ConfigureAwait(false);

                if (apiResponse.Success)
                {
                    response = apiResponse.Data;
                }
                else if (apiResponse.ErrorResponse != null && apiResponse.ErrorResponse.Error != null)
                {
                    return StatusCode(apiResponse.ErrorResponse.Error.Code, apiResponse.ErrorResponse);
                }
            }
            catch (Exception ex)
            {
                saveNLogger.SaveLogFile("LoanService", "GetLoanFieldValues", ex.StackTrace, ex.Message);
                throw new Exception("Internal Server Error");
            }

            return response;
        }

        private Object GetErrorResponse(int statusCode, string message)
        {
            return new ErrorResponse
            {
                Error = new Error()
                {
                    Code = statusCode,
                    Message = message
                }
            };
        }

        public async Task<Object> SubmitRateLock(Guid loanGuid, RateLockRequest rateLockRequest, DTO.Action action)
        {
            var response = new Object();
            try
            {
                string apiURL = string.Format(encompassAPIs.GetRateLockRequests, loanGuid);
                ApiResponse<Object> apiResponse = await httpService.GetAsync<Object>(apiURL).ConfigureAwait(false);
                if (apiResponse.Success && apiResponse.Data != null)
                {
                    var rateLockResponse = apiResponse.Data as JArray;
                    if (action == DTO.Action.Extend && rateLockResponse != null && rateLockResponse.Count > 0)
                    {
                        JObject recentRateLockRequest = null;
                        foreach (JObject rateLock in rateLockResponse)
                        {
                            if ((string.Equals(rateLock["requestType"].ToString(), "Extension") || string.Equals(rateLock["requestType"].ToString(), "Lock")) && string.Equals(rateLock["lockStatus"].ToString(), "Locked"))
                            {
                                recentRateLockRequest = rateLock;
                                break;
                            }
                        }
                        if (recentRateLockRequest == null)
                            return StatusCode(404, GetErrorResponse(404, errorMessages.RateLockError));
                        apiURL = string.Format(encompassAPIs.SubmitRateLockRequest, loanGuid, action, recentRateLockRequest["id"]);
                        apiResponse = await httpService.PostAsync<Object>(apiURL, new { rateLockRequest.LockRequest }).ConfigureAwait(false);
                        if (apiResponse.Success && apiResponse.Data != null)
                        {
                            response = apiResponse.Data;
                            object customFields = rateLockRequest.CustomFieldUpdateRequest;
                            await UpdateLoan(loanGuid.ToString(), new { customFields });
                        }
                        else if (apiResponse.ErrorResponse != null && apiResponse.ErrorResponse.Error != null)
                        {
                            logger.LogError(apiResponse.ErrorResponse.Error.Message);
                            return StatusCode(apiResponse.ErrorResponse.Error.Code, apiResponse.ErrorResponse);
                        }
                    }
                    else if (rateLockResponse.Count == 0)
                        return StatusCode(404, GetErrorResponse(404, errorMessages.RateLockError));
                }
                else if (apiResponse.ErrorResponse != null && apiResponse.ErrorResponse.Error != null)
                {
                    logger.LogError(apiResponse.ErrorResponse.Error.Message);
                    return StatusCode(apiResponse.ErrorResponse.Error.Code, apiResponse.ErrorResponse);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.StackTrace, ex.Message);
                return StatusCode(500, ErrorHandling.GetErrorResponse(500, ex.Message));
            }
            return response;
        }

        public async Task<Object> GetLoanQualifer(string loanGuid, JObject request)
        {
            var response = new Object();
            JObject loanQualifierRequestPayload = null;
            try
            {
                string apiUrl = string.Format(encompassAPIs.RetrieveLoanInfoForQualifier, loanGuid);
                ApiResponse<Object> apiResponse = await httpService.GetAsync<Object>(apiUrl).ConfigureAwait(false);
                AddCustomPropertiesInAppInsights(null, JsonConvert.SerializeObject(apiResponse.Data), apiUrl, loanGuid, "GetLoanQualifer", "RetrieveLoanInfoForQualifier");
                if (apiResponse.Success && apiResponse.Data != null)
                {
                    LoanQualifierRequest loanQualifierRequest = GetLoanQualifierRequest((JObject)apiResponse.Data, request);
                    if (loanQualifierRequest != null)
                    {
                        apiUrl = string.Format(encompassAPIs.RetrieveLoanQualifier);
                        if (loanQualifierRequest.LoanQualifierInfo.ProductOptions.Count == 0)
                        {
                            loanQualifierRequestPayload = JsonConvert.DeserializeObject<dynamic>(JsonConvert.SerializeObject(loanQualifierRequest.LoanQualifierInfo));
                            loanQualifierRequestPayload.Property("ProductOptions").Remove();
                        }
                        else
                        {
                            loanQualifierRequestPayload = JsonConvert.DeserializeObject<dynamic>(JsonConvert.SerializeObject(loanQualifierRequest.LoanQualifierInfo));
                        }
                        ApiResponse<Object> loanQualifierApiResponse = await httpService.PostAsync<Object>(apiUrl, loanQualifierRequestPayload).ConfigureAwait(false);
                        AddCustomPropertiesInAppInsights(JsonConvert.SerializeObject(loanQualifierRequestPayload), JsonConvert.SerializeObject(loanQualifierApiResponse), apiUrl, loanGuid, "GetLoanQualifer", "RetrieveLoanQualifier");
                        if (loanQualifierApiResponse.Success && loanQualifierApiResponse.Data != null)
                        {
                            if (string.IsNullOrEmpty(loanQualifierRequest.LoanQualifierInfo.LoanInformation.LoanId) && (JContainer)loanQualifierApiResponse.Data != null && ((JContainer)loanQualifierApiResponse.Data)["loanId"] != null)
                            {
                                List<CustomField> customFields = new List<CustomField>();
                                CustomField customField = new CustomField()
                                {
                                    FieldName = "CX.EPPS.LOAN.ID",
                                    StringValue = ((JContainer)loanQualifierApiResponse.Data)["loanId"].ToString()
                                };
                                customFields.Add(customField);

                                await UpdateLoan(loanGuid, new { customFields });
                            }

                            response = new
                            {
                                loanQualifierApiResponse.Data,
                                loanQualifierRequest.CurrentRateLockDetails
                            };
                            AddCustomPropertiesInAppInsights(null, string.Format("{0}{1}", "Compination of the EPPS response and Current Loan info", JsonConvert.SerializeObject(response)), null, loanGuid, "GetLoanQualifer", "Compination of the EPPS response and Current Loan info");
                        }
                        else if (loanQualifierApiResponse.ErrorResponse != null && loanQualifierApiResponse.ErrorResponse.Error != null)
                        {
                            loanQualifierApiResponse.ErrorResponse.Error.Message = string.Format("Error occurred at GetLoanQualifer with EPPS UserName= {0}; Error Message= {1}", loanQualifierRequest.LoanQualifierInfo.EppsUserName, loanQualifierApiResponse.ErrorResponse.Error.Message);
                            logger.LogError(string.Format("Error occurred at GetLoanQualifer with EPPS UserName= {0}; Error Response= {1}", loanQualifierRequest.LoanQualifierInfo.EppsUserName, JsonConvert.SerializeObject(loanQualifierApiResponse.ErrorResponse)));
                            return StatusCode(loanQualifierApiResponse.ErrorResponse.Error.Code, loanQualifierApiResponse.ErrorResponse);
                        }
                    }
                }
                else if (apiResponse.ErrorResponse != null && apiResponse.ErrorResponse.Error != null)
                {
                    logger.LogError(string.Format("Error Occued at GetLoanQualifer. Error Response= {0}", JsonConvert.SerializeObject(apiResponse.ErrorResponse)));
                    return StatusCode(apiResponse.ErrorResponse.Error.Code, apiResponse.ErrorResponse);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(string.Format("Error Occued at GetLoanQualifer. Error Message= {0}; Stack Trace= {1}", ex.Message, ex.StackTrace));
                return StatusCode(500, ErrorHandling.GetErrorResponse(500, errorMessages.InternalError));
            }
            return response;
        }

        private LoanQualifierRequest GetLoanQualifierRequest(JObject loanInfo, JObject request)
        {
            LoanQualifierRequest loanQualifierRequestInfo = new LoanQualifierRequest();

            loanQualifierRequestInfo.LoanQualifierInfo = new LoanQualiferInfo();
            loanQualifierRequestInfo.LoanQualifierInfo.RequestAction = request["requestAction"] != null ? request["requestAction"].ToString() : "";
            string organizationCode = loanInfo["organizationCode"] != null ? loanInfo["organizationCode"].ToString() : "";
            if (configuration["eppsusername"] != null && configuration["eppsusername"] == ePPSConfiguration.UserName)
            {
                loanQualifierRequestInfo.LoanQualifierInfo.EppsUserName = configuration["eppsusername"];
            }
            else
            {
                loanQualifierRequestInfo.LoanQualifierInfo.EppsUserName = string.Format("{0}{1}", configuration["eppsusername"], organizationCode);
            }
            loanQualifierRequestInfo.LoanQualifierInfo.LockDays = request["lockDays"] != null ? request["lockDays"].ToObject<int[]>() : null;
            loanQualifierRequestInfo.LoanQualifierInfo.LoanType = loanInfo["mortgageType"] != null ? GetLoanType(loanInfo["mortgageType"].ToString()) : "";

            loanQualifierRequestInfo.LoanQualifierInfo.LoanInformation = new LoanInformation();
            loanQualifierRequestInfo.LoanQualifierInfo.LoanInformation.LienPosition = (loanInfo["loanProductData"] != null && loanInfo["loanProductData"]["lienPriorityType"] != null) ? GetLienPosition(loanInfo["loanProductData"]["lienPriorityType"].ToString()) : 0;
            loanQualifierRequestInfo.LoanQualifierInfo.LoanInformation.LoanPurpose = (loanInfo["property"] != null && loanInfo["property"]["loanPurposeType"] != null) ? GetPurpose(loanInfo["property"]["loanPurposeType"].ToString()) : 0;
            if (loanQualifierRequestInfo.LoanQualifierInfo.LoanInformation.LienPosition == 1)
            {
                loanQualifierRequestInfo.LoanQualifierInfo.LoanInformation.FirstMortgageAmount = loanInfo["borrowerRequestedLoanAmount"] != null ? Convert.ToDouble(loanInfo["borrowerRequestedLoanAmount"]) : 0;
            }
            else if (loanQualifierRequestInfo.LoanQualifierInfo.LoanInformation.LienPosition == 2)
            {
                loanQualifierRequestInfo.LoanQualifierInfo.LoanInformation.FirstMortgageAmount = 0;
            }
            loanQualifierRequestInfo.LoanQualifierInfo.LoanInformation.TotalMortgageAmount = loanInfo["baseLoanAmount"] != null ? Convert.ToDouble(loanInfo["baseLoanAmount"]) : 0;
            loanQualifierRequestInfo.LoanQualifierInfo.LoanInformation.FinancedAmount = loanInfo["miAndFundingFeeFinancedAmount"] != null ? Convert.ToDouble(loanInfo["miAndFundingFeeFinancedAmount"]) : 0;
            loanQualifierRequestInfo.LoanQualifierInfo.LoanInformation.CashOut = loanInfo["uldd"]["refinanceCashOutAmount "] != null ? Convert.ToDouble(loanInfo["uldd"]["refinanceCashOutAmount "]) : 0;
            loanQualifierRequestInfo.LoanQualifierInfo.LoanInformation.Target = (loanInfo["rateLock"] != null && loanInfo["rateLock"]["buySideRateNetBuyRate"] != null) ? Convert.ToDouble(loanInfo["rateLock"]["buySideRateNetBuyRate"]) : 0;
            loanQualifierRequestInfo.LoanQualifierInfo.LoanInformation.LoanChannel = request["loanChannel"] != null ? Convert.ToInt32(request["loanChannel"]) : 0;
            loanQualifierRequestInfo.LoanQualifierInfo.LoanInformation.LoanId = ExtractLoanIdInfo(loanInfo);

            loanQualifierRequestInfo.LoanQualifierInfo.Compensation = new Compensation();
            loanQualifierRequestInfo.LoanQualifierInfo.Compensation.Model = request["model"] != null ? Convert.ToInt32(request["model"]) : 0;
            loanQualifierRequestInfo.LoanQualifierInfo.ProductType = GetProductType(loanInfo["loanAmortizationType"] != null ? loanInfo["loanAmortizationType"].ToString() : string.Empty, loanInfo["loanAmortizationTermMonths"] != null ? Convert.ToInt32(loanInfo["loanAmortizationTermMonths"].ToString()) : 0, (loanInfo["loanProductData"] != null && loanInfo["loanProductData"]["subsequentRateAdjustmentMonthsCount"] != null) ? Convert.ToInt32(loanInfo["loanProductData"]["subsequentRateAdjustmentMonthsCount"].ToString()) : 0);
            loanQualifierRequestInfo.LoanQualifierInfo.ProductOptions = GetProductOption((loanInfo["rateLock"] != null && loanInfo["rateLock"]["lenderPaidMortgageInsuranceIndicator"] != null) ? Convert.ToBoolean(loanInfo["rateLock"]["lenderPaidMortgageInsuranceIndicator"]) : false, (loanInfo["rateLock"] != null && loanInfo["rateLock"]["requestImpoundWavied"] != null) ? loanInfo["rateLock"]["requestImpoundWavied"].ToString() : string.Empty, (loanInfo["regulationZ"] != null && loanInfo["regulationZ"]["interestOnly"] != null) ? Convert.ToBoolean(loanInfo["regulationZ"]["interestOnly"]) : false);
            loanQualifierRequestInfo.LoanQualifierInfo.SpecialProducts = loanInfo["loanProgramName"] != null ? GetSpecialProduct(loanInfo["loanProgramName"].ToString()) : null;
            loanQualifierRequestInfo.LoanQualifierInfo.StandardProducts = loanInfo["mortgageType"] != null ? GetStandardProduct(loanInfo["mortgageType"].ToString()) : null;
            loanQualifierRequestInfo.LoanQualifierInfo.DocumentationLevel = request["documentationLevel"] != null ? Convert.ToInt32(request["documentationLevel"]) : 0;

            loanQualifierRequestInfo.LoanQualifierInfo.BorrowerFinancialHistory = new BorrowerFinancialHistory();
            loanQualifierRequestInfo.LoanQualifierInfo.Property = new Property();
            loanQualifierRequestInfo.LoanQualifierInfo.Borrowers = new List<object>();
            for (int i = 0; i < loanInfo["borrowerPairCount"].ToObject<int>(); i++)
            {
                object borrower = new object();
                BorrowerFinanancial borrowerFinanancial = new BorrowerFinanancial();
                borrowerFinanancial.CreditScore = loanInfo["creditScoreToUse"] != null ? Convert.ToInt32(loanInfo["creditScoreToUse"]) : 0;
                if (loanInfo["applications"] != null && loanInfo["applications"][i]["borrower"] != null)
                {
                    borrowerFinanancial.LiquidAsset = loanInfo["applications"][i]["borrower"]["subtotalLiquidAssetsMinusGiftAmount"] != null ? Convert.ToDouble(loanInfo["applications"][i]["borrower"]["subtotalLiquidAssetsMinusGiftAmount"]) : 0;
                    borrowerFinanancial.Income = loanInfo["applications"][i]["borrower"]["baseMonthlyIncomeAmount"] != null ? Convert.ToDouble(loanInfo["applications"][i]["borrower"]["baseMonthlyIncomeAmount"]) * 12 : 0;
                    borrowerFinanancial.MonthlyDebt = loanInfo["applications"][i]["totalMonthlyPaymentAmount"] != null ? Convert.ToDouble(loanInfo["applications"][i]["totalMonthlyPaymentAmount"]) : 0;
                    borrower = new
                    {
                        FirstName = loanInfo["applications"][i]["borrower"]["firstName"] != null ? loanInfo["applications"][i]["borrower"]["firstName"].ToString() : "",
                        LastName = loanInfo["applications"][i]["borrower"]["lastName"] != null ? loanInfo["applications"][i]["borrower"]["lastName"].ToString() : "",
                        BorrowerFinanancial = borrowerFinanancial
                    };
                    loanQualifierRequestInfo.LoanQualifierInfo.BorrowerFinancialHistory.FirstTimeHomeBuyers = (loanInfo["rateLock"] != null && loanInfo["rateLock"]["firstTimeHomebuyersIndicator"] != null) ? Convert.ToBoolean(loanInfo["rateLock"]["firstTimeHomebuyersIndicator"]) : false;
                    loanQualifierRequestInfo.LoanQualifierInfo.Borrowers.Add(borrower);
                }

                if (loanInfo["applications"] != null && loanInfo["applications"][i]["coborrower"] != null && (loanInfo["applications"][i]["coborrower"]["firstName"] != null || loanInfo["applications"][i]["coborrower"]["lastName"] != null))
                {
                    borrower = new
                    {
                        FirstName = loanInfo["applications"][i]["coborrower"]["firstName"] != null ? loanInfo["applications"][i]["coborrower"]["firstName"].ToString() : "",
                        LastName = loanInfo["applications"][i]["coborrower"]["lastName"] != null ? loanInfo["applications"][i]["coborrower"]["lastName"].ToString() : "",
                        BorrowerFinanancial = new
                        {
                            CreditScore = borrowerFinanancial.CreditScore
                        }
                    };
                    loanQualifierRequestInfo.LoanQualifierInfo.Borrowers.Add(borrower);
                }
            }

            loanQualifierRequestInfo.LoanQualifierInfo.BorrowerFinancialHistory.Days30MortgageLatestin12Months = request["30DaysMortgageLatestin12Months"] != null ? Convert.ToInt32(request["30DaysMortgageLatestin12Months"]) : 0;
            loanQualifierRequestInfo.LoanQualifierInfo.BorrowerFinancialHistory.Days60MortgageLatestin12Months = request["60DaysMortgageLatestin12Months"] != null ? Convert.ToInt32(request["60DaysMortgageLatestin12Months"]) : 0;
            loanQualifierRequestInfo.LoanQualifierInfo.BorrowerFinancialHistory.Days90MortgageLatestin12Months = request["90DaysMortgageLatestin12Months"] != null ? Convert.ToInt32(request["90DaysMortgageLatestin12Months"]) : 0;
            loanQualifierRequestInfo.LoanQualifierInfo.BorrowerFinancialHistory.Days30MortgageLatestin24Months = request["30DaysMortgageLatestin24Months"] != null ? Convert.ToInt32(request["30DaysMortgageLatestin24Months"]) : 0;
            loanQualifierRequestInfo.LoanQualifierInfo.BorrowerFinancialHistory.Days60MortgageLatestin24Months = request["60DaysMortgageLatestin24Months"] != null ? Convert.ToInt32(request["60DaysMortgageLatestin24Months"]) : 0;
            loanQualifierRequestInfo.LoanQualifierInfo.BorrowerFinancialHistory.Days90MortgageLatestin24Months = request["90DaysMortgageLatestin24Months"] != null ? Convert.ToInt32(request["90DaysMortgageLatestin24Months"]) : 0;
            loanQualifierRequestInfo.LoanQualifierInfo.BorrowerFinancialHistory.Days120MortgageLatestin12Months = request["120DaysMortgageLatestin12Months"] != null ? Convert.ToInt32(request["120DaysMortgageLatestin12Months"]) : 0;
            loanQualifierRequestInfo.LoanQualifierInfo.BorrowerFinancialHistory.NoticeOfDefaultForeClosure = request["noticeOfDefaultForeClosure"] != null ? request["noticeOfDefaultForeClosure"].ToString() : null;
            loanQualifierRequestInfo.LoanQualifierInfo.BorrowerFinancialHistory.BankruptcyInMonths = request["bankruptcyInMonths"] != null ? request["bankruptcyInMonths"].ToString() : null;
            loanQualifierRequestInfo.LoanQualifierInfo.BorrowerFinancialHistory.DemonstrateHousingPaymentHistory = (loanInfo["rateLock"] != null && loanInfo["rateLock"]["twelveMonthMortgageRentalHistoryIndicator"] != null) ? Convert.ToBoolean(loanInfo["rateLock"]["twelveMonthMortgageRentalHistoryIndicator"]) : false;

            loanQualifierRequestInfo.LoanQualifierInfo.Property.Value = GetPropertyValue(loanInfo["propertyAppraisedValueAmount"], loanInfo["propertyEstimatedValueAmount"]);
            loanQualifierRequestInfo.LoanQualifierInfo.Property.Type = GetPropertType(loanInfo["property"]["financedNumberOfUnits"] != null ? Convert.ToInt32(loanInfo["property"]["financedNumberOfUnits"].ToString()) : 0, loanInfo["loanProductData"]["gsePropertyType"] !=null ? loanInfo["loanProductData"]["gsePropertyType"].ToString().ToLower() : string.Empty);
            loanQualifierRequestInfo.LoanQualifierInfo.Property.Use = (loanInfo["property"] != null && loanInfo["property"]["propertyUsageType"] != null) ? GetOccupancy(loanInfo["property"]["propertyUsageType"].ToString()) : 0;
            loanQualifierRequestInfo.LoanQualifierInfo.Property.Zip = (loanInfo["property"] != null && loanInfo["property"]["postalCode"] != null) ? loanInfo["property"]["postalCode"].ToString() : "";
            loanQualifierRequestInfo.LoanQualifierInfo.Property.StreetAddress = (loanInfo["property"] != null && loanInfo["property"]["streetAddress"] != null) ? loanInfo["property"]["streetAddress"].ToString() : "";
            loanQualifierRequestInfo.LoanQualifierInfo.Property.City = (loanInfo["property"] != null && loanInfo["property"]["city"] != null) ? loanInfo["property"]["city"].ToString() : "";
            loanQualifierRequestInfo.LoanQualifierInfo.Property.County = (loanInfo["property"] != null && loanInfo["property"]["county"] != null) ? loanInfo["property"]["county"].ToString() : "";
            loanQualifierRequestInfo.LoanQualifierInfo.Property.State = (loanInfo["property"] != null && loanInfo["property"]["state"] != null) ? loanInfo["property"]["state"].ToString() : "";
            loanQualifierRequestInfo.LoanQualifierInfo.Property.Rural = request["rural"] != null ? Convert.ToBoolean(request["rural"]) : false;
            loanQualifierRequestInfo.LoanQualifierInfo.Property.Tax = loanInfo["proposedRealEstateTaxesAmount"] != null ? Convert.ToDouble(loanInfo["proposedRealEstateTaxesAmount"]) * 12 : 0;
            loanQualifierRequestInfo.LoanQualifierInfo.Property.InsuranceAmount = loanInfo["proposedHazardInsuranceAmount"] != null ? Convert.ToDouble(loanInfo["proposedHazardInsuranceAmount"]) * 12 : 0;
            loanQualifierRequestInfo.LoanQualifierInfo.Property.AssociationFee = loanInfo["proposedDuesAmount"] != null ? Convert.ToDouble(loanInfo["proposedDuesAmount"]) : 0;
            loanQualifierRequestInfo.LoanQualifierInfo.Property.RentalIncome = loanInfo["subjectPropertyGrossRentalIncomeAmount"] != null ? Convert.ToDouble(loanInfo["subjectPropertyGrossRentalIncomeAmount"]) : 0;

            loanQualifierRequestInfo.CurrentRateLockDetails = new CurrentRateLockDetails();
            loanQualifierRequestInfo.CurrentRateLockDetails.LoanProgram = loanInfo["loanProgramName"] != null ? loanInfo["loanProgramName"].ToString() : "";
            loanQualifierRequestInfo.CurrentRateLockDetails.RateLockInvestor = (loanInfo["rateLock"] != null && loanInfo["rateLock"]["investorName"] != null) ? loanInfo["rateLock"]["investorName"].ToString() : "";
            loanQualifierRequestInfo.CurrentRateLockDetails.NetbyRate = (loanInfo["rateLock"] != null && loanInfo["rateLock"]["buySideRateNetBuyRate"] != null) ? Convert.ToDouble(loanInfo["rateLock"]["buySideRateNetBuyRate"]) : 0;

            Double buySidePriceNetBuyPrice = (loanInfo["rateLock"] != null && loanInfo["rateLock"]["buySidePriceNetBuyPrice"] != null) ? Convert.ToDouble(loanInfo["rateLock"]["buySidePriceNetBuyPrice"]) : 0;
            Double totalPrice = (loanInfo["rateLock"] != null && loanInfo["rateLock"]["totalPrice"] != null) ? Convert.ToDouble(loanInfo["rateLock"]["totalPrice"]) : 0;
            loanQualifierRequestInfo.CurrentRateLockDetails.NetbyPrice = GetNetbuyPrice(buySidePriceNetBuyPrice, totalPrice);            
            return loanQualifierRequestInfo;
        }
        private int GetLienPosition(string lienPosition)
        {
            int lien_Position = 0;
            switch (lienPosition)
            {
                case "FirstLien":
                    lien_Position = 1;
                    break;
                case "SecondLien":
                    lien_Position = 2;
                    break;
            }
            return lien_Position;
        }
        private int GetPurpose(string purpose)
        {
            int purpose_Type = 0;
            switch (purpose)
            {
                case "Purchase":
                    purpose_Type = 1;
                    break;
                case "NoCash-Out Refinance":
                    purpose_Type = 2;
                    break;
                case "Cash-Out Refinance":
                    purpose_Type = 3;
                    break;
                case "ConstructionOnly":
                    purpose_Type = 4;
                    break;
                case "ConstructionToPermanent":
                    purpose_Type = 5;
                    break;
            }
            return purpose_Type;
        }

        private int GetPropertType(int noOfUnits, string gsePropertyType)
        {
            int property_Type = 0;
            switch (noOfUnits)
            {
                case int x when x <= 1:
                    switch (gsePropertyType)
                    {
                        case "detached":
                            property_Type = 1;
                            break;
                        case "condominium":
                            property_Type = 5;
                            break;
                        case "highrisecondominium":
                            property_Type = 7;
                            break;
                        case "attached":
                            property_Type = 8;
                            break;
                        case "cooperative":
                            property_Type = 9;
                            break;
                        case "manufacturedhousing":
                        case "manufacturedhomecondopudcoop":
                        case "mhselect":
                            property_Type = 11;
                            break;
                        case "detachedcondo":
                            property_Type = 12;
                            break;
                        case "pud":
                            property_Type = 13;
                            break;
                        default:
                            property_Type = 0;
                            break;
                    }
                    break;
                case 2:
                    property_Type = 2;
                    break;
                case 3:
                    property_Type = 3;
                    break;
                case int x when x >= 4:
                    property_Type = 4;
                    break;
                default:
                    property_Type = 0;
                    break;
            }
            return property_Type;
        }

        private int GetOccupancy(string occupancy)
        {
            int occupancy_Type = 0;
            switch (occupancy)
            {
                case "PrimaryResidence":
                    occupancy_Type = 1;
                    break;
                case "SecondHome":
                    occupancy_Type = 2;
                    break;
                case "Investor":
                    occupancy_Type = 3;
                    break;
            }
            return occupancy_Type;
        }

        private int[] GetProductType(string amortizationType, int term, int adjustableMonth)
        {
            int[] occupancy_Type = new int[1];

            if (amortizationType.ToLower().Equals("fixed"))
            {
                if (term >= 361 && term <= 480)
                {
                    occupancy_Type.SetValue(14, 0);
                }
                else if (term >= 301 && term <= 360)
                {
                    occupancy_Type.SetValue(1, 0);
                }
                else if (term >= 241 && term <= 300)
                {
                    occupancy_Type.SetValue(20, 0);
                }
                else if (term >= 181 && term <= 240)
                {
                    occupancy_Type.SetValue(2, 0);
                }
                else if (term >= 121 && term <= 180)
                {
                    occupancy_Type.SetValue(3, 0);
                }
                else if (term >= 1 && term <= 120)
                {
                    occupancy_Type.SetValue(4, 0);
                }
                else
                {
                    occupancy_Type.SetValue(109, 0);
                }
            }
            else if (amortizationType.ToLower().Equals("adjustablerate"))
            {
                if (adjustableMonth.Equals(120))
                {
                    occupancy_Type.SetValue(5, 0);
                }
                else if (adjustableMonth.Equals(84))
                {
                    occupancy_Type.SetValue(6, 0);
                }
                else if (adjustableMonth.Equals(60))
                {
                    occupancy_Type.SetValue(7, 0);
                }
                else if (adjustableMonth.Equals(36))
                {
                    occupancy_Type.SetValue(8, 0);
                }
                else
                {
                    occupancy_Type.SetValue(109, 0);
                }
            }
            else
            {
                occupancy_Type.SetValue(109, 0);
            }
            return occupancy_Type;
        }

        private string ExtractLoanIdInfo(JObject loanInfo)
        {
            string loanId = string.Empty;
            foreach (JToken customField in loanInfo["customFields"])
            {
                if (customField != null && customField["fieldName"].ToString() == "CX.EPPS.LOAN.ID" && customField["stringValue"] != null)
                {
                    loanId = customField["stringValue"].ToString();
                }
            }
            return loanId;
        }
        public async Task<Object> UpdateLoanWithTemplate(string loanGuid, string loanTemplatePath, string loanProgramPath, string closingCostPath, Object request)
        {
            Object response = new Object();
            try
            {
                string apiURL = string.Format(encompassAPIs.UpdateLoanWithTemplate, loanGuid, loanTemplatePath);
                ApiResponse<Object> apiResponse = await httpService.PatchAsync<Object>(apiURL, request).ConfigureAwait(false);
                if (apiResponse.Success)
                {
                    response = apiResponse.Data;
                    if (!(string.IsNullOrEmpty(loanProgramPath) || string.IsNullOrEmpty(closingCostPath)))
                    {
                        await ApplyLoanProgramClosingCost(loanGuid, loanProgramPath, closingCostPath);
                    }
                }
                else if (apiResponse.ErrorResponse != null && apiResponse.ErrorResponse.Error != null)
                {
                    logger.LogError(string.Format("Error occurred at UpdateLoanWithTemplate. Error Message={0}.", apiResponse.ErrorResponse.Error.Message));
                    return StatusCode(apiResponse.ErrorResponse.Error.Code, apiResponse.ErrorResponse);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(string.Format("Error occurred at UpdateLoanWithTemplate. Error Stack Trace={0}. Error Message={1}.", ex.StackTrace, ex.Message));
                return StatusCode(500, ErrorHandling.GetErrorResponse(500, ex.Message));
            }
            return response;
        }

        public async Task<Object> ApplyLoanProgramClosingCost(string loanGuid, string loanProgramPath, string closingCostPath)
        {
            Object response = new Object();
            try
            {
                string applyTemplateUrl = String.Format(encompassAPIs.ApplyTemplate);
                var templateUpdateRequest = new
                {
                    loanGuid,
                    loanProgramPath,
                    closingCostPath,
                    loanConfiguration.URLAVersion
                };
                ApiResponse<object> apiApplyLoanProgramResponse = await httpService.PostAsync<object>(applyTemplateUrl, templateUpdateRequest, false, true).ConfigureAwait(false);
                if (apiApplyLoanProgramResponse != null && apiApplyLoanProgramResponse.Success)
                {
                    response = apiApplyLoanProgramResponse.Data;
                }
                else if (apiApplyLoanProgramResponse != null && !apiApplyLoanProgramResponse.Success && apiApplyLoanProgramResponse.ErrorResponse != null && apiApplyLoanProgramResponse.ErrorResponse.Error != null)
                {
                    logger.LogError(string.Format("Error occurred at ApplyLoanProgramClosingCost. Error Message={0},", apiApplyLoanProgramResponse.ErrorResponse.Error.Message));
                    return StatusCode(apiApplyLoanProgramResponse.ErrorResponse.Error.Code, apiApplyLoanProgramResponse.ErrorResponse);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(string.Format("Error occurred at ApplyLoanProgramClosingCost. Error Stack Trace={0}. Error Message={1}.", ex.StackTrace, ex.Message));
                return StatusCode(500, ErrorHandling.GetErrorResponse(500, ex.Message));
            }
            return response;
        }

        private async Task AddCustomPropertiesInAppInsights(Object requestBody, Object responseBody, string apiUrl, string loanGuid, string method, string purpose)
        {
            TelemetryConfiguration telemetryConfiguration = TelemetryConfiguration.CreateDefault();
            telemetryConfiguration.InstrumentationKey = configuration["ApplicationInsights:InstrumentationKey"];
            TelemetryClient telemetryClient = new TelemetryClient(telemetryConfiguration);
            Dictionary<string, string> customProperties = new Dictionary<string, string>
            {
                {"Service", "EPPS" },
                {"Method", method },
                {"LoanGuid", loanGuid },
                {"RequestBody", JsonConvert.SerializeObject(requestBody) },
                {"ResponseBody", JsonConvert.SerializeObject(responseBody) },
                {"Purpose", purpose },
            };
            //telemetryClient.TrackEvent("SSPL Details", customProperties);
            telemetryClient.TrackTrace(string.Format("Service= {0}; Event= {1}; {2}; ", "EPPS", "TrackTrace", apiUrl), SeverityLevel.Information, customProperties);
            telemetryClient.Flush();
            telemetryConfiguration.Dispose();
        }
        private string GetLoanType(string loanType)
        {
            string loan_Type = string.Empty;
            switch (loanType)
            {
                case "HELOC":
                    loan_Type = "Heloc";
                    break;
                default:
                    loan_Type = "NonHeloc";
                    break;
            }
            return loan_Type;
        }
        private Double GetNetbuyPrice(double buySidePriceNetBuyPrice, double totalPrice)
        {
            double rate = buySidePriceNetBuyPrice - totalPrice;
            return rate;
        }
        private int[] GetStandardProduct(string loanType)
        {
            int[] loan_Type = null;
            switch (loanType)
            {
                case "FHA":
                    loan_Type = new int[1];
                    loan_Type[0] = 2;
                    break;
                case "VA":
                    loan_Type = new int[1];
                    loan_Type[0] = 3;
                    break;
                case "USDA":
                    loan_Type = new int[1];
                    loan_Type[0] = 4;
                    break;
                default:
                    loan_Type = new int[3];
                    loan_Type[0] = 1;
                    loan_Type[1] = 5;
                    loan_Type[2] = 6;
                    break;
            }
            return loan_Type;
        }

        private int[] GetSpecialProduct(string loanProgramName)
        {
            int[] loanPgmName = null;
            switch (loanProgramName)
            {
                case "FHA Streamline":
                    loanPgmName = new int[1];
                    loanPgmName[0] = 27;
                    break;
                case "VA IRRRL":
                    loanPgmName = new int[1];
                    loanPgmName[0] = 55;
                    break;
                case "HOMEPOSSIBLE":
                    loanPgmName = new int[1];
                    loanPgmName[0] = 20;
                    break;
                case "HOMEREADY":
                    loanPgmName = new int[1];
                    loanPgmName[0] = 556;
                    break;
                case "REFINOW":
                    loanPgmName = new int[1];
                    loanPgmName[0] = 1426;
                    break;
                default:
                    loanPgmName = new int[1];
                    loanPgmName[0] = 0;
                    break;
            }
            return loanPgmName;
        }

        public async Task<Object> GetLoanV2(string loanGuid, string entities)
        {
            Object response = new Object();
            string apiUrl = string.Empty;
            try
            {
                if (string.IsNullOrEmpty(entities))
                {
                    apiUrl = string.Format(encompassAPIs.RetrieveLoanV2, loanGuid);
                }
                else
                {
                    apiUrl = string.Format(encompassAPIs.RetrieveLoanWithSpecificEntitiesV2, loanGuid, entities);
                }
                ApiResponse<Object> apiResponse = await httpService.GetAsync<Object>(apiUrl).ConfigureAwait(false);
                if (apiResponse.Success)
                {
                    response = apiResponse.Data;
                }
                else if (apiResponse.ErrorResponse != null && apiResponse.ErrorResponse.Error != null)
                {
                    logger.LogError(string.Format("Error occurred at GetLoanV2. Error Code={0}. Error Message={1}.", apiResponse.ErrorResponse.Error.Code, apiResponse.ErrorResponse));
                    return StatusCode(apiResponse.ErrorResponse.Error.Code, apiResponse.ErrorResponse);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(string.Format("Error occurred at GetLoanV2. Error Stack Trace={0}. Error Message={1}.", ex.StackTrace, ex.Message));
                return StatusCode(500, string.Format("Error occurred at GetLoanV2. Error Stack Trace={0}. Error Message={1}.", ex.StackTrace, ex.Message));
            }
            return response;
        }

        public async Task<Object> UpdateLoanV2(string loanGuid, Object request)
        {
            Object response = new Object();
            try
            {
                string apiURL = string.Format(encompassAPIs.UpdateLoanV2, loanGuid);
                ApiResponse<Object> apiResponse = await httpService.PatchAsync<Object>(apiURL, request).ConfigureAwait(false);
                AddCustomPropertiesInAppInsights(JsonConvert.SerializeObject(request), JsonConvert.SerializeObject(apiResponse.Data), apiURL, loanGuid, "UpdateLoan", "Update Loan in Encompass");
                if (apiResponse.Success)
                {
                    response = apiResponse.Data;
                }
                else if (apiResponse.ErrorResponse != null && apiResponse.ErrorResponse.Error != null)
                {
                    logger.LogError(string.Format("Error occurred at UpdateLoanV2. Error Code={0}. Error Message={1}.", apiResponse.ErrorResponse.Error.Code, apiResponse.ErrorResponse));
                    return StatusCode(apiResponse.ErrorResponse.Error.Code, apiResponse.ErrorResponse);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(string.Format("Error occurred at UpdateLoanV2. Error Stack Trace={0}. Error Message={1}.", ex.StackTrace, ex.Message));
                return StatusCode(500, string.Format("Error occurred at UpdateLoanV2. Error Stack Trace={0}. Error Message={1}.", ex.StackTrace, ex.Message));
            }
            return response;
        }

        private double GetPropertyValue(JToken propertyAppraisedValueAmount, JToken propertyEstimatedValueAmount)
        {
            double propertyValue = 0;
            if (propertyAppraisedValueAmount != null && Convert.ToDouble(propertyAppraisedValueAmount) > 0)
            {
                propertyValue = Convert.ToDouble(propertyAppraisedValueAmount);
            }
            else
            {
                propertyValue = propertyEstimatedValueAmount != null ? Convert.ToInt32(propertyEstimatedValueAmount) : 0;
            }
            return propertyValue;
        }

        private List<int> GetProductOption(bool lenderPaidMortgageInsuranceIndicator, string requestImpoundWavied, bool interestOnly)
        {
            List<int> product_Option = new List<int>();
            if (lenderPaidMortgageInsuranceIndicator)
            {
                product_Option.Add(1);
            }
            if (requestImpoundWavied.ToLower().Equals("waived"))
            {
                product_Option.Add(2);
            }
            if (interestOnly)
            {
                product_Option.Add(4);
            }
            return product_Option;
        }
    }
}