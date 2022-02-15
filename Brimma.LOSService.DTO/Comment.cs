using System;
using System.Collections.Generic;
using System.Text;

namespace Brimma.LOSService.DTO
{
    public class Comment
    {
        public string Comments { get; set; }
        public int ForRoleId { get; set; }
        public string CommentId { get; set; }
        public string DateCreated { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public string DateReviewed { get; set; }
        public string ReviewedBy { get; set; }
    }
}
