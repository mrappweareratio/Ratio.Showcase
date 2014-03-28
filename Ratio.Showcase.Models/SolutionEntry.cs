using System.Collections.Generic;
using Contentful.SDK.ContentModel;

namespace Ratio.Showcase.Models
{
    public class SolutionEntry : Entry
    {
        public new SolutionFields Fields { get; set; }

        public SolutionEntry(Entry entry)
        {
            this.Sys = entry.Sys;
            this.Fields.Title = entry.GetField<string>("title");
            this.Fields.Description = entry.GetField<string>("description");
        }

        public SolutionEntry()
        {
        }

        public override TEntry From<TEntry>(Entry entry)
        {
            return new SolutionEntry(entry) as TEntry;
        }

        public class SolutionFields
        {
            public string Title { get; set; }
            public string Description { get; set; }
            public IEnumerable<TagEntry> Tags { get; set; }
        }
    }
}