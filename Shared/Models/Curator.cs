using Newtonsoft.Json;

namespace Ratio.Showcase.Shared.Models
{
    public class Curator : ICurator
    {        
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [JsonProperty("white_logo_img")]
        public string WhiteLogoImage { get; set; }
        [JsonProperty("logo_img")]
        public string LogoImageName { get; set; }
        [JsonProperty("external_url")]
        public string ExternalUrl { get; set; }
    }
}