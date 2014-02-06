using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using OneMSQFT.Common.Models;
using OneMSQFT.UILogic.DataLayer;

namespace OneMSQFT.UILogic.Tests.DataLayer
{
    [TestClass]
    public class DataRepositoryTests
    {
        [TestMethod]
        public async Task MockDataRepository_TimlineResult_Not_Null()
        {
            var dataRepo = new MockDataRepository(new MockLocalStorageProvider());
            var result = await dataRepo.LoadAllData();
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task MockDataRepository_TimlineResult_Events_Not_Null()
        {
            var dataRepo = new MockDataRepository(new MockLocalStorageProvider());
            var result = await dataRepo.LoadAllData();
            Assert.IsNotNull(result.Events);
        }

        [TestMethod]
        public async Task MockDataRepository_TimlineResult_Events_Has_One()
        {
            var dataRepo = new MockDataRepository(new MockLocalStorageProvider());
            var result = await dataRepo.LoadAllData();
            var e = result.Events.FirstOrDefault();
            Assert.IsNotNull(e);
        }

        [TestMethod]
        public async Task MockDataRepository_TimlineResult_IsEvent()
        {
            var dataRepo = new MockDataRepository(new MockLocalStorageProvider());
            var result = await dataRepo.LoadAllData();
            var e = result.Events.FirstOrDefault();
            Assert.IsTrue(e is Event);
        }
    }
}
