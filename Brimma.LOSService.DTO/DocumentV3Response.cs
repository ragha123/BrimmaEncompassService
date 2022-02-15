using System;
using System.Collections.Generic;
using System.Text;

namespace Brimma.LOSService.DTO
{
    public class DocumentV3Response
    {
        public string Title { get; set; }
        public string Id { get; set; }
        public Object Application { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public DateTime? StatusDate { get; set; }
        public DateTime? CreatedDate { get; set; }
        public List<Attachment> Attachments { get; set; }
    }
}
