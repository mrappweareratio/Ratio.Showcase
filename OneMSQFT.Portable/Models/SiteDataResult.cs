using System.Collections.Generic;
using Newtonsoft.Json;

namespace OneMSQFT.Common.Models
{
    public class SiteDataResult
    {
        [JsonProperty("events")]
        public IEnumerable<Event> Events { get; set; }
        [JsonProperty("tag_data")]
        public IEnumerable<Tag> Tags { get; set; }
    }
}