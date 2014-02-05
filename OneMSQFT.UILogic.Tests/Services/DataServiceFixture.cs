using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using OneMSQFT.Common.Models;
using OneMSQFT.UILogic.Services;
using OneMSQFT.UILogic.Tests.Mocks;

namespace OneMSQFT.UILogic.Tests.Services
{
    [TestClass]
    public class DataServiceFixture
    {
        [TestMethod]
        async public Task DataService_GetEvents_Calls_Repository()
        {
            bool called = false;
            var mock = new MockDataRepository
            {
                LoadAllDataDelegate = () =>
                {
                    called = true;
                    return Task.FromResult(new TimelineResult());
                }
            };
            var dataService = new DataService(mock);
            await dataService.GetEvents();
            Assert.IsTrue(called);
        }
    }
}
