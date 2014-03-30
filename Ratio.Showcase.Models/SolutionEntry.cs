using System.Collections.Generic;
using Contentful.SDK;
using Contentful.SDK.ContentModel;

namespace Ratio.Showcase.Models
{
    public class SolutionEntry : Entry
    {
        public new SolutionFields Fields { get; set; }

        public SolutionEntry(IEntry entry)
        {
            this.Sys = entry.Sys;
            this.Fields = entry.Fields.ToObject<SolutionFields>();
        }

        public override TEntry From<TEntry>(Entry entry)
        {
            return new SolutionEntry(entry) as TEntry;
        }

        public class SolutionFields
        {
            public string Title { get; set; }
            public string Description { get; set; }
            [LinkedContentArray(typeof(TagEntry), LinkType.Entry)]
            public IEnumerable<TagEntry> Tags { get; set; }
            [LinkedContentArray(typeof(Asset), LinkType.Asset)]
            public IEnumerable<Asset> Images { get; set; }
        }
    }
}