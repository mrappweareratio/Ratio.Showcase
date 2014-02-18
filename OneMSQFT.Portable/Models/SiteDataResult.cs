using Newtonsoft.Json;

namespace OneMSQFT.Common.Models
{        
    [JsonObject]
    public class SiteDataResult : IBaseResult<SiteData>
    {
        [JsonProperty("site_data")]
        public SiteData Data { get; set; }
    }
}