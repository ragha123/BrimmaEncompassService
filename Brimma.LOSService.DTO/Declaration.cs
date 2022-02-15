using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Brimma.LOSService.DTO
{
    public class Declaration
    {
        [JsonProperty(PropertyName = "isPrimaryResidence")]
        public bool IsPrimaryResidence { get; set; }

        [JsonProperty(PropertyName = "hadOwnership")]
        public bool HadOwnership { get; set; }

        [JsonProperty(PropertyName = "ownershipPropertyType")]
        public string OwnershipPropertyType { get; set; }

        [JsonProperty(PropertyName = "ownershipPropertyTitle")]
        public string OwnershipPropertyTitle { get; set; }

        [JsonProperty(PropertyName = "hasRelationshipWithSeller")]
        public bool HasRelationshipWithSeller { get; set; }

        [JsonProperty(PropertyName = "hasUndisclosedFunding")]
        public bool HasUndisclosedFunding { get; set; }

        [JsonProperty(PropertyName = "undisclosedFundingAmount")]
        public double UndisclosedFundingAmount { get; set; }

        [JsonProperty(PropertyName = "undisclosedFundingText")]
        public string UndisclosedFundingText { get; set; }

        [JsonProperty(PropertyName = "hasUndisclosedLoans")]
        public bool HasUndisclosedLoans { get; set; }

        [JsonProperty(PropertyName = "hasUndisclosedCredit")]
        public bool HasUndisclosedCredit { get; set; }

        [JsonProperty(PropertyName = "hasPriorityLien")]
        public bool HasPriorityLien { get; set; }

        [JsonProperty(PropertyName = "isCosignerOnUndisclosedDebt")]
        public bool IsCosignerOnUndisclosedDebt { get; set; }

        [JsonProperty(PropertyName = "cosignerOnUndisclosedDebtText")]
        public string CosignerOnUndisclosedDebtText { get; set; }

        [JsonProperty(PropertyName = "hasOutstandingJudgments")]
        public bool HasOutstandingJudgments { get; set; }

        [JsonProperty(PropertyName = "outstandingJudgmentsText")]
        public string outstandingJudgmentsText { get; set; }

        [JsonProperty(PropertyName = "isDelinquentOnDebt")]
        public bool IsDelinquentOnDebt { get; set; }

        [JsonProperty(PropertyName = "delinquentOnDebtText")]
        public string DelinquentOnDebtText { get; set; }

        [JsonProperty(PropertyName = "isPartyToLawsuit")]
        public bool IsPartyToLawsuit { get; set; }

        [JsonProperty(PropertyName = "partyToLawsuitText")]
        public string PartyToLawsuitText { get; set; }

        [JsonProperty(PropertyName = "hasConveyedTitle")]
        public bool HasConveyedTitle { get; set; }

        [JsonProperty(PropertyName = "hasCompletedShortSale")]
        public bool HasCompletedShortSale { get; set; }

        [JsonProperty(PropertyName = "hasForeclosuredProperty")]
        public bool HasForeclosuredProperty { get; set; }

        [JsonProperty(PropertyName = "foreclosedPropertyText")]
        public string ForeclosedPropertyText { get; set; }

        [JsonProperty(PropertyName = "hasDeclaredBankruptcy")]
        public bool HasDeclaredBankruptcy { get; set; }

        [JsonProperty(PropertyName = "bankruptcyTypes")]
        public List<string> BankruptcyTypes { get; set; }

        [JsonProperty(PropertyName = "declaredBankruptcyText")]
        public string DeclaredBankruptcyText { get; set; }
    }
}
