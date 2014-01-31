using System.Collections.Generic;

namespace OneMSQFT.Common.Models
{
    public class Exhibit : IExhibit
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int SquareFootage { get; set; }
        public List<Tag> Tags { get; set; }    
        public string Exhibitor { get; set; }
        public string FacebookEventUrl { get; set; }
        public string PhotoFilePath { get; set; }
        public string VideoFilePath { get; set; }
    }
}