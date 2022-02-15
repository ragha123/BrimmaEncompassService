using System;
using System.Collections.Generic;
using System.Text;

namespace Brimma.LOSService.DTO
{
    public class DownloadOriginalAttachment
    {
        public string Id { get; set; }
        public string[] OriginalUrls { get; set; }
        public string Url { get; set; }
        public string AuthorizationHeader { get; set; }
    }
}
