namespace Brimma.LOSService.DTO
{
    public class AttachmentInfo
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public string ObjectId { get; set; }
        public bool IsActive { get; set; }
        public AssignedToInfo AssignedTo { get; set; }
        public long FileSize { get; set; }
        public bool IsRemoved { get; set; }
        public CreatedByInfo CreatedBy { get; set; }
        public string CreatedDate { get; set; }        
    }
}