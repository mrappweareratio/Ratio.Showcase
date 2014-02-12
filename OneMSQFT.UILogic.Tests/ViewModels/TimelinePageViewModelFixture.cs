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
using OneMSQFT.Common.DataLayer;
using OneMSQFT.Common.Models;
using OneMSQFT.UILogic.DataLayer;
using OneMSQFT.UILogic.Interfaces.ViewModels;
using OneMSQFT.UILogic.Services;
using OneMSQFT.UILogic.Tests.Mocks;
using OneMSQFT.UILogic.ViewModels;

namespace OneMSQFT.UILogic.Tests.ViewModels
{
    [TestClass]
    public class TimelinePageViewModelFixture
    {
        public IAsyncAction ExecuteOnUIThread(DispatchedHandler action)
        {
            return Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, action);
        }

        [TestMethod]
        public void TimelinePageViewModel_Implements_Interface()
        {
            var vm = new TimelinePageViewModel(new MockDataService(), new MockAlertMessageService(), new MockNavigationService()) as ITimelinePageViewModel;
            Assert.IsNotNull(vm);
        }

        [TestMethod]
        public void TimelinePageViewModel_Constructs()
        {
            var vm = new TimelinePageViewModel(new MockDataService(), new MockAlertMessageService(), new MockNavigationService()) as ITimelinePageViewModel;
            Assert.IsNotNull(vm.SquareFootEvents, "SquareFootEvents");
            Assert.IsNotNull(vm.TimeLineItems, "TimeLineItems");
            Assert.IsNotNull(vm.TimeLineMenuItems, "TimeLineMenuItems");
            Assert.IsNotNull(vm.EventHeroItemClickCommand, "EventHeroItemClickCommand");
        }       

        [TestMethod]
        public void TimelinePageViewModel_NavigatededTo_Calls_DataService()
        {
            var autoResetEvent = new AutoResetEvent(false);
            bool called = false;
            var mockDataService = new MockDataService
            {
                GetEventsDelegate = async () =>
                {
                    called = true;
                    autoResetEvent.Set();
                    return await Task.FromResult<IEnumerable<Event>>(new List<Event>());
                }
            };
            var timeLine = new TimelinePageViewModel(mockDataService, new MockAlertMessageService(), new MockNavigationService());
            ExecuteOnUIThread(() => timeLine.OnNavigatedTo(null, NavigationMode.New, null));
            autoResetEvent.WaitOne(500);
            Assert.IsTrue(called);
        }

        [TestMethod]
        async public Task TimelinePageViewModel_Alerts_On_Null_Events()
        {
            bool called = false;
            bool alerted = false;
            var autoResetEvent = new AutoResetEvent(false);
            var mockDataService = new MockDataService
            {
                GetEventsDelegate = () =>
                {
                    called = true;
                    return Task.FromResult<IEnumerable<Event>>(null);
                }
            };
            var mockAlerts = new MockAlertMessageService()
            {
                ShowAsyncDelegate = (s, s1) =>
                {
                    alerted = true;
                    autoResetEvent.Set();
                    return Task.FromResult(0);
                }
            };
            var timeLine = new TimelinePageViewModel(mockDataService, mockAlerts, new MockNavigationService());
            ExecuteOnUIThread(() => timeLine.OnNavigatedTo(null, NavigationMode.New, null));
            autoResetEvent.WaitOne(500);
            Assert.IsTrue(called);
            Assert.IsTrue(alerted);
        }


        [TestMethod]
        public void TimelinePageViewModel_Loading_Sample_Populates_Events()
        {
            var internetConnection = new MockInternetConnectionService
            {
                IsConnectedDelegate = () => true
            };
            var timeLine = new TimelinePageViewModel(new DataService(new SampleDataRepository(), new MockDataCacheService() { ContainsDataDelegate = s => Task.FromResult(false) }, internetConnection), new MockAlertMessageService(), new MockNavigationService());
            var autoResetEvent = new AutoResetEvent(false);
            ExecuteOnUIThread(() => timeLine.OnNavigatedTo(null, NavigationMode.New, null));
            autoResetEvent.WaitOne(500);
            Assert.IsTrue(timeLine.SquareFootEvents.Any());
            Assert.IsTrue(timeLine.TimeLineItems.Any());
            Assert.IsTrue(timeLine.TimeLineMenuItems.Any());
        }
    }
}
