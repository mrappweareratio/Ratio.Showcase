using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using OneMSQFT.Common.DataLayer;
using OneMSQFT.Common.Models;
using OneMSQFT.UILogic.DataLayer;

namespace OneMSQFT.UILogic.Tests.DataLayer
{
    [TestClass]
    public class SampleDataRepositoryFixture
    {
        [TestMethod]
        async public Task MockDataRepository_TimlineResult_Not_Null()
        {
            var dataRepo = new SampleDataRepository();
            var result = await dataRepo.LoadAllData();
            Assert.IsNotNull(result);
        }

        [TestMethod]
        async public Task MockDataRepository_TimlineResult_Events_Not_Null()
        {
            var dataRepo = new SampleDataRepository();
            var result = await dataRepo.LoadAllData();
            Assert.IsNotNull(result.Events);
        }

        [TestMethod]
        async public Task MockDataRepository_TimlineResult_Events_Has_One()
        {
            var dataRepo = new SampleDataRepository();
            var result = await dataRepo.LoadAllData();
            var e = result.Events.FirstOrDefault();
            Assert.IsNotNull(e);
        }

        [TestMethod]
        async public Task MockDataRepository_TimlineResult_Events_Are_Valid()
        {
            var dataRepo = new SampleDataRepository();
            var result = await dataRepo.LoadAllData();
            var validEvents = result.Events.ToList().TrueForAll(ValidateEvent);            
            Assert.IsTrue(validEvents, "ValidEvents");
        }

        public bool ValidateEvent(Event e)
        {
            Assert.IsNotNull(e.Name, "Name");
            Assert.IsNotNull(e.Color, "Color");
            Assert.IsTrue(e.Color.Length == 6, "Color is Hex");
            return true;
        }
    }
}
