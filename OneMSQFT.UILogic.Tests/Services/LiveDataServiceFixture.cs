using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Ratio.Showcase.Shared.Models;
using Ratio.Showcase.UILogic.DataLayer;
using Ratio.Showcase.UILogic.Services;
using Ratio.Showcase.UILogic.Tests.DataLayer;
using Ratio.Showcase.UILogic.Tests.Mocks;

namespace Ratio.Showcase.UILogic.Tests.Services
{
    [TestClass]
    public class LiveDataServiceFixture
    {
        [TestInitialize]
        async public Task Init()
        {
            this.Repository = new ApiDataRepository(new ApiConfiguration());
            this.Cache = new DataCacheService();
            this.Internet = new MockInternetConnectionService(true);
            this.DataService = new DataService(Repository, Cache, Internet);                
            await Cache.InvalidateDataAsync("site_data");
        }

        public MockInternetConnectionService Internet { get; set; }
        public DataService DataService { get; set; }
        public DataCacheService Cache { get; set; }
        public ApiDataRepository Repository { get; set; }

        [TestMethod]
        public async Task DataService_Api_Valid_Cache_Valid()
        {
            var events = await DataService.GetEvents(new CancellationToken());
            var eventsList = events.ToList();
            Assert.IsTrue(eventsList.TrueForAll(DataRepositoryFixture.ValidateEvent), "ValidateEvent IsTrue");
            Assert.IsTrue(eventsList.SelectMany(x => x.Exhibits).ToList().TrueForAll(DataRepositoryFixture.ValidateExhibit), "ValidateExhibit IsTrue");
            
            //wait for background cache
            await Task.Delay(1000);
            var cached = await Cache.ContainsDataAsync("site_data", false);            
            Assert.IsTrue(cached, "Cached after 1000");

            DataService = new DataService(Repository, Cache, Internet);
            events = await DataService.GetEvents(new CancellationToken());
            eventsList = events.ToList();
            Assert.IsTrue(eventsList.TrueForAll(DataRepositoryFixture.ValidateEvent), "Cached ValidateEvent IsTrue");
            Assert.IsTrue(eventsList.SelectMany(x => x.Exhibits).ToList().TrueForAll(DataRepositoryFixture.ValidateExhibit), "Cached ValidateExhibit IsTrue");            
        }


        [TestMethod]
        public async Task Api_Valid_Cache_Valid()
        {
            var events = await DataService.GetEvents(new CancellationToken());
            var eventsList = events.ToList();
            Assert.IsTrue(eventsList.TrueForAll(DataRepositoryFixture.ValidateEvent), "ValidateEvent IsTrue");
            Assert.IsTrue(eventsList.SelectMany(x => x.Exhibits).ToList().TrueForAll(DataRepositoryFixture.ValidateExhibit), "ValidateExhibit IsTrue");
            
            //wait for background cache
            await Task.Delay(1000);
            var cached = await Cache.ContainsDataAsync("site_data", false);
            Assert.IsTrue(cached, "Cached after 1000");
            
            var cachedSiteData = await Cache.GetDataAsync<SiteData>("site_data");
            Assert.IsNotNull(cachedSiteData, "cachedSiteData");
            eventsList = cachedSiteData.Events.ToList();
            Assert.IsTrue(eventsList.TrueForAll(DataRepositoryFixture.ValidateEvent), "Cached ValidateEvent IsTrue");
            Assert.IsTrue(eventsList.SelectMany(x => x.Exhibits).ToList().TrueForAll(DataRepositoryFixture.ValidateExhibit), "Cached ValidateExhibit IsTrue");
        }        
    }
}
