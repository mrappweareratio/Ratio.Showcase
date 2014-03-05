using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.UI.Core;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using OneMSQFT.Common;
using OneMSQFT.Common.Analytics;
using OneMSQFT.Common.Models;
using OneMSQFT.UILogic.Analytics;
using OneMSQFT.UILogic.DataLayer;
using OneMSQFT.UILogic.Navigation;
using OneMSQFT.UILogic.Services;
using OneMSQFT.UILogic.Tests.Mocks;
using OneMSQFT.UILogic.Utils;

namespace OneMSQFT.UILogic.Tests
{
    [TestClass]
    public class OneMsqftApplicationFixture
    {
        public IAsyncAction ExecuteOnUIThread(DispatchedHandler action)
        {
            return Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, action);
        }

        #region UnhandledException

        [TestMethod]
        async public Task UnhandledException_Debug_Alerts()
        {
            bool alerted = true;
            var app = new OneMsqftApplication(new MockNavigationService(), new MockDataService(),
                new MockConfigurationService(), new MockAnalyticsService(), new MockAlertMessageService()
                {
                    ShowAsyncDelegate = async (s, s1) =>
                    {
                        alerted = true;
                    }
                });
            var canHandleException = app.CanHandleException(new ArgumentOutOfRangeException());
#if DEBUG
            Assert.IsTrue(canHandleException, "CanHandleException");
#endif
            if (canHandleException)
                await app.HandleException(new ArgumentOutOfRangeException(), "ArgumentOutOfRangeException");

#if DEBUG
            Assert.IsTrue(alerted, "alerted");
#endif
        }

        #endregion

        #region Launch and Configuration
        [TestMethod]
        public void Application_Launches_To_Timeline_Page_In_Time()
        {
            string page = null;
            var autoResetEvent = new AutoResetEvent(false);
            var navigationService = new MockNavigationService()
                {
                    NavigateDelegate = (a, b) =>
                    {
                        page = a;
                        return true;
                    }
                };
            var analytics = new MockAnalyticsService();
            var app = new OneMsqftApplication(navigationService, new MockDataService(), new MockConfigurationService(), analytics, new MockAlertMessageService());
            ExecuteOnUIThread(() => app.OnLaunchApplication(new MockLaunchActivatedEventArgs()));
            autoResetEvent.WaitOne(2000);
            Assert.AreEqual(page, ViewLocator.Pages.Timeline, "On Timeline");
        }

        [TestMethod]
        public void Application_Launche_Loads_Events()
        {
            string page = null;
            bool called = false;
            var autoResetEvent = new AutoResetEvent(false);
            var navigationService = new MockNavigationService()
            {
                NavigateDelegate = (a, b) =>
                {
                    page = a;
                    return true;
                }
            };
            var analytics = new MockAnalyticsService();
            var app = new OneMsqftApplication(navigationService, new MockDataService()
            {
                GetEventsDelegate = async () =>
                {
                    called = true;
                    return new List<Event>();
                }
            }, new MockConfigurationService(), analytics, new MockAlertMessageService());
            ExecuteOnUIThread(() => app.OnLaunchApplication(new MockLaunchActivatedEventArgs()));
            autoResetEvent.WaitOne(200);
            Assert.IsTrue(called, "Called Data Service");
            Assert.AreEqual(page, ViewLocator.Pages.Timeline, "On Timeline");
        }


        [TestMethod]
        public void Application_Launches_Loads_Events_In_Time()
        {
            string page = null;
            var autoResetEvent = new AutoResetEvent(false);
            var navigationService = new MockNavigationService()
            {
                NavigateDelegate = (a, b) =>
                {
                    page = a;
                    return true;
                }
            };
            var analytics = new MockAnalyticsService();
            var dataService = new DataService(new DemoDataRepository(), new MockDataCacheService(), new MockInternetConnectionService(true));
            var app = new OneMsqftApplication(navigationService, dataService, new MockConfigurationService(), analytics, new MockAlertMessageService());
            ExecuteOnUIThread(() => app.OnLaunchApplication(new MockLaunchActivatedEventArgs()));
            autoResetEvent.WaitOne(1500);
            Assert.AreEqual(page, ViewLocator.Pages.Timeline, "On Timeline");
        }

        [TestMethod]
        public void Application_Launch_PreviousExecutionState_Running_Skips_Events()
        {
            string page = null;
            int called = 0;
            var autoResetEvent = new AutoResetEvent(false);
            var navigationService = new MockNavigationService()
            {
                NavigateDelegate = (a, b) =>
                {
                    page = a;
                    return true;
                }
            };
            var analytics = new MockAnalyticsService();
            var app = new OneMsqftApplication(navigationService, new MockDataService()
            {
                GetEventsDelegate = async () =>
                {
                    called++;
                    return new List<Event>();
                }
            }, new MockConfigurationService(), analytics, new MockAlertMessageService());
            ExecuteOnUIThread(() => app.OnLaunchApplication(new MockLaunchActivatedEventArgs() { PreviousExecutionState = ApplicationExecutionState.NotRunning }));
            autoResetEvent.WaitOne(500);
            ExecuteOnUIThread(() => app.OnLaunchApplication(new MockLaunchActivatedEventArgs() { PreviousExecutionState = ApplicationExecutionState.Running }));
            Assert.AreEqual(1, called, "Data Service Called Once after Events initially populated");
            Assert.AreEqual(page, ViewLocator.Pages.Timeline, "On Timeline");
        }

        [TestMethod]
        public void Application_Launch_Configuration_To_Event()
        {
            string page = null;
            object pageParam = null;
            const string eventId = "0";
            bool called = false;
            var autoResetEvent = new AutoResetEvent(false);
            var navigationService = new MockNavigationService()
            {
                NavigateDelegate = (a, b) =>
                {
                    page = a;
                    pageParam = b;
                    return true;
                }
            };
            var data = new MockDataService()
            {
                GetEventsDelegate = () => Task.FromResult<IEnumerable<Event>>(new List<Event>() { MockModelGenerator.NewEvent(eventId, "Event") })
            };
            var analytics = new MockAnalyticsService();
            var app = new OneMsqftApplication(navigationService, data, new MockConfigurationService()
            {
                StartupItemId = eventId,
                StartupItemType = StartupItemType.Event
            }, analytics, new MockAlertMessageService());
            ExecuteOnUIThread(() => app.OnLaunchApplication(new MockLaunchActivatedEventArgs()));
            autoResetEvent.WaitOne(2000);
            Assert.AreEqual(page, ViewLocator.Pages.Timeline, "On Timeline");
            Assert.AreEqual(pageParam, eventId, "Event Id");
        }

        [TestMethod]
        public void Application_Launch_Configuration_To_Exhibit()
        {
            string page = null;
            object pageParam = null;
            const string exhibitId = "0";
            var autoResetEvent = new AutoResetEvent(false);
            var navigationService = new MockNavigationService()
            {
                NavigateDelegate = (a, b) =>
                {
                    page = a;
                    pageParam = b;
                    return true;
                }
            };
            var data = new MockDataService()
            {
                GetEventsDelegate = () =>
                {
                    var ev = MockModelGenerator.NewEvent(exhibitId, "Event");
                    ev.Exhibits = new List<Exhibit> { MockModelGenerator.NewExhibit(exhibitId, "Exhibit") };
                    return Task.FromResult<IEnumerable<Event>>(new List<Event>() { ev });
                }
            };
            var analytics = new MockAnalyticsService();
            var app = new OneMsqftApplication(navigationService, data, new MockConfigurationService()
            {
                StartupItemId = exhibitId,
                StartupItemType = StartupItemType.Exhibit
            }, analytics, new MockAlertMessageService());
            ExecuteOnUIThread(() => app.OnLaunchApplication(new MockLaunchActivatedEventArgs()));
            autoResetEvent.WaitOne(2000);
            Assert.AreEqual(page, ViewLocator.Pages.ExhibitDetails, "On Exhibits");
            Assert.AreEqual(pageParam, exhibitId, "Exhibit Id");
        }

        [TestMethod]
        public void Application_Handles_Bad_Deep_Link()
        {
            string page = null;
            object pageParam = null;
            const string eventId = "0";
            var autoResetEvent = new AutoResetEvent(false);
            var navigationService = new MockNavigationService()
            {
                NavigateDelegate = (a, b) =>
                {
                    page = a;
                    pageParam = b;
                    return true;
                }
            };
            var data = new MockDataService()
            {
                GetEventsDelegate = () => Task.FromResult<IEnumerable<Event>>(new List<Event>())
            };
            var analytics = new MockAnalyticsService();
            var app = new OneMsqftApplication(navigationService, data, new MockConfigurationService()
            {
                StartupItemId = eventId,
                StartupItemType = StartupItemType.Exhibit
            }, analytics, new MockAlertMessageService());
            ExecuteOnUIThread(() => app.OnLaunchApplication(new MockLaunchActivatedEventArgs()));
            autoResetEvent.WaitOne(2000);
            Assert.AreEqual(page, ViewLocator.Pages.Timeline, "Revert to Timeline");
            Assert.IsNull(pageParam, "null param");
        }
        #endregion

        #region Analytics
        [TestMethod]
        public void Init_Configures_Analytics()
        {
            bool called = false;
            string page = null;
            object pageParam = null;
            var autoResetEvent = new AutoResetEvent(false);
            var navigationService = new MockNavigationService()
            {
                NavigateDelegate = (a, b) =>
                {
                    page = a;
                    pageParam = b;
                    return true;
                }
            };
            var data = new MockDataService()
            {
                GetEventsDelegate = () => Task.FromResult<IEnumerable<Event>>(new List<Event>())
            };
            var configuration = new ConfigurationService();
            var analytics = new MockAnalyticsService()
            {
                ConfigureDelegate = () => { called = true; }
            };
            var app = new OneMsqftApplication(navigationService, data, configuration, analytics, new MockAlertMessageService());
            app.OnInitialize(new MockLaunchActivatedEventArgs());
            Assert.IsTrue(called, "ConfigureDelegate");
        }

        [TestMethod]
        public async Task Launch_Starts_Session()
        {
            bool called = false;
            string page = null;
            object pageParam = null;
            var autoResetEvent = new AutoResetEvent(false);
            var navigationService = new MockNavigationService()
            {
                NavigateDelegate = (a, b) =>
                {
                    page = a;
                    pageParam = b;
                    return true;
                }
            };
            var data = new MockDataService()
            {
                GetEventsDelegate = () => Task.FromResult<IEnumerable<Event>>(new List<Event>())
            };
            var configuration = new ConfigurationService();
            var analytics = new MockAnalyticsService()
            {
                StartSessionDelegate = () => { called = true; }
            };
            var app = new OneMsqftApplication(navigationService, data, configuration, analytics, new MockAlertMessageService());
            var args = new MockLaunchActivatedEventArgs();
            app.OnInitialize(args);
            await app.OnLaunchApplication(args);
            Assert.IsTrue(called, "StartSessionDelegate");
        }

        [TestMethod]
        public async Task Running_Launch_Skips_Starts_Session()
        {
            bool called = false;
            string page = null;
            object pageParam = null;
            var autoResetEvent = new AutoResetEvent(false);
            var navigationService = new MockNavigationService()
            {
                NavigateDelegate = (a, b) =>
                {
                    page = a;
                    pageParam = b;
                    return true;
                }
            };
            var data = new MockDataService()
            {
                GetEventsDelegate = () => Task.FromResult<IEnumerable<Event>>(new List<Event>())
            };
            var configuration = new ConfigurationService();
            var analytics = new MockAnalyticsService()
            {
                StartSessionDelegate = () => { called = true; }
            };
            var app = new OneMsqftApplication(navigationService, data, configuration, analytics, new MockAlertMessageService());
            var args = new MockLaunchActivatedEventArgs() { PreviousExecutionState = ApplicationExecutionState.Running };
            await app.OnLaunchApplication(args);
            Assert.IsFalse(called, "StartSessionDelegate");
        }

        [TestMethod]
        public async Task Suspend_Stops_Session()
        {
            bool called = false;
            string page = null;
            object pageParam = null;
            var navigationService = new MockNavigationService()
            {
                NavigateDelegate = (a, b) =>
                {
                    page = a;
                    pageParam = b;
                    return true;
                }
            };
            var data = new MockDataService()
            {
                GetEventsDelegate = () => Task.FromResult<IEnumerable<Event>>(new List<Event>())
            };
            var configuration = new ConfigurationService();
            var analytics = new MockAnalyticsService()
            {
                StopSessionDelegate = () => { called = true; }
            };
            var app = new OneMsqftApplication(navigationService, data, configuration, analytics, new MockAlertMessageService());
            var args = new MockLaunchActivatedEventArgs();
            app.OnInitialize(args);
            await app.OnLaunchApplication(args);
            app.OnSuspending(new MockSuspendingEventArgs());
            Assert.IsTrue(called, "StopSessionDelegate");
        }

        [TestMethod]
        public async Task Resume_Restarts_Session()
        {
            bool calledStop = false;
            int calledStart = 0;
            string page = null;
            object pageParam = null;
            var navigationService = new MockNavigationService()
            {
                NavigateDelegate = (a, b) =>
                {
                    page = a;
                    pageParam = b;
                    return true;
                }
            };
            var data = new MockDataService()
            {
                GetEventsDelegate = () => Task.FromResult<IEnumerable<Event>>(new List<Event>())
            };
            var configuration = new ConfigurationService();
            var analytics = new MockAnalyticsService()
            {
                StartSessionDelegate = () => { calledStart++; },
                StopSessionDelegate = () => { calledStop = true; }
            };
            var app = new OneMsqftApplication(navigationService, data, configuration, analytics, new MockAlertMessageService());
            var args = new MockLaunchActivatedEventArgs();
            app.OnInitialize(args);
            await app.OnLaunchApplication(args);
            app.OnSuspending(new MockSuspendingEventArgs());
            app.OnResuming();
            Assert.IsTrue(calledStop, "StopSessionDelegate");
            Assert.AreEqual(calledStart, 2, "Called Start Twice after Resume");
        }
        #endregion

        #region Pinning

        [TestMethod]
        public async Task DeepLink_Pin_Event()
        {
            string page = null;
            object pageParam = null;
            var navigationService = new MockNavigationService()
            {
                NavigateDelegate = (a, b) =>
                {
                    page = a;
                    pageParam = b;
                    return true;
                }
            };
            var data = new MockDataService()
            {
                GetEventsDelegate = () => Task.FromResult<IEnumerable<Event>>(new List<Event>()
                {
                    MockModelGenerator.NewEvent("0", "Event")
                })
            };
            var configuration = new ConfigurationService();
            var analytics = new MockAnalyticsService();
            var app = new OneMsqftApplication(navigationService, data, configuration, analytics, new MockAlertMessageService());
            var args = new MockLaunchActivatedEventArgs()
            {
                Arguments = PinningUtils.GetSecondaryTileIdByEventId("0")
            };
            app.OnInitialize(args);
            await app.OnLaunchApplication(args);
            Assert.AreEqual(page, ViewLocator.Pages.Timeline, "Timeline");
            Assert.AreEqual(pageParam, "0", "Event Id");
        }

        [TestMethod]
        public async Task DeepLink_Pin_Exhibit()
        {
            bool tracked = false;
            string page = null;
            object pageParam = null;
            var navigationService = new MockNavigationService()
            {
                NavigateDelegate = (a, b) =>
                {
                    page = a;
                    pageParam = b;
                    return true;
                }
            };
            var evt = MockModelGenerator.NewEvent("0", "Event");
            evt.Exhibits = new List<Exhibit> { MockModelGenerator.NewExhibit("1", "Exhibit") };
            var data = new MockDataService()
            {
                GetEventsDelegate = () => Task.FromResult<IEnumerable<Event>>(new List<Event>()
                {
                    evt
                })
            };
            var configuration = new ConfigurationService();
            var analytics = new MockAnalyticsService()
            {
                TrackEventsDelegate = (events, context) =>
                {
                    tracked = true;
                    Assert.IsTrue(events.Contains(TrackingEventsData.Events.PageView), "PageView Event");
                    Assert.IsNotNull(context.ExhibitName, "ExhibitName");
                    Assert.AreEqual(context.PageName, TrackingContextData.PageNames.ExhibitLanding, "ExhibitLanding");
                }
            };
            var app = new OneMsqftApplication(navigationService, data, configuration, analytics, new MockAlertMessageService());
            var args = new MockLaunchActivatedEventArgs()
            {
                Arguments = PinningUtils.GetSecondaryTileIdByExhibitId("1")
            };
            app.OnInitialize(args);
            await app.OnLaunchApplication(args);
            Assert.AreEqual(page, ViewLocator.Pages.ExhibitDetails, "Exhibit Page");
            Assert.AreEqual(pageParam, "1", "Exhibit Id");
            Assert.IsTrue(tracked, "tracked");
        }

        [TestMethod]
        public async Task DeepLink_Pin_Bad_Exhibit()
        {
            string page = null;
            object pageParam = null;
            var navigationService = new MockNavigationService()
            {
                NavigateDelegate = (a, b) =>
                {
                    page = a;
                    pageParam = b;
                    return true;
                }
            };
            var evt = MockModelGenerator.NewEvent("0", "Event");
            evt.Exhibits = new List<Exhibit> { MockModelGenerator.NewExhibit("1", "Exhibit") };
            var data = new MockDataService()
            {
                GetEventsDelegate = () => Task.FromResult<IEnumerable<Event>>(new List<Event>()
                {
                    evt
                })
            };
            var configuration = new ConfigurationService();
            var analytics = new MockAnalyticsService();
            var app = new OneMsqftApplication(navigationService, data, configuration, analytics, new MockAlertMessageService());
            var args = new MockLaunchActivatedEventArgs()
            {
                Arguments = PinningUtils.GetSecondaryTileIdByExhibitId("2")
            };
            app.OnInitialize(args);
            await app.OnLaunchApplication(args);
            Assert.AreEqual(page, ViewLocator.Pages.Timeline, "Timeline Page");
            Assert.IsNull(pageParam);
        }

        [TestMethod]
        public async Task DeepLink_Pin_Bad_Event()
        {
            string page = null;
            object pageParam = null;
            var navigationService = new MockNavigationService()
            {
                NavigateDelegate = (a, b) =>
                {
                    page = a;
                    pageParam = b;
                    return true;
                }
            };
            var data = new MockDataService()
            {
                GetEventsDelegate = () => Task.FromResult<IEnumerable<Event>>(new List<Event>())
            };
            var configuration = new ConfigurationService();
            var analytics = new MockAnalyticsService();
            var app = new OneMsqftApplication(navigationService, data, configuration, analytics, new MockAlertMessageService());
            var args = new MockLaunchActivatedEventArgs()
            {
                Arguments = PinningUtils.GetSecondaryTileIdByEventId("0")
            };
            app.OnInitialize(args);
            await app.OnLaunchApplication(args);
            Assert.AreEqual(page, ViewLocator.Pages.Timeline, "Timeline");
            Assert.IsNull(pageParam, "No Page Param");
        }

        #endregion

        #region Error Handling

        [TestMethod]
        public async Task TaskCanceledException_Messages_User_No_Network()
        {
            bool alerted = false;
            string message = null;
            var navigationService = new MockNavigationService();
            var internet = new MockInternetConnectionService(false);
            var data = new DataService(new MockDataRepository()
            {
                GetSiteDataDelegate = () =>
                {
                    //disable internet connection
                    internet.IsConnected = false;
                    //thow timeout or TaskCanceledException
                    throw new TaskCanceledException();
                }
            }, new MockDataCacheService(), internet);

            var configuration = new ConfigurationService();
            var analytics = new MockAnalyticsService();
            var sharing = new MockSharingService();
            var alerts = new MockAlertMessageService()
            {
                ShowAsyncDelegate = (s, s1) =>
                {
                    alerted = true;
                    message = s;
                    return Task.FromResult(0);
                }
            };
            var dispatcher = new MockDispatcherService();
            var app = new OneMsqftApplication(navigationService, data, configuration, analytics, alerts, sharing, internet, dispatcher);
            var args = new MockLaunchActivatedEventArgs();
            app.OnInitialize(args);
            await app.OnLaunchApplication(args);
            Assert.IsTrue(alerted, "Alerted");
            Assert.AreEqual(Strings.InternetConnectionFailureMessage, message);
        }

        [TestMethod]
        public async Task FaultedHandler_Messages_User_ServerFailureMessage()
        {
            bool alerted = false;
            string message = null;
            var navigationService = new MockNavigationService();
            var internet = new MockInternetConnectionService(false);
            var data = new DataService(new MockDataRepository()
            {
                GetSiteDataDelegate = () =>
                {
                    //disable internet connection
                    internet.IsConnected = true;
                    //thow timeout or TaskCanceledException
                    throw new ArgumentNullException();
                }
            }, new MockDataCacheService(), internet);

            var configuration = new ConfigurationService();
            var analytics = new MockAnalyticsService();
            var sharing = new MockSharingService();
            var alerts = new MockAlertMessageService()
            {
                ShowAsyncDelegate = (s, s1) =>
                {
                    alerted = true;
                    message = s;
                    return Task.FromResult(0);
                }
            };
            var dispatcher = new MockDispatcherService();
            var app = new OneMsqftApplication(navigationService, data, configuration, analytics, alerts, sharing, internet, dispatcher);
            var args = new MockLaunchActivatedEventArgs();
            app.OnInitialize(args);
            await app.OnLaunchApplication(args);
            Assert.IsTrue(alerted, "Alerted");
            Assert.AreEqual(Strings.SiteDataFailureMessage, message);
        }

        #endregion
    }
}
