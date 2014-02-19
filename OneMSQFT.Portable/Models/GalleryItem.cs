using Newtonsoft.Json;

namespace OneMSQFT.Common.Models
{
    public class MediaContentSource : IMediaContentSource
    {
        public string Id { get; set; }
        public string Img { get; set; }
        [JsonProperty("video_id")]
        public string VideoId { get; set; }
        [JsonProperty("video_hd_url")]
        public string VideoUrlHd { get; set; }
        [JsonProperty("video_sd_url")]
        public string VideoUrlSd { get; set; }
        [JsonProperty("video_mobile_url")]
        public string VideoUrlMobile { get; set; }
        [JsonProperty("type")]
        public ContentSourceType ContentSourceType { get; set; }
        [JsonProperty("sort_order")]
        public int SortOrder { get; set; }
    }

    public enum ContentSourceType
    {
        Video = 1,
        Image = 2,
    }
}