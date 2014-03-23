using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Ratio.Showcase.Shared.Models
{
    public class Link: ILink
    {
        public string Id { get; set; }
        [JsonProperty("exhibit_id")]
        public string ExhibitId { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
    }
}
