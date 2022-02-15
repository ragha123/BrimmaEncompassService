using System;
using System.Collections.Generic;
using System.Text;

namespace Brimma.LOSService.DTO
{
    public class CreateAttachmentRequest
    {
        public string Title { get; set; }
        public File File { get; set; }
    }
}
