using System;
using System.Collections.Generic;
using System.Text;

namespace Brimma.LOSService.DTO
{
    public class Property
    {
        public double Value { get; set; }
        public int Type { get; set; }
        public int Use { get; set; }
        public string Zip { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string County { get; set; }
        public string State { get; set; }
        public bool Rural { get; set; }
        public double Tax { get; set; }
        public double InsuranceAmount { get; set; }
        public double AssociationFee { get; set; }
        public double RentalIncome { get; set; }
    }
}
