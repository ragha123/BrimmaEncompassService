using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Brimma.LOSService.DTO
{
    public class EmploymentInfo
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "isCurrent")]
        public bool IsCurrent { get; set; }

        [JsonProperty(PropertyName = "address")]
        public Address Address { get; set; }

        [JsonProperty(PropertyName = "phoneNumber")]
        public string PhoneNumber { get; set; }

        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "startDate")]
        public string StartDate { get; set; }

        [JsonProperty(PropertyName = "endDate")]
        public string EndDate { get; set; }

        [JsonProperty(PropertyName = "experienceInMonths")]
        public int ExperienceInMonths { get; set; }

        [JsonProperty(PropertyName = "selfEmployed")]
        public bool SelfEmployed { get; set; }

        [JsonProperty(PropertyName = "monthlyIncomeOrLoss")]
        public double MonthlyIncomeOrLoss { get; set; }

        [JsonProperty(PropertyName = "monthlyIncome")]
        public Income MonthlyIncome { get; set; }
    }
}
