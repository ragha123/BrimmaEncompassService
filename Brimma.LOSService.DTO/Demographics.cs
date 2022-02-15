using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Brimma.LOSService.DTO
{
    public class Demographics
    {
        [JsonProperty(PropertyName = "ethnicity")]
        public Ethnicity Ethnicity { get; set; }

        [JsonProperty(PropertyName = "race")]
        public Race Race { get; set; }

        [JsonProperty(PropertyName = "sex")]
        public Sex Sex { get; set; }
    }
}
