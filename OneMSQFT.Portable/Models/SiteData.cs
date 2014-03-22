using System.Collections.Generic;
using Newtonsoft.Json;

namespace Ratio.Showcase.Shared.Models
{
    public class SiteData
    {
        [JsonProperty("event_data")]
        public IEnumerable<Event> Events { get; set; }
        [JsonProperty("tag_data")]
        public IEnumerable<Tag> Tags { get; set; }
        [JsonProperty("theme_data")]
        public IEnumerable<Theme> Themes { get; set; }   
    }
}