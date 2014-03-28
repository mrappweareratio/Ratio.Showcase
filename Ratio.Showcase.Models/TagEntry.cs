using Contentful.SDK.ContentModel;

namespace Ratio.Showcase.Models
{
    public class TagEntry : Entry
    {
        public string Name { get { return GetField<string>("name"); } }
    }
}
