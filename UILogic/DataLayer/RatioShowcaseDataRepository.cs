using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Contentful.SDK;
using Contentful.SDK.ContentModel;
using Contentful.SDK.Search;
using Ratio.Showcase.Models;
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
                new ContentTypeSearchFilter(PlatformEntry.ContentTypeId),
                new IncludeLinksSearchOption(2)//include Platforms.Fields.Solutions.Fields.Images
            });
            return platforms;
        }

        private async Task<ContentfulClient> EnsureClientAsync()
        {
            if (_client == null)
            {
                _client = new ContentfulClient();
                await _client.CreateAsync("665ovgw8r0b9", "7a353660e71adb5e121872e753ce4dcf3ea614b0e1bd9aad845f48dedfb3feb1");
            }
            return _client;
        }

        async public Task<SiteData> GetSiteData(CancellationToken token)
        {
            var sd = new SiteData();
            var platforms = await GetPlatformsAsync();
            sd.Events = GenerateEvents(platforms.Items);
            return sd;
        }

        private IEnumerable<Event> GenerateEvents(IEnumerable<PlatformEntry> platformEntries)
        {
            return platformEntries.Select(platform => new Event()
            {
                Color = "EF4A39",
                Name = platform.Fields.Title,
                Id = GenerateId(platform.Sys.Id),
                Description = platform.Fields.Description,
                MediaContent = GenerateMediaContent(platform.Fields.Images),
                Exhibits = GenerateExhibits(platform.Fields.Solutions)
            });
        }

        private IEnumerable<Exhibit> GenerateExhibits(IEnumerable<SolutionEntry> solutions)
        {
            return solutions.Select(x => new Exhibit()
            {
                Id = GenerateId(x.Sys.Id),
                Color = "EF4A39",
                DateEnd = x.Sys.UpdatedAt,
                DateStart = x.Sys.CreatedAt,
                Name = x.Fields.Title,
                Description = x.Fields.Description,
                ThumbImage = x.Fields.Images.Any() ? x.Fields.Images.First().GetImageUrl(new Asset.ImageOptions(){Width = 683}) : String.Empty,
                MediaContent = GenerateMediaContent(x.Fields.Images)
            });
        }

        private static string GenerateId(string id)
        {
            return Regex.Replace(id, @"^[a-zA-Z][a-zA-Z0-9]", String.Empty);
        }

        private IEnumerable<MediaContentSource> GenerateMediaContent(IEnumerable<Asset> images)
        {
            return images.Select(x => new MediaContentSource()
            {
                ContentSourceType = ContentSourceType.Image,
                Id = x.Sys.Id,
                Img = x.GetImageUrl(new Asset.ImageOptions(){Width = 1366})
            });
        }
    }
}
