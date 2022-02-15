using System;
using System.Collections.Generic;
using System.Text;

namespace Brimma.LOSService.DTO
{
    public class BusinessContactRequest
    {
        public string CompanyName { get; set; }
        public int CategoryId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public BusinessContactCurrentMailingAddress CurrentMailingAddress { get; set; }
    }
}
