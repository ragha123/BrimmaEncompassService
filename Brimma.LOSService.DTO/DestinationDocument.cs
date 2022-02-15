using System;
using System.Collections.Generic;
using System.Text;

namespace Brimma.LOSService.DTO
{
    public class DestinationDocument
    {
        public string Title { get; set; }
        public string Id { get; set; }
        public List<AttachmentAssignInfo> Attachments { get; set; }
    }
}
