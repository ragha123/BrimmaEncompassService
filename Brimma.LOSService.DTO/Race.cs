using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Brimma.LOSService.DTO
{
    public class Race
    {
        [JsonProperty(PropertyName = "dnp")]
        public bool Dnp { get; set; }

        [JsonProperty(PropertyName = "americanIndian")]
        public bool AmericanIndian { get; set; }

        [JsonProperty(PropertyName = "tribeName")]
        public string TribeName { get; set; }

        [JsonProperty(PropertyName = "asian")]
        public bool Asian { get; set; }

        [JsonProperty(PropertyName = "asianRaceDesignationTypes")]
        public string[] AsianRaceDesignationTypes { get; set; }

        [JsonProperty(PropertyName = "asianRaceOtherDetail")]
        public string AsianRaceOtherDetail { get; set; }

        [JsonProperty(PropertyName = "africanAmerican")]
        public bool AfricanAmerican { get; set; }

        [JsonProperty(PropertyName = "pacificIslander")]
        public bool PacificIslander { get; set; }

        [JsonProperty(PropertyName = "pacificIslanderRaceDesignationTypes")]
        public string[] PacificIslanderRaceDesignationTypes { get; set; }

        [JsonProperty(PropertyName = "pacificIslanderRaceOtherDetail")]
        public string PacificIslanderRaceOtherDetail { get; set; }

        [JsonProperty(PropertyName = "white")]
        public bool White { get; set; }
    }
}
