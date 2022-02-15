using System;
using System.Collections.Generic;
using System.Text;

namespace Brimma.LOSService.DTO
{
    public class Attachment
    {
        public string EntityId { get; set; }
        public string EntityName { get; set; }
        public string EntityType { get; set; }
        public string OriginalUrl { get; set; }
        public bool IsActive { get; set; }
        public string Url { get; set; }
        public string AuthorizationHeader { get; set; }
    }
}
