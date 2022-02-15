using System;
using System.Collections.Generic;
using System.Text;

namespace Brimma.LOSService.DTO
{
    public class DocumentInfo
    {
        public string DocumentId { get; set; }
        public string TitleWithIndex { get; set; }
        public string ApplicationName { get; set; }
        public string MilestoneId { get; set; }
        public bool WebCenterAllowed { get; set; }
        public bool TpoAllowed { get; set; }
        public bool ThirdPartyAllowed { get; set; }
        public bool IsRequested { get; set; }
        public string RequestedBy { get; set; }
        public bool IsRerequested { get; set; }
        public string RerequestedBy { get; set; }
        public int DaysDue { get; set; }
        public bool IsReceived { get; set; }
        public List<Comment> Comments { get; set; }
        public List<Role> Roles { get; set; }
    }
}
