using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace OneMSQFT.Common
{
    public class JsonHelper
    {
        public static TObject DeserializeObject<TObject>(string json) where TObject : class
        {
            return JsonConvert.DeserializeObject<TObject>(json);
        }
    }
}
