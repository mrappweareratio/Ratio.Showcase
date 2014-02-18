using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace OneMSQFT.Common.Models
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
