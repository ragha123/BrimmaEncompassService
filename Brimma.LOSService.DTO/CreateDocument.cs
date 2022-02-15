using System;
using System.Collections.Generic;
using System.Text;

namespace Brimma.LOSService.DTO
{
    public class UploadAttachment
    {
        public string DocumentTitle { get; set; }
        public string AttachmentTitle { get; set; }
        public string ApplicationId { get; set; }
        public byte[] AttachmentByteArrayData { get; set; }
        public string ContentType { get; set; }
        public string AttachmentTitleWithFileExtension { get; set; }
    }
}
