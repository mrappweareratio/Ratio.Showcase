using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using OneMSQFT.Common.Analytics;
using OneMSQFT.Common.Services;
using OneMSQFT.UILogic.Analytics;
using OneMSQFT.UILogic.Tests.Mocks;
using OneMSQFT.UILogic.ViewModels;

namespace OneMSQFT.UILogic.Tests.ViewModels
{
    [TestClass]
    public class AboutPageViewModelFixture
    {
        #region Analytics

        [TestMethod]
        public void PageView()
        {
            bool tracked = false;
            IDataService dataService = new MockDataService();
            IAlertMessageService messageService = new MockAlertMessageService();
            IAnalyticsService analyticsService = new MockAnalyticsService()
            {
                TrackEventsDelegate = (context, data) =>
                {
                    tracked = true;
                    Assert.IsTrue(context.Contains(TrackingEventsData.Events.PageView), "PageView");
                    Assert.IsNotNull(data.PageName);
                    Assert.AreEqual(data.PageName, TrackingContextData.PageNames.About, "About");
                }
            };
            var vm = new AboutPageViewModel(dataService, messageService, analyticsService);
            vm.OnNavigatedTo(null, NavigationMode.New, null);
            Assert.IsTrue(tracked, "tracked");
        }

        #endregion
    }
}
