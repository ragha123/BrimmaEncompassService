﻿namespace Brimma.LOSService.DTO
{
    public class LockRequest
    {
        public Resource Resource { get; set; }
        public string LockType { get; set; }
    }

    public class Resource
    {
        public string EntityId { get; set; }
        public string EntityType { get; set; }
    }
}
