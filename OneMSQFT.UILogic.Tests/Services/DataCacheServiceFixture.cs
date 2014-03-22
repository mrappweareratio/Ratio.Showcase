using System;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Ratio.Showcase.Shared.DataLayer;
using Ratio.Showcase.Shared.Models;
using Ratio.Showcase.Shared.Services;
using Ratio.Showcase.UILogic.DataLayer;
using Ratio.Showcase.UILogic.Services;

namespace Ratio.Showcase.UILogic.Tests.Services
{
    [TestClass]
    public class DataCacheServiceFixture
    {
        [TestInitialize]
        public async Task Init()
        {
            this.DataCache = new DataCacheService();
            this.DataRepository = new DemoDataRepository();
            await DataCache.InvalidateDataAsync(dataKey);
        }

        public IDataCacheService DataCache { get; set; }
        public IDataRepository DataRepository { get; set; }
        private const string dataKey = "site_data";

        
        [TestMethod]
        async public Task DataCacheService_IsNotNull()
        {
            Assert.IsNotNull(DataCache);
        }

        [TestMethod]
        async public Task DataCacheService_ContainsDataAsync_IsFalse()
        {
            var result = await DataCache.ContainsDataAsync(dataKey, true);
            Assert.IsFalse(result);
        }

        [TestMethod]
        async public Task DataCacheService_GetDataAsync_HasNoData()
        {
            var result = await DataCache.GetDataAsync<SiteDataResult>(dataKey);
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task DataCacheService_StoreDataAsync()
        {
            //Load data to be saved
            var data = DataRepository.GetSiteData(new CancellationToken());

            if (data != null)
            {
                await DataCache.StoreDataAsync(dataKey, data);
            }

            bool result = false;
            try
            {
                var cacheFolder = ApplicationData.Current.TemporaryFolder;
                StorageFile file = await cacheFolder.GetFileAsync(dataKey);
                result = true;
            }
            catch (Exception)
            {
            }

            Assert.IsTrue(result);
        }

        //[TestMethod]
        //async public Task DataCacheService_GetDataAsync_HasValidData()
        //{
        //    var result = await DataCache.ContainsDataAsync(dataKey, false);
        //    Assert.IsFalse(result);
        //}
    }
}
