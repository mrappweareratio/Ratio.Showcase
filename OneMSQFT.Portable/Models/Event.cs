using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Ratio.Showcase.Shared.Models
{
    public class Event : IEvent<Exhibit>
    {
        public Event()
        {
            Exhibits = new List<Exhibit>();
            MediaContent = new List<MediaContentSource>();
        }

        public string Id { get; set; }
        [JsonProperty("meta_title")]
        public string SocialMediaTitle { get; set; }
        [JsonProperty("meta_desc")]
        public string SocialMediaDescription { get; set; }
        [JsonProperty("meta_img")]
        public string SocialMediaImagePath { get; set; } //Not currently used but may be needed in the future
        [JsonProperty("sqft")]
        public int SquareFootage { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public string Longitude { get; set; }
        public string Lattitude { get; set; }
        [JsonProperty("display_date")]
        public string DisplayDate { get; set; }
        [JsonProperty("start_date")]
        public DateTime? DateStart { get; set; }
        [JsonProperty("end_date")]
        public DateTime? DateEnd { get; set; }
        [JsonProperty("exhibit_data")]
        public IEnumerable<Exhibit> Exhibits { get; set; }
        public string Color { get; set; }
        [JsonProperty("theme_color")]
        public string ThemeId { get; set; }
        [JsonProperty("content_data")]
        public IEnumerable<MediaContentSource> MediaContent { get; set; }
        [JsonProperty("created_at")]
        public DateTime? CreatedAt { get; set; }
        [JsonProperty("updated_at")]
        public DateTime? UpdatedAt { get; set; }
    }
}
