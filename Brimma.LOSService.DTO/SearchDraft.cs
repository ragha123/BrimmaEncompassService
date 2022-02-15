using System;
using System.Collections.Generic;
using System.Text;

namespace Brimma.LOSService.DTO
{
    public class SearchDraft
    {
        public string searchText { get; set; }
        public string sortField { get; set; }
        public string sortOrder { get; set; }
        public int start { get; set; }
        public int limit { get; set; }
    }
}
