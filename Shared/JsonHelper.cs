using Newtonsoft.Json;

namespace Ratio.Showcase.Shared
{
    public class JsonHelper
    {
        public static TObject DeserializeObject<TObject>(string json) where TObject : class
        {
            return JsonConvert.DeserializeObject<TObject>(json);
        }

        public static string SerializeObject<TObject>(TObject obj) where TObject : class
        {
            return JsonConvert.SerializeObject(obj);
        }
    }
}
