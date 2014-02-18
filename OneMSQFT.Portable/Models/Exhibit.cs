using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace OneMSQFT.Common.Models
{
    public class Exhibit : IExhibit
    {
        public Exhibit()
        {
            Tags = new List<Tag>();
            MediaContent = new List<MediaContentSource>();
        }

        public string Id { get; set; }
        [JsonProperty("title")]
        public string Name { get; set; }
        public string Description { get; set; }
        public string Color { get; set; }
        [JsonProperty("sqft")]
        public int SquareFootage { get; set; }
        public List<Tag> Tags { get; set; }
        public string Exhibitor { get; set; }
        public string Introduction { get; set; }
        [JsonProperty("rsvp_url")]
        public string RsvpUrl { get; set; }
        [JsonProperty("link_data")]
        public IEnumerable<Link> Links { get; set; }
        [JsonProperty("curator_id")]
        public string CuratorId { get; set; } //TODO: This property should probably go away, waiting on final schema from third party.
        [JsonProperty("curator_data")]
        public ICurator Curator { get; set; }
        [JsonProperty("thumb_img")]
        public string ThumbImage { get; set; }
        [JsonProperty("content_data")]
        public IEnumerable<MediaContentSource> MediaContent { get; set; }
        [JsonProperty("display_date")]
        public string DisplayDate { get; set; }
        [JsonProperty("start_date")]
        public DateTime DateStart { get; set; }
        [JsonProperty("end_date")]
        public DateTime DateEnd { get; set; }
        [JsonProperty("meta_title")]
        public string SocialMediaTitle { get; set; }
        [JsonProperty("meta_desc")]
        public string SocialMediaDescription { get; set; }
        public string SocialMediaImagePath { get; set; } //Currently not being used but for future support we might need it.
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}