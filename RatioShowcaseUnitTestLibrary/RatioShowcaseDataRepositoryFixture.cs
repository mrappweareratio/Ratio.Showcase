using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contentful.SDK;
using Contentful.SDK.Search;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Ratio.Showcase.Models;
using Ratio.Showcase.Shared;
using Ratio.Showcase.UILogic.DataLayer;

namespace RatioShowcaseUnitTestLibrary
{
    [TestClass]
    public class RatioShowcaseDataRepositoryFixture
    {
        private ContentfulClient _client;

        [TestInitialize]
        public async Task Init()
        {
            _client = new ContentfulClient();
            await _client.CreateAsync("665ovgw8r0b9", "7a353660e71adb5e121872e753ce4dcf3ea614b0e1bd9aad845f48dedfb3feb1");
           
        }

        [TestMethod]
        public void CheckSpace()
        {
            Assert.IsNotNull(_client.Space);
        }

        [TestMethod]
        public async Task GetPlatforms()
        {
            var platforms = await _client.GetEntriesAsync<PlatformEntry>(new List<SearchFilter>
            {
                new ContentTypeSearchFilter(PlatformEntry.ContentTypeId),
                new IncludeLinksSearchOption(2)
            });
            Assert.IsNotNull(platforms);
        }

        [TestMethod]
        public async Task GetPlatformsAsync()
        {
            var data = new RatioShowcaseDataRepository();
            var platforms = await data.GetPlatformsAsync();
            Assert.IsNotNull(platforms);
        }

        [TestMethod]
        public async Task GetPlatformsHasSolutions()
        {
            var data = new RatioShowcaseDataRepository();
            var platforms = await data.GetPlatformsAsync();
            Assert.IsNotNull(platforms);
            Assert.IsTrue(platforms.Items.ToList().SelectMany(x => x.Fields.Tags).ToList().TrueForAll(x => x.Name != null), "Tag names");
            Assert.IsTrue(platforms.Items.ToList().TrueForAll(x => x.Fields.Solutions != null));
            Assert.IsTrue(platforms.Items.ToList().SelectMany(x => x.Fields.Solutions).ToList().TrueForAll(x => x.Fields.Title != null), "Tag names");
        }

        [TestMethod]
        public async Task GetPlatformsHasImages()
        {
            var data = new RatioShowcaseDataRepository();
            var platforms = await data.GetPlatformsAsync();
            Assert.IsNotNull(platforms);
            Assert.IsTrue(platforms.Items.ToList().SelectMany(x => x.Fields.Tags).ToList().TrueForAll(x => x.Name != null), "Tag names");
            Assert.IsTrue(platforms.Items.ToList().TrueForAll(x => x.Fields.Images != null), "Images");
            Assert.IsTrue(platforms.Items.ToList().SelectMany(x => x.Fields.Images).ToList().TrueForAll(x => x.Url != null), "Image Urls");
        }


        [TestMethod]
        public async Task GetPlatformsHasSolutionsWithImages()
        {
            var data = new RatioShowcaseDataRepository();
            var platforms = await data.GetPlatformsAsync();
            Assert.IsNotNull(platforms);
            Assert.IsTrue(platforms.Items.ToList().SelectMany(x => x.Fields.Tags).ToList().TrueForAll(x => x.Name != null), "Tag names");
            Assert.IsTrue(platforms.Items.ToList().TrueForAll(x => x.Fields.Solutions != null), "Solutions");
            Assert.IsTrue(platforms.Items.ToList().SelectMany(x => x.Fields.Solutions).ToList().TrueForAll(x => x.Fields.Title != null), "Solution Titles");
            Assert.IsTrue(platforms.Items.ToList().SelectMany(x => x.Fields.Solutions).SelectMany(x => x.Fields.Images).ToList().TrueForAll(x => x.Fields.Title != null), "Solution Titles");
        }
    }
}
