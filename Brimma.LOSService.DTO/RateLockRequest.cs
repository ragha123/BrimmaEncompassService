using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Brimma.LOSService.DTO
{
    public class RateLockRequest
    {
        [Required]
        public LockRequestEntity LockRequest { get; set; }
        public List<CustomField> CustomFieldUpdateRequest { get; set; }

    }
    public class LockRequestEntity
    {
        [Required]
        public int? DaystoExtend { get; set; } 
        public decimal? LockExtendPriceAdjustment { get; set; } 
        public string Comments { get; set; }
    }

    //can be extended with multiple actions
    public enum Action
    {
        Extend 
    }
}

