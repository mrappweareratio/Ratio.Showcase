using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using OneMSQFT.Common.Models;
using OneMSQFT.UILogic.Analytics;
using OneMSQFT.UILogic.Services;
using OneMSQFT.UILogic.Tests.Mocks;

namespace OneMSQFT.UILogic.Tests.Services
{
    [TestClass]
    public class AnalyticsServiceFixture
    {
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
            var app = new OneMsqftApplication(navigationService, data, configuration, new AnalyticsService(), new MockAlertMessageService());            
            app.OnInitialize(new MockLaunchActivatedEventArgs());            
        }

        [TestMethod]
        async public Task Launch_Starts_Session()
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
            var app = new OneMsqftApplication(navigationService, data, configuration, new AnalyticsService(), new MockAlertMessageService());
            var args = new MockLaunchActivatedEventArgs();
            app.OnInitialize(args);            
            await app.OnLaunchApplication(args);
        }

        [TestMethod]
        public async Task Suspend_Stops_Session()
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
            var analytics = new AnalyticsService();
            var app = new OneMsqftApplication(navigationService, data, configuration, analytics, new MockAlertMessageService());
            var args = new MockLaunchActivatedEventArgs();
            app.OnInitialize(args);
            await app.OnLaunchApplication(args);
            app.OnSuspending(new MockSuspendingEventArgs());            
        }

        [TestMethod]
        public async Task Resume_Restarts_Session()
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
            var analytics =new AnalyticsService();
            var app = new OneMsqftApplication(navigationService, data, configuration, analytics, new MockAlertMessageService());
            var args = new MockLaunchActivatedEventArgs();
            app.OnInitialize(args);
            await app.OnLaunchApplication(args);
            app.OnSuspending(new MockSuspendingEventArgs());
            app.OnResuming();            
        }
    }
}
