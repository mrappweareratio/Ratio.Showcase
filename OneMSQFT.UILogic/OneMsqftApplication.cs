using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.ApplicationSettings;
using Windows.UI.Popups;
using Windows.UI.StartScreen;
using Microsoft.Practices.Prism.StoreApps.Interfaces;
using OneMSQFT.Common.Analytics;
using OneMSQFT.Common.Models;
using OneMSQFT.Common.Services;
using OneMSQFT.UILogic.Analytics;
using OneMSQFT.UILogic.Interfaces;
using OneMSQFT.UILogic.Navigation;
using OneMSQFT.UILogic.Utils;
using Strings = OneMSQFT.Common.Strings;

namespace OneMSQFT.UILogic
{
    public class OneMsqftApplication : IOneMsqftApplication
    {
        public IAlertMessageService MessageService { get; private set; }
        public IAnalyticsService Analytics { get; set; }
        private IEnumerable<Event> _events;
        public IConfigurationService Configuration { get; private set; }
        public IDataService DataService { get; set; }
        public INavigationService NavigationService { get; private set; }
        public ISharingService SharingService { get; private set; }

        public OneMsqftApplication(INavigationService navigationService, IDataService dataService, IConfigurationService configuration, IAnalyticsService analytics, IAlertMessageService alertMessageService)
        {
            MessageService = alertMessageService;
            Analytics = analytics;
            Analytics.KioskModeEnabled = this.KioskModeEnabled;
            Configuration = configuration;
            DataService = dataService;
            NavigationService = navigationService;
            _events = new List<Event>();
        }

        public OneMsqftApplication(INavigationService navigationService, IDataService dataService, IConfigurationService configuration, IAnalyticsService analytics, IAlertMessageService alertMessageService, ISharingService sharingService)
            : this(navigationService, dataService, configuration, analytics, alertMessageService)
        {
            SharingService = sharingService;
        }

        public async Task HandleException(Exception exception, string message)
        {
            await MessageService.ShowAsync(message, Strings.UnhandledExceptionTitle);
        }

        public bool CanHandleException(Exception exception)
        {
#if DEBUG
            return true;
#endif
            return false;
        }

        async public Task OnLaunchApplication(ILaunchActivatedEventArgs args)
        {
            if (args.PreviousExecutionState != ApplicationExecutionState.Running)
            {
                Analytics.StartSession();
                _events = await DataService.GetEvents();
            }

            if (!String.IsNullOrEmpty(args.Arguments))
            {
                var events = new TrackingEventsData { TrackingEventsData.Events.PageView };
                var context = new TrackingContextData();

                var pinningContext = PinningUtils.ParseArguments(args.Arguments);
                switch (pinningContext.StartupItemType)
                {
                    case StartupItemType.None:
                        break;
                    case StartupItemType.Event:
                        Event evt;
                        if (_events == null || (evt = _events.FirstOrDefault(x => x.Id.Equals(pinningContext.StartupItemId))) == null)
                        {
                            context.PageName = TrackingContextData.PageNames.Home;
                            NavigationService.Navigate(ViewLocator.Pages.Timeline, null);
                        }
                        else
                        {
                            context.PageName = TrackingContextData.PageNames.EventLanding;
                            context.EventName = evt.Name;
                            NavigationService.Navigate(ViewLocator.Pages.Timeline, pinningContext.StartupItemId);
                        }
                        Analytics.TrackEvents(events, context);
                        return;
                    case StartupItemType.Exhibit:
                        Exhibit exhibit;
                        if (_events == null ||
                            (exhibit = _events.SelectMany(x => x.Exhibits).FirstOrDefault(x => x.Id.Equals(pinningContext.StartupItemId))) == null)
                        {
                            context.PageName = TrackingContextData.PageNames.Home;
                            NavigationService.Navigate(ViewLocator.Pages.Timeline, null);
                        }
                        else
                        {
                            context.PageName = TrackingContextData.PageNames.ExhibitLanding;
                            context.ExhibitName = exhibit.Name;
                            NavigationService.Navigate(ViewLocator.Pages.ExhibitDetails, pinningContext.StartupItemId);
                        }
                        Analytics.TrackEvents(events, context);
                        return;
                }
            }
            GoHome(true);
        }

        public void GoHome(bool isLaunch = false)
        {
            var trackingEventsData = new TrackingEventsData { TrackingEventsData.Events.PageView };
            var trackingContextData = new TrackingContextData();

            switch (Configuration.StartupItemType)
            {
                case StartupItemType.None:
                    NavigationService.Navigate(ViewLocator.Pages.Timeline, null);
                    break;
                case StartupItemType.Event:
                    Event evt;
                    if (_events == null || (evt = _events.FirstOrDefault(x => x.Id.Equals(Configuration.StartupItemId))) == null)
                    {
                        trackingContextData.PageName = TrackingContextData.PageNames.Home;
                        NavigationService.Navigate(ViewLocator.Pages.Timeline, null);
                    }
                    else
                    {
                        trackingContextData.PageName = TrackingContextData.PageNames.EventLanding;
                        trackingContextData.EventName = evt.Name;
                        NavigationService.Navigate(ViewLocator.Pages.Timeline, Configuration.StartupItemId);
                    }
                    break;
                case StartupItemType.Exhibit:
                    Exhibit exhibit;
                    if (_events == null ||
                        (exhibit = _events.SelectMany(x => x.Exhibits).FirstOrDefault(x => x.Id.Equals(Configuration.StartupItemId))) == null)
                    {
                        trackingContextData.PageName = TrackingContextData.PageNames.Home;
                        NavigationService.Navigate(ViewLocator.Pages.Timeline, null);
                    }
                    else
                    {
                        trackingContextData.PageName = TrackingContextData.PageNames.ExhibitLanding;
                        trackingContextData.ExhibitName = exhibit.Name;
                        NavigationService.Navigate(ViewLocator.Pages.ExhibitDetails, Configuration.StartupItemId);
                    }
                    break;
            }
            if (isLaunch)
            {
                Analytics.TrackEvents(trackingEventsData, trackingContextData);
            }
        }

        public void OnInitialize(IActivatedEventArgs args)
        {
            Analytics.Configure();
        }

        public void OnSuspending(ISuspendingEventArgs suspendingEventArgs)
        {
            Analytics.StopSession();
        }

        public void OnResuming()
        {
            Analytics.StartSession();
        }

        public bool KioskModeEnabled
        {
            get
            {
#if KIOSK
                return true;
#else
                return false;
#endif
            }
        }

        public IList<SettingsCommand> GetSettingsCommands()
        {
            var commands = new List<SettingsCommand>();
            if (KioskModeEnabled)
            {                
            }
            return commands;
        }
    }
}