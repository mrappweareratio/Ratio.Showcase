using System.Collections.Generic;

namespace OneMSQFT.Common.Models
{
    public class Exhibit : IExhibit
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Color { get; set; }
        public int SquareFootage { get; set; }
        public List<Tag> Tags { get; set; }
        public string Exhibitor { get; set; }
        public string Introduction { get; set; }
        public string RsvpUrl { get; set; }
        public string FacebookEventUrl { get; set; }        
        public IEnumerable<MediaContentSource> MediaContent { get; set; }
        public string HeroPhotoFilePath { get; set; }
        public string SubHeroPhotoFilePath { get; set; }
        public string ArtistName { get; set; }
    }
}