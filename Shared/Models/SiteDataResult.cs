using Newtonsoft.Json;

namespace Ratio.Showcase.Shared.Models
{        
    [JsonObject]
    public class SiteDataResult : IBaseResult<SiteData>
    {
        [JsonProperty("site_data")]
        public SiteData Data { get; set; }
    }
}