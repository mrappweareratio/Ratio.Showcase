using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        [TestMethod]
        public void TimelinePageViewModel_Implements_Interface()
        {
            var vm = new TimelinePageViewModel(new MockDataService(), new MockAlertMessageService()) as ITimelinePageViewModel;
            Assert.IsNotNull(vm);
        }

        [TestMethod]
        public void TimelinePageViewModel_Constructs()
        {
            var vm = new TimelinePageViewModel(new MockDataService(), new MockAlertMessageService()) as ITimelinePageViewModel;
            Assert.IsNotNull(vm.SquareFootEvents, "SquareFootEvents");
            Assert.IsNotNull(vm.TimeLineItems, "TimeLineItems");
            Assert.IsNotNull(vm.TimeLineMenuItems, "TimeLineMenuItems");
            Assert.IsNotNull(vm.EventHeroItemClickCommand, "EventHeroItemClickCommand");
        }

        [TestMethod]
        public void WindowSizeChanged_Sets_Zoom_Heights()
        {
            double windowHeight = 768;
            double windowWidth = 1366;
            var vm = new TimelinePageViewModel(new MockDataService(), new MockAlertMessageService()) as ITimelinePageViewModel;
            vm.WindowSizeChanged(windowWidth, windowHeight);
            Assert.AreEqual(windowHeight / 4, vm.ZoomedOutItemHeight);
            Assert.AreEqual(windowWidth / 6, vm.ZoomedOutItemWidth);
            Assert.AreEqual(windowHeight, vm.EventItemHeight);
            Assert.IsTrue(vm.EventItemWidth < windowWidth, "EventItemWidth < Window");
        }

        [TestMethod]
        async public Task TimelinePageViewModel_NavigatededTo_Calls_DataService()
        {
            bool called = false;
            var mockDataService = new MockDataService
            {
                GetEventsDelegate = () =>
                {
                    called = true;
                    return Task.FromResult<IEnumerable<Event>>(new List<Event>());
                }
            };
            var timeLine = new TimelinePageViewModel(mockDataService, new MockAlertMessageService());
            timeLine.OnNavigatedTo(null, NavigationMode.New, null);
            await timeLine.LoadingTaskCompletionSource.Task;
            Assert.IsTrue(called);
        }

        [TestMethod]
        async public Task TimelinePageViewModel_Alerts_On_Null_Events()
        {
            bool called = false;
            bool alerted = false;
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
                    return Task.FromResult(0);
                }
            };
            var timeLine = new TimelinePageViewModel(mockDataService, mockAlerts);          
            timeLine.OnNavigatedTo(null, NavigationMode.New,null);
            await timeLine.LoadingTaskCompletionSource.Task;
            Assert.IsTrue(called);
            Assert.IsTrue(alerted);
        }


        [TestMethod]
        async public Task TimelinePageViewModel_Loading_Sample_Populates_Events()
        {
            var timeLine = new TimelinePageViewModel(new DataService(new SampleDataRepository()), new MockAlertMessageService());
            timeLine.OnNavigatedTo(null, NavigationMode.New, null);
            await timeLine.LoadingTaskCompletionSource.Task;
            Assert.IsTrue(timeLine.SquareFootEvents.Any()); 
            Assert.IsTrue(timeLine.TimeLineItems.Any());
            Assert.IsTrue(timeLine.TimeLineMenuItems.Any());

        }
    }
}
