namespace OneMSQFT.Common.Models
{
    public class MediaContentSource : IMediaContentSource
    {
        public string Id { get; set; }
        public string ParentId { get; set; }
        public string Source { get; set; }
        public ContentSourceType ContentSourceType { get; set; }
    }

    public enum ContentSourceType
    {
        Image = 1,
        Video = 2
    }
}