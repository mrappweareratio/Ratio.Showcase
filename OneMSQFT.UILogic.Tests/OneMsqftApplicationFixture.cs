using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.UI.Core;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using OneMSQFT.Common.Models;
using OneMSQFT.UILogic.DataLayer;
using OneMSQFT.UILogic.Navigation;
using OneMSQFT.UILogic.Services;
using OneMSQFT.UILogic.Tests.Mocks;

namespace OneMSQFT.UILogic.Tests
{
    [TestClass]
    public class OneMsqftApplicationFixture
    {
        public IAsyncAction ExecuteOnUIThread(DispatchedHandler action)
        {
            return Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, action);
        }

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
            var app = new OneMsqftApplication(navigationService, new MockDataService(), new MockConfigurationService());
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
            var app = new OneMsqftApplication(navigationService, new MockDataService()
            {
                GetEventsDelegate = async () =>
                {
                    called = true;
                    return new List<Event>();
                }
            }, new MockConfigurationService());
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
            var dataService = new DataService(new DemoDataRepository(), new MockDataCacheService(), 
                new MockInternetConnectionService() { IsConnectedDelegate = () => true });
            var app = new OneMsqftApplication(navigationService, dataService, new MockConfigurationService());
            ExecuteOnUIThread(() => app.OnLaunchApplication(new MockLaunchActivatedEventArgs()));
            autoResetEvent.WaitOne(1500);
            Assert.AreEqual(page, ViewLocator.Pages.Timeline, "On Timeline");
        }

        [TestMethod]
        public void Application_Launch_PreviousExecutionState_Running_Skips_Events()
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
            var app = new OneMsqftApplication(navigationService, new MockDataService()
            {
                GetEventsDelegate = async () =>
                {
                    called = true;
                    return new List<Event>();
                }
            }, new MockConfigurationService());
            ExecuteOnUIThread(() => app.OnLaunchApplication(new MockLaunchActivatedEventArgs() { PreviousExecutionState = ApplicationExecutionState.Running }));
            autoResetEvent.WaitOne(200);
            Assert.IsFalse(called, "Skipped Data Service");
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
            var app = new OneMsqftApplication(navigationService, data, new MockConfigurationService()
            {
                StartupItemId = eventId,
                StartupItemType = StartupItemType.Event
            });
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
            var app = new OneMsqftApplication(navigationService, data, new MockConfigurationService()
            {
                StartupItemId = exhibitId,
                StartupItemType = StartupItemType.Exhibit
            });
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
            var app = new OneMsqftApplication(navigationService, data, new MockConfigurationService()
            {
                StartupItemId = eventId,
                StartupItemType = StartupItemType.Exhibit
            });
            ExecuteOnUIThread(() => app.OnLaunchApplication(new MockLaunchActivatedEventArgs()));
            autoResetEvent.WaitOne(2000);
            Assert.AreEqual(page, ViewLocator.Pages.Timeline, "Revert to Timeline");
            Assert.IsNull(pageParam, "null param");
        }
    }
}
