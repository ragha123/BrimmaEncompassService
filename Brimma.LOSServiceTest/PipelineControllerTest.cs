using System;
using System.Collections.Generic;

namespace Brimma.LOSServiceTest
{
    public class PipelineControllerTest
    {
        private Object GetLoansList()
        {
            var response = new
            {
                total = "5",
                cursor = "a9d81e99-4497-411d-ad35-c0efdc1475af",
                loans = new List<Object>() {
                new
                {
                    loanNumber = "TEST190100133",
                    loanGuid = "ac30785b-d4ac-4aec-9eb6-a3b211073baf",
                    borrower = new
                    {
                        firstName = "Robert A",
                        lastName = "Roman"
                    },
                    coBorrower = new
                    {
                        firstName = "Kathleen A",
                        lastName = "Roman"
                    },
                    propertyAddress = new
                    {
                        street = "6319 Goldeneye St.",
                        city = "Ventura",
                        state = "CA",
                        zip = "93003"
                    }
                }
                }
            };
            return response;
        }

        private Object GetLoanStatus()
        {
            var response = new
            {
                loanNumber = "TEST190401238",
                loanGuid = "4eea2080-211b-4d3b-bae7-4aa4b27fdbc0",
                borrower = new
                {
                    firstName = "Supree",
                    lastName = "Periasamy",
                    homePhone = "919-263-1017",
                    workPhone = "",
                    cellPhone = "201-314-0412",
                    homeEMail = "periyasamy.thirumala",
                    workEMail = "supree@brimmatech.com"
                },
                coBorrower = new
                {
                    firstName = "Kavitha",
                    lastName = "Subramanian"
                },
                propertyAddress = new
                {
                    street = "7605 Carrick Hill CT",
                    city = "Wake Forest",
                    state = "NC",
                    zip = "27587"
                },
                currentMilestone = "File started",
                currentMilestoneDate = "4/6/2019 9=54=57 PM",
                nextExpectedMilestone = "Processing",
                loanOfficerName = ""
            };
            return response;
        }

    }
}
