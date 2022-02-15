using System.Collections.Generic;
namespace Brimma.LOSService.DTO
{
    public class PipelineRequest
    {
        public Filter Filter { get; set; }
        public List<string> Fields { get; set; }
        public List<SortOrder> SortOrder { get; set; }
    }

    public class SortOrder
    {
        public string CanonicalName { get; set; }
        public string Order { get; set; }
    }

    public class Filter
    {
        public string Operator { get; set; }
        public List<Term> Terms { get; set; }
    }

    public class Term
    {
        public string CanonicalName { get; set; }
        public string Value { get; set; }
        public string MatchType { get; set; }
    }
}
