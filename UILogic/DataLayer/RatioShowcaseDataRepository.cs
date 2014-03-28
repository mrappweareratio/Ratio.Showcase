using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Contentful.SDK;
using Contentful.SDK.ContentModel;
using Contentful.SDK.Search;
using Ratio.Showcase.Models;
using Ratio.Showcase.Shared;
using Ratio.Showcase.Shared.DataLayer;
using Ratio.Showcase.Shared.Models;

namespace Ratio.Showcase.UILogic.DataLayer
{
    public class RatioShowcaseDataRepository : IDataRepository
    {
        private ContentfulClient _client;

        async public Task<EntryArray<PlatformEntry>> GetPlatformsAsync()
        {
            await EnsureClientAsync();
            var platforms = await _client.GetEntriesAsync<PlatformEntry>(new List<SearchFilter>
            {
                new ContentTypeSearchFilter(PlatformEntry.ContentTypeId)
            });
            return platforms;
        }

        private async Task<ContentfulClient> EnsureClientAsync()
        {
            if (_client == null) { 
                _client = new ContentfulClient();
                await _client.CreateAsync("665ovgw8r0b9", "7a353660e71adb5e121872e753ce4dcf3ea614b0e1bd9aad845f48dedfb3feb1");
            }
            return _client;
        }

        async public Task<SiteData> GetSiteData(CancellationToken token)
        {
            var client = new ContentfulClient();
            var sd = new SiteData();
            var ev = new List<Event>();
            await client.GetEntriesAsync<PlatformEntry>(new List<SearchFilter>
            {
                new ContentTypeSearchFilter(PlatformEntry.ContentTypeId)
            });
            sd.Events = ev;
            return sd;
        }
    }
}
