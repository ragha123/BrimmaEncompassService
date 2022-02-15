using System;
using System.Collections.Generic;
using System.Text;

namespace Brimma.LOSService.DTO
{
    public class AttachmentAssignRequest
    {
        public string SourceDocumentTitle { get; set; }
        public string SourceDocumentId { get; set; }
        public List<DestinationDocument> DestinationDocuments { get; set; }
    }
}
