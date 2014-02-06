using System;

namespace OneMSQFT.Common.Models
{
    public class ExhibitDetail
    {
        public Exhibit Exhibit { get; set; }
        public String EventId { get; set; }
        public String NextExhibitId { get; set; }
        public String PreviousExhibitId { get; set; }        
    }
}