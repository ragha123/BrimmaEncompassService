using System;
using System.Collections.Generic;
using System.Text;

namespace Brimma.LOSService.DTO
{
    public class Document
    {
        public string Title { get; set; }
        public string DocumentId { get; set; }
        public string ApplicationId { get; set; }
        public string ApplicationName { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string StatusDate { get; set; }
        public string RequestedBy { get; set; }
        public string ReceivedBy { get; set; }
        public string CreatedBy { get; set; }
        public List<Attachment> Attachments { get; set; }        
    }
}
