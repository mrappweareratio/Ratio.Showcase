using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;

namespace OneMSQFT.Common.Models
{
    public class Event : IEvent
    {
        public string Id { get; set; }
        public string SocialMediaTitle { get; set; }
        public string SocialMediaDescription { get; set; }
        public string SocialMediaImagePath { get; set; }
        public int SquareFootage { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Geolocation { get; set; }
        public string DateRange { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public Boolean IsInTheFuture { get; set; }
        public string SeoMetaDescription { get; set; }
        public string PhotoFilePath { get; set; }
        public List<Curator> Curators { get; set; }
        public string Color { get; set; }
    }
}
