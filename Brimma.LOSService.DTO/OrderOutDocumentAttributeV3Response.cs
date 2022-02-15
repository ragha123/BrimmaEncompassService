using System;
using System.Collections.Generic;
using System.Text;

namespace Brimma.LOSService.DTO
{
    public class OrderOutDocumentAttributeV3Response
    {
        public string Title { get; set; }
        public string Id { get; set; }
        public DateTime requestedDate { get; set; }
        public List<OrderOutAttachmentAttributes> Attachments { get; set; }
    }
}