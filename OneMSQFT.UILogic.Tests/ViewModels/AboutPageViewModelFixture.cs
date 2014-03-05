using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Xaml.Navigation;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using OneMSQFT.Common.Analytics;
using OneMSQFT.Common.Models;
using OneMSQFT.Common.Services;
using OneMSQFT.UILogic.Analytics;
using OneMSQFT.UILogic.Interfaces.ViewModels;
using OneMSQFT.UILogic.Tests.Mocks;
using OneMSQFT.UILogic.ViewModels;

namespace OneMSQFT.UILogic.Tests.ViewModels
{
    [TestClass]
    public class AboutPageViewModelFixture
    {
        public IAsyncAction ExecuteOnUIThread(DispatchedHandler action)
        {
            return Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, action);
        }

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

        [TestMethod]
        public void ViewModel_Implements_Interface()
        {
            var vm = new AboutPageViewModel(new MockDataService(), new MockAlertMessageService(), new MockAnalyticsService()) as IAboutPageViewModel;
            Assert.IsNotNull(vm);
        }

        [TestMethod]
        public void ViewModel_NavigatedTo_Calls_DataService_GetEvents()
        {
            var autoResetEvent = new AutoResetEvent(false);
            bool called = false;
            var mockDataService = new MockDataService
            {
                  GetEventsDelegate = async () =>
                  {
                      called = true;
                      return new List<Event>();
                  }
            };

            var vm = new AboutPageViewModel(mockDataService, new MockAlertMessageService(), new MockAnalyticsService());
            ExecuteOnUIThread(()=> vm.OnNavigatedTo("0", NavigationMode.New, null));
            autoResetEvent.WaitOne(200);
            Assert.IsTrue(called);
            Assert.IsNotNull(vm.SquareFootEvents);
        }

        #region Resizing

        [TestMethod]
        public void IsHorizontal_Updates_On_Window_Size_Changed()
        {
            var autoResetEvent = new AutoResetEvent(false);
            bool changed = false;

            var vm = new AboutPageViewModel(new MockDataService(), new MockAlertMessageService(), new MockAnalyticsService());
            vm.PropertyChanged += (sender, args) =>
            {
                changed = true;
            };
            ExecuteOnUIThread(() => vm.WindowSizeChanged(1000, 500));
            autoResetEvent.WaitOne(500);
            Assert.IsTrue(changed, "PropertyChanged");
            Assert.IsTrue(vm.IsHorizontal);
            ExecuteOnUIThread(() => vm.WindowSizeChanged(500, 1000));
            autoResetEvent.WaitOne(500);
            Assert.IsTrue(changed, "PropertyChanged");
            Assert.IsFalse(vm.IsHorizontal);
        }

        [TestMethod]
        public void Local_Properties_Update_On_Window_Size_Changed()
        {
            var autoResetEvent = new AutoResetEvent(false);
            bool changed = false;
            var changedProperties = new List<string>();

            var vm = new AboutPageViewModel(new MockDataService(), new MockAlertMessageService(), new MockAnalyticsService());
            vm.PropertyChanged += (sender, args) =>
            {
                changed = true;
                changedProperties.Add(args.PropertyName);
            };
            ExecuteOnUIThread(() => vm.WindowSizeChanged(1000, 500));
            autoResetEvent.WaitOne(500);
            Assert.IsTrue(changed, "PropertyChanged");
            Assert.IsTrue(changedProperties.Contains("AboutPageTotalWidth"));
            Assert.IsTrue(changedProperties.Contains("AboutPageTwoThirdsWidth"));
            Assert.IsTrue(changedProperties.Contains("AboutPageTotalHeight"));
            Assert.IsTrue(changedProperties.Contains("AboutPageTwoThirdsHeight"));
            Assert.IsTrue(changedProperties.Contains("Panel1TextWidth"));
            Assert.IsTrue(changedProperties.Contains("Panel2TextWidth"));
            Assert.IsTrue(changedProperties.Contains("Panel1Margin"));
            Assert.IsTrue(changedProperties.Contains("Panel2Margin"));           
        }
        [TestMethod]
        public void Base_Properties_Update_On_Window_Size_Changed()
        {
            var autoResetEvent = new AutoResetEvent(false);
            bool changed = false;
            var changedProperties = new List<string>();

            var vm = new AboutPageViewModel(new MockDataService(), new MockAlertMessageService(), new MockAnalyticsService());
            vm.PropertyChanged += (sender, args) =>
            {
                changed = true;
                changedProperties.Add(args.PropertyName);
            };
            ExecuteOnUIThread(() => vm.WindowSizeChanged(1000, 500));
            autoResetEvent.WaitOne(500);
            Assert.IsTrue(changed, "PropertyChanged");
            Assert.IsTrue(changedProperties.Contains("FullScreenWidth"));
            Assert.IsTrue(changedProperties.Contains("FullScreenHeight"));
            Assert.IsTrue(changedProperties.Contains("IsHorizontal"));
            Assert.IsTrue(changedProperties.Contains("LargeFlexyFontSize"));
            Assert.IsTrue(changedProperties.Contains("MediumLargeFlexyFontSize"));
            Assert.IsTrue(changedProperties.Contains("MediumLargeFlexyTightLeading"));
            Assert.IsTrue(changedProperties.Contains("MediumFlexyFontSize"));
            Assert.IsTrue(changedProperties.Contains("MediumSmallFlexyFontSize"));
            Assert.IsTrue(changedProperties.Contains("SmallFlexyFontSize"));
        }



        #endregion
    }
}
