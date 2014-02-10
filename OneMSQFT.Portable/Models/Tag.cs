using Newtonsoft.Json;

namespace OneMSQFT.Common.Models
{
    public class Tag
    {
        [JsonProperty("id")]
        public int TagId { get; set; }
        [JsonProperty("name")]
        public string TagName { get; set; }
    }
}