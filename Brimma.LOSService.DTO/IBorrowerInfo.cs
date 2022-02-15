using System;
using System.Collections.Generic;
using System.Text;

namespace Brimma.LOSService.DTO
{
    public interface IBorrowerInfo
    {
        public PersonalInfo PersonalInfo { get; set; }
        public ResidenceAddress CurrentAddress { get; set; }
        public bool HasSeparateMailingAddress { get; set; }
        public ResidenceAddress MailingAddress { get; set; }
        public List<ResidenceAddress> FormerAddresses { get; set; }
        public bool HasDependents { get; set; }
        public int[] AgeOfDependents { get; set; }
        public List<EmploymentInfo> EmploymentInfo { get; set; }
        public Demographics Demographics { get; set; }
    }
}
