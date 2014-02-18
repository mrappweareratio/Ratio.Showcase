using System.Collections.Generic;
using Newtonsoft.Json;

namespace OneMSQFT.Common.Models
{
    public class SiteData
    {
        [JsonProperty("event_data")]
        public IEnumerable<Event> Events { get; set; }
        [JsonProperty("tag_data")]
        public IEnumerable<Tag> Tags { get; set; }   
    }
}