using System;
using System.Collections.Generic;
using System.Text;

namespace Brimma.LOSService.DTO
{
    public class CreateAttachmentResponse
    {
        public string AttachmentId { get; set; }
        public string AuthorizationHeader { get; set; }
        public bool MultiChunkRequired { get; set; }
        public string UploadUrl { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
