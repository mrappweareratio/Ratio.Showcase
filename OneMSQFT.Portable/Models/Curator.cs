using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace OneMSQFT.Common.Models
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