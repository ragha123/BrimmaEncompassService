using System;

namespace Brimma.LOSService.DTO
{
    public class DisclosureTracking2015LogIntentToProceed
    {
        public bool Intent { get; set; }
        public DateTime Date { get; set; }
        public string ReceivedMethod { get; set; }
        public string ReceivedMethodOther { get; set; }
    }
}