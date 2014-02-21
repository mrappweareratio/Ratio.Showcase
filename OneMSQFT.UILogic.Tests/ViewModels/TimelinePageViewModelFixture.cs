using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;
using Microsoft.Practices.Prism.StoreApps.Interfaces;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using OneMSQFT.Common.DataLayer;
using OneMSQFT.Common.Models;
using OneMSQFT.Common.Services;
using OneMSQFT.UILogic.DataLayer;
using OneMSQFT.UILogic.Interfaces.ViewModels;
using OneMSQFT.UILogic.Services;
using OneMSQFT.UILogic.Tests.Mocks;
using OneMSQFT.UILogic.Utils;
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
            var vm = new TimelinePageViewModel(new MockDataService(), new MockAlertMessageService(), new MockNavigationService(), new MockConfigurationService()) as ITimelinePageViewModel;
            Assert.IsNotNull(vm);
        }

        [TestMethod]
        public void TimelinePageViewModel_Constructs()
        {
            var vm = new TimelinePageViewModel(new MockDataService(), new MockAlertMessageService(), new MockNavigationService(), new MockConfigurationService()) as ITimelinePageViewModel;
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
            var timeLine = new TimelinePageViewModel(mockDataService, new MockAlertMessageService(), new MockNavigationService(), new MockConfigurationService());
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
            var timeLine = new TimelinePageViewModel(mockDataService, mockAlerts, new MockNavigationService(), new MockConfigurationService());
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
            var timeLine = new TimelinePageViewModel(new DataService(new DemoDataRepository(), new MockDataCacheService() { ContainsDataDelegate = s => Task.FromResult(false) }, internetConnection), new MockAlertMessageService(), new MockNavigationService(), new MockConfigurationService());
            var autoResetEvent = new AutoResetEvent(false);
            ExecuteOnUIThread(() => timeLine.OnNavigatedTo(null, NavigationMode.New, null));
            autoResetEvent.WaitOne(2000);
            Assert.IsTrue(timeLine.SquareFootEvents.Any(), "SquareFootEvents");
            Assert.IsTrue(timeLine.TimeLineItems.Any(), "TimeLineItems");
            Assert.IsTrue(timeLine.TimeLineMenuItems.Any(), "TimelineMenuItems");
        }

        [TestMethod]
        public async Task Set_Startup_Event_Command_To_Configuration()
        {
            string passedEventId = String.Empty;
            bool called = false;
            var timeLine = new TimelinePageViewModel(new MockDataService(), new MockAlertMessageService(), new MockNavigationService(), new MockConfigurationService()
            {
                SetStartupEventDelegate = s =>
                {
                    called = true;
                    passedEventId = s;
                }
            });
            await ExecuteOnUIThread(async () =>
            {
                timeLine.SelectedEvent = new EventItemViewModel(MockModelGenerator.NewEvent("0", "Event"));
                await timeLine.SetStartupCommand.Execute();
            });
            Assert.IsTrue(called);
            Assert.AreEqual(passedEventId, "0");
        }

        [TestMethod]
        public void Set_Startup_Event_Needs_Selected_Event()
        {
            var timeLine = new TimelinePageViewModel(new MockDataService(), new MockAlertMessageService(),
                new MockNavigationService(), new MockConfigurationService()
                {
                    SetStartupEventDelegate = s =>
                    {
                    }
                });
            Assert.IsFalse(timeLine.SetStartupCommand.CanExecute());
        }

        [TestMethod]
        public async Task Startup_Flow()
        {
            var configuration = new ConfigurationService();
            configuration.ClearStartupItem();

            var timeLine = new TimelinePageViewModel(new MockDataService(), new MockAlertMessageService(),
                new MockNavigationService(), configuration);

            timeLine.SelectedEvent = new EventItemViewModel(MockModelGenerator.NewEvent("0", "Event"));
            Assert.IsTrue(timeLine.SetStartupCommand.CanExecute(), "SetStartupCommand IsTrue");
            Assert.IsFalse(timeLine.ClearStartupCommand.CanExecute(), "ClearStartupCommand IsFalse");
            Assert.AreEqual(timeLine.SetStartupVisibility, Visibility.Visible, "SetStartupVisibility");
            Assert.AreEqual(timeLine.ClearStartupVisibility, Visibility.Collapsed, "ClearStartupVisibility");

            //set event, can clear afterwards
            await timeLine.SetStartupCommand.Execute();
            Assert.IsTrue(timeLine.ClearStartupCommand.CanExecute(), "ClearStartupCommand IsTrue");
            Assert.IsFalse(timeLine.SetStartupCommand.CanExecute(), "SetStartupCommand IsFalse");
            Assert.AreEqual(timeLine.ClearStartupVisibility, Visibility.Visible, "ClearStartupVisibility");
            Assert.AreEqual(timeLine.SetStartupVisibility, Visibility.Collapsed, "SetStartupVisibility");

            //select new event, back to being able to set
            timeLine.SelectedEvent = new EventItemViewModel(MockModelGenerator.NewEvent("1", "Event"));
            Assert.IsTrue(timeLine.SetStartupCommand.CanExecute(), "SetStartupCommand IsTrue");
            Assert.IsFalse(timeLine.ClearStartupCommand.CanExecute(), "ClearStartupCommand IsFalse");
            Assert.AreEqual(timeLine.SetStartupVisibility, Visibility.Visible, "SetStartupVisibility");
            Assert.AreEqual(timeLine.ClearStartupVisibility, Visibility.Collapsed, "ClearStartupVisibility");
        }


        [TestMethod]
        public void Api_SquareFootEventsSorted()
        {
            var autoREsetEvents = new AutoResetEvent(false);
            var timeLine = new TimelinePageViewModel(new DataService(new ApiDataRepository(new ApiConfiguration()), new DataCacheService(), new InternetConnectionService()), new MockAlertMessageService(),
               new MockNavigationService(), new MockConfigurationService());
            ExecuteOnUIThread(() =>
            {
                timeLine.OnNavigatedTo(null, NavigationMode.New, null);
                Assert.IsTrue(timeLine.SetStartupCommand.CanExecute(), "CanExecute True");
            });
            autoREsetEvents.WaitOne(3000);
            Assert.IsNotNull(timeLine.SquareFootEvents);
            Assert.IsTrue(timeLine.SquareFootEvents.Any(), "SquareFootEvents Any");
            var sortedEvents = timeLine.SquareFootEvents.OrderByDescending(x => x.DateStart).ToList();
            for (int i = 0; i < sortedEvents.Count; i++)
            {
                Assert.AreEqual(sortedEvents[i].Id, timeLine.SquareFootEvents[i].Id, "Matching Sorted Id");
            }
        }

        #region Pinning

        [TestMethod]
        public void Pinning_PinContextChanged()
        {
            bool changed = false;
            var data = new MockDataService();
            INavigationService navigation = new MockNavigationService();
            IConfigurationService configuration = new MockConfigurationService();
            var vm = new TimelinePageViewModel(data, new MockAlertMessageService(), navigation, configuration);
            vm.PinContextChanged += (sender, args) =>
            {
                changed = true;
            };
            vm.SelectedEvent = new EventItemViewModel(MockModelGenerator.NewEvent("0", "Name"));
            Assert.IsTrue(changed, "PinContextChanged");
        }

        [TestMethod]
        public void Pinning_PinContextChanged_TileId()
        {
            bool changed = false;
            var data = new MockDataService();
            INavigationService navigation = new MockNavigationService();
            IConfigurationService configuration = new MockConfigurationService();
            var vm = new TimelinePageViewModel(data, new MockAlertMessageService(), navigation, configuration);
            vm.PinContextChanged += (sender, args) =>
            {
                changed = true;
            };
            vm.SelectedEvent = new EventItemViewModel(MockModelGenerator.NewEvent("0", "Name"));
            Assert.IsTrue(changed, "PinContextChanged");
            var secondaryTileArgs = vm.GetSecondaryTileArguments();
            Assert.AreEqual(secondaryTileArgs.Id, PinningUtils.GetSecondaryTileIdByEventId("0"));
            Assert.AreEqual(secondaryTileArgs.ArgumentsName, PinningUtils.GetSecondaryTileIdByEventId("0"));
            //todo insert tests against color
        }
        #endregion

    }
}
