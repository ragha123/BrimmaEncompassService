namespace Brimma.LOSService.DTO
{
    public class PipelineSearchRequest
    {
        public string Name { get; set; }
        public string LoanType { get; set; }
        public string MileStone { get; set; }
        public int Limit { get; set; }
        public int Start { get; set; }
        public string Cursor { get; set; }
    }
}
