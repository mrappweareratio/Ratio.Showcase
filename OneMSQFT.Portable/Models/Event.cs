using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;
using Newtonsoft.Json;

namespace OneMSQFT.Common.Models
{
    public class Event : IEvent<Exhibit>
    {
        public Event()
        {
            Exhibits = new List<Exhibit>();
        }

        public string Id { get; set; }
        [JsonProperty("meta_title")]
        public string SocialMediaTitle { get; set; }
        [JsonProperty("meta_desc")]
        public string SocialMediaDescription { get; set; }
        [JsonProperty("meta_img")]
        public string SocialMediaImagePath { get; set; }
        [JsonProperty("sqft")]
        public int SquareFootage { get; set; }
        [JsonProperty("title")]
        public string Name { get; set; }
        public string Description { get; set; }
        public string Longitude { get; set; }
        public string Lattitude { get; set; }
        [JsonProperty("display_date")]
        public string DisplayDate { get; set; }
        [JsonProperty("start_date")]
        public DateTime DateStart { get; set; }
        [JsonProperty("end_date")]
        public DateTime DateEnd { get; set; }
        [JsonProperty("exhibit_data")]
        public IEnumerable<Exhibit> Exhibits { get; set; }
        public string Color { get; set; }
        public IEnumerable<MediaContentSource> MediaContent { get; set; }
        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }
        [JsonProperty("updated_at")]
        public DateTime UpdatedAt { get; set; }
    }
}
