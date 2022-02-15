using System;
using System.Collections.Generic;
using System.Text;

namespace Brimma.LOSService.DTO
{
    public class Borrowers
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public BorrowerFinanancial BorrowerFinanancial { get; set; }
    }
}
