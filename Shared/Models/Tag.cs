using Newtonsoft.Json;

namespace Ratio.Showcase.Shared.Models
{
    public class Tag
    {
        [JsonProperty("id")]
        public int TagId { get; set; }
        [JsonProperty("name")]
        public string TagName { get; set; }
    }
}