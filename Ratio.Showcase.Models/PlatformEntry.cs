using System.Collections.Generic;
using Contentful.SDK;
using Contentful.SDK.ContentModel;

namespace Ratio.Showcase.Models
{
    public class PlatformEntry : Entry
    {
        public const string ContentTypeId = "17oEYslqWEC6WgQ6gmYY88";

        public new PlatformFields Fields { get; set; }

        public PlatformEntry()
        {
        }

        private PlatformEntry(Entry entry)
        {
            this.Fields.Title = entry.GetField<string>("title");
            this.Fields.Description = entry.GetField<string>("description");
        }

        public override TEntry From<TEntry>(Entry entry)
        {
            return new PlatformEntry(entry) as TEntry;
        }

        public class PlatformFields
        {
            PlatformFields()
            {
                Tags = new List<TagEntry>();
                Solutions = new List<SolutionEntry>();
            }

            public string Title { get; set; }
            public string Description { get; set; }
            [LinkedContentArray(typeof(TagEntry), LinkType.Entry)]
            public IEnumerable<TagEntry> Tags { get; set; }
            [LinkedContentArray(typeof(SolutionEntry), LinkType.Entry)]
            public IEnumerable<SolutionEntry> Solutions { get; set; }
        }
    }
}