using System.Collections.Generic;

namespace OneMSQFT.Common.Models
{
    public class Curator : ICurator<Exhibit>
    {        
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int SquareFootage { get; set; }
        public string SocialMediaTitle { get; set; }
        public string SocialMediaDescription { get; set; }
        public string SocialMediaImagePath { get; set; }
        public string SeoMetaDescription { get; set; }
        public string Color { get; set; }
        public string LogoImageName { get; set; }
        public string ExternalUrl { get; set; }
        public IEnumerable<Exhibit> Exhibits { get; set; }
        public string EventId { get; set; }
    }
}