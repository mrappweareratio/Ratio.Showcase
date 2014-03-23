using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Ratio.Showcase.Shared.DataLayer;
using Ratio.Showcase.Shared.Models;
using Ratio.Showcase.UILogic.DataLayer;
using Ratio.Showcase.UILogic.Tests.Mocks;
using Ratio.Showcase.UILogic.Tests.Utils;

namespace Ratio.Showcase.UILogic.Tests.DataLayer
{
    [TestClass]
    public class ApiDataRepositoryFixture
    {
        private SiteData result;

        [TestInitialize]
        async public Task Init()
        {            
            this.ApiConfiguration = new MockApiConfiguration("http://1msqft-stage.azurewebsites.net/api");
            var apiDataRepository = new ApiDataRepository(ApiConfiguration);
            var token = (new CancellationTokenSource()).Token;
            result = await apiDataRepository.GetSiteData(token);
        }

        public IApiConfiguration ApiConfiguration { get; set; }

        [TestMethod]
        async public Task Loads_Site_Data()
        {          
            Assert.IsNotNull(result, "Result");
            Assert.IsNotNull(result.Events, "Events");
            Assert.IsNotNull(result.Themes, "Themes");
        }

        [TestMethod]
        async public Task Loads_Site_Data_Valid_Events()
        {         
            Assert.IsTrue(result.Events.ToList().TrueForAll(DataRepositoryFixture.ValidateEvent), "ValidateEvent");            
        }

        [TestMethod]
        async public Task Loads_Site_Data_Valid_Exhibits()
        {         
            Assert.IsTrue(result.Events.SelectMany(x => x.Exhibits).ToList().TrueForAll(DataRepositoryFixture.ValidateExhibit), "ValidateExhibit");            
        }

        [TestMethod]
        async public Task Loads_Site_Data_Theme_Ids_Valid()
        {         
            Assert.IsNotNull(result.Themes, "Themes");
            Assert.IsTrue(result.Themes.Any(), "Multiple Themes");
            Assert.IsTrue(result.Themes.ToList().TrueForAll(x => !String.IsNullOrEmpty(x.Color)), "Theme Colors");
            var themeIds = result.Themes.Select(x => x.Id);
            Assert.IsTrue(result.Events.ToList().TrueForAll(x => themeIds.Contains(x.ThemeId)));
        }

        [TestMethod]
        async public Task Loads_Site_Data_Valid_Event_Colors()
        {         
            Assert.IsTrue(result.Events.ToList().TrueForAll(x => !String.IsNullOrEmpty(x.Color)), "Event Colors");            
        }

        [TestMethod]
        async public Task Loads_Site_Data_Valid_Exhibit_Colors()
        {         
            Assert.IsTrue(result.Events.ToList().TrueForAll(x => !String.IsNullOrEmpty(x.Color)), "Event Colors");
            Assert.IsTrue(result.Events.SelectMany(x => x.Exhibits).ToList().TrueForAll(x => !String.IsNullOrEmpty(x.Color)), "Exhibit Colors");            
        }

        [TestMethod]
        async public Task Loads_Site_Data_Valid_Media()
        {
            foreach (var evt in result.Events)
            {
                Assert.IsNotNull(evt.MediaContent, "Event MediaContent IsNotNull");
                MediaContentSourceUtilsFixture.ValidateMediaContent(evt.MediaContent.ToList());
                foreach (var ex in evt.Exhibits)
                {
                    Assert.IsNotNull(evt.MediaContent, "Exhibit MediaContent IsNotNull");
                    MediaContentSourceUtilsFixture.ValidateMediaContent(ex.MediaContent.ToList());
                }
            }
        }
    }
}
