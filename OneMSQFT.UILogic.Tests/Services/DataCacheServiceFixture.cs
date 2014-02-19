using System;
using System.Threading.Tasks;
using Windows.Storage;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using OneMSQFT.Common.DataLayer;
using OneMSQFT.Common.Models;
using OneMSQFT.Common.Services;
using OneMSQFT.UILogic.DataLayer;
using OneMSQFT.UILogic.Services;

namespace OneMSQFT.UILogic.Tests.Services
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
            var data = DataRepository.GetSiteData();

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
