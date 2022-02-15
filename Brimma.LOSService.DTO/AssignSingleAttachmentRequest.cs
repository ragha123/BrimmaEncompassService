using System;
using System.Collections.Generic;
using System.Text;

namespace Brimma.LOSService.DTO
{
    public class AssignSingleAttachmentRequest
    {
        public string AttachmentId { get; set; }
        public string DestinationDocumentTitle { get; set; }
        public string SourceDocumentId { get; set; }
    }
}
