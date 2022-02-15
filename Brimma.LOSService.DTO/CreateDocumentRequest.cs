using System;
using System.Collections.Generic;
using System.Text;

namespace Brimma.LOSService.DTO
{
    public class CreateDocumentRequest
    {
        public string Title { get; set; }
        public List<Attachment> Attachments { get; set; }
    }
}
