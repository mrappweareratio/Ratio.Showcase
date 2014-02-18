using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using OneMSQFT.Common.DataLayer;
using OneMSQFT.UILogic.DataLayer;
using OneMSQFT.UILogic.Tests.Mocks;

namespace OneMSQFT.UILogic.Tests.DataLayer
{
    [TestClass]
    public class ApiDataRepositoryFixture
    {
        [TestInitialize]
        public void Init()
        {            
            this.ApiConfiguration = new MockApiConfiguration("http://1msqft-stage.azurewebsites.net/api");            
        }

        public IApiConfiguration ApiConfiguration { get; set; }

        [TestMethod]
        async public Task Loads_Site_Data()
        {
            var apiDataRepository = new ApiDataRepository(ApiConfiguration);
            var result = await apiDataRepository.GetSiteData();
            Assert.IsNotNull(result, "Result");
        }
    }
}
