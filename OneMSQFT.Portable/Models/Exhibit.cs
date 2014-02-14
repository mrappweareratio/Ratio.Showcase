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
        public string RsvpUrl { get; set; }         
        public IEnumerable<MediaContentSource> MediaContent { get; set; }
        [JsonProperty("display_date")]
        public string DisplayDate { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public string SocialMediaTitle { get; set; }
        public string SocialMediaDescription { get; set; }
        public string SocialMediaImagePath { get; set; }
        public string SeoMetaDescription { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}