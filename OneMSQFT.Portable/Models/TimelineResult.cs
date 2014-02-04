using System.Collections.Generic;
using Newtonsoft.Json;

namespace OneMSQFT.Common.Models
{
    public class TimelineResult
    {
        [JsonProperty("events")]
        public IEnumerable<Event> Events { get; set; } 
    }
}