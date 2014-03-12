using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.System;
using Windows.UI.ApplicationSettings;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.StartScreen;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Microsoft.Practices.Prism.StoreApps.Interfaces;
using OneMSQFT.Common;
using OneMSQFT.Common.Analytics;
using OneMSQFT.Common.Models;
using OneMSQFT.Common.Services;
using OneMSQFT.UILogic.Analytics;
using OneMSQFT.UILogic.Interfaces;
using OneMSQFT.UILogic.Interfaces.ViewModels;
using OneMSQFT.UILogic.Navigation;
using OneMSQFT.UILogic.Services;
using OneMSQFT.UILogic.Utils;
using ISharingService = OneMSQFT.Common.Services.ISharingService;
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
        public IInternetConnectionService InternetConnection { get; private set; }
        public IDispatcherService DispatcherService { get; private set; }

        public OneMsqftApplication(INavigationService navigationService, IDataService dataService, IConfigurationService configuration, IAnalyticsService analytics, IAlertMessageService alertMessageService)
        {
            MessageService = alertMessageService;
            Analytics = analytics;
            Analytics.KioskModeEnabled = this.KioskModeEnabled;
            Configuration = configuration;
            DataService = dataService;
            NavigationService = navigationService;
        }

        public OneMsqftApplication(INavigationService navigationService, IDataService dataService, IConfigurationService configuration, IAnalyticsService analytics, IAlertMessageService alertMessageService, ISharingService sharingService, IInternetConnectionService internetConnectionService, IDispatcherService dispatcherService)
            : this(navigationService, dataService, configuration, analytics, alertMessageService)
        {
            SharingService = sharingService;
            InternetConnection = internetConnectionService;
            DispatcherService = dispatcherService;
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
            }

            Func<Task> getEventsFailure = async () =>
            {
                if (InternetConnection.IsConnected)
                {
                    await DispatcherService.RunAsync(async () =>
                    {
                        await MessageService.ShowAsync(Strings.SiteDataFailureMessage, String.Empty);
                    });

                }
                else
                {
                    await DispatcherService.RunAsync(async () =>
                    {
                        await MessageService.ShowAsync(Strings.InternetConnectionFailureMessage, String.Empty);
                    });
                }
            };

            if (_events == null || !_events.Any())
            {

                _events = await Task.Run(async () => await DataService.GetEvents(new CancellationToken()).TryCatchAsync(async exception =>
                {
                    //task faulted
                    await getEventsFailure();
                }, async () =>
                {
                    //task cancelled
                    await getEventsFailure();
                }));
            }

            if (_events == null)
            {
                NavigationService.Navigate(ViewLocator.Pages.About, null);
                return;
            }

            if (!String.IsNullOrEmpty(args.Arguments))
            {
                var pinningContext = PinningUtils.ParseArguments(args.Arguments);
                switch (pinningContext.StartupItemType)
                {
                    case StartupItemType.Event:
                        Event evt;
                        if (_events == null || (evt = _events.FirstOrDefault(x => x.Id.Equals(pinningContext.StartupItemId))) == null)
                        {
                            Analytics.TrackPageViewHome();
                            NavigationService.Navigate(ViewLocator.Pages.Timeline, null);
                        }
                        else
                        {

                            Analytics.TrackEventLanding(evt.Name);
                            NavigationService.Navigate(ViewLocator.Pages.Timeline, pinningContext.StartupItemId);
                        }
                        return;
                    case StartupItemType.Exhibit:
                        Exhibit exhibit;
                        if (_events == null ||
                            (exhibit = _events.SelectMany(x => x.Exhibits).FirstOrDefault(x => x.Id.Equals(pinningContext.StartupItemId))) == null)
                        {
                            Analytics.TrackPageViewHome();
                            NavigationService.Navigate(ViewLocator.Pages.Timeline, null);
                        }
                        else
                        {
                            Analytics.TrackExhibitLanding(exhibit.Name);
                            NavigationService.Navigate(ViewLocator.Pages.ExhibitDetails, pinningContext.StartupItemId);
                        }
                        return;
                }
            }
            GoHome(true);
        }

        public void GoHome(bool isLaunch = false)
        {
            switch (Configuration.StartupItemType)
            {
                case StartupItemType.None:
                    if (isLaunch) Analytics.TrackPageViewHome();
                    NavigationService.Navigate(ViewLocator.Pages.Timeline, null);
                    break;
                case StartupItemType.Event:
                    Event evt;
                    if (_events == null || (evt = _events.FirstOrDefault(x => x.Id.Equals(Configuration.StartupItemId))) == null)
                    {
                        if (isLaunch) Analytics.TrackPageViewHome();
                        NavigationService.Navigate(ViewLocator.Pages.Timeline, null);
                    }
                    else
                    {
                        if (isLaunch) Analytics.TrackEventLanding(evt.Name);
                        NavigationService.Navigate(ViewLocator.Pages.Timeline, Configuration.StartupItemId);
                    }
                    break;
                case StartupItemType.Exhibit:
                    Exhibit exhibit;
                    if (_events == null ||
                        (exhibit = _events.SelectMany(x => x.Exhibits).FirstOrDefault(x => x.Id.Equals(Configuration.StartupItemId))) == null)
                    {
                        if (isLaunch) Analytics.TrackPageViewHome();
                        NavigationService.Navigate(ViewLocator.Pages.Timeline, null);
                    }
                    else
                    {
                        if (isLaunch) Analytics.TrackExhibitLanding(exhibit.Name);
                        NavigationService.Navigate(ViewLocator.Pages.ExhibitDetails, Configuration.StartupItemId);
                    }
                    break;
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
            var settingsCommands = new List<SettingsCommand>();
            if (KioskModeEnabled && CurrentPage != null)
            {
                var vm = CurrentPage.DataContext as IBasePageViewModel;
                if (vm != null)
                {
                    if (vm.SetStartupCommand.CanExecute())
                        settingsCommands.Add(new SettingsCommand(Guid.NewGuid().ToString(), Strings.SetAsStartUp, command => vm.SetStartupCommand.Execute()));
                    if (vm.ClearStartupCommand.CanExecute())
                        settingsCommands.Add(new SettingsCommand(Guid.NewGuid().ToString(), Strings.ClearStartUp, command => vm.ClearStartupCommand.Execute()));
                }
            }
            if (KioskModeEnabled)
            {
                settingsCommands.Add(new SettingsCommand(Guid.NewGuid().ToString(), Strings.ClearDataCache,
                    (c) => DataService.ClearDataCacheInStorage()));
            }
            settingsCommands.Add(new SettingsCommand(Guid.NewGuid().ToString(), Strings.PrivacyPolicy, async (c) => await Launcher.LaunchUriAsync(new Uri(Strings.PrivacyPolicyUrl))));
            return settingsCommands;
        }

        public Page CurrentPage
        {
            get
            {
                if (Window.Current == null) return null;
                var frame = Window.Current.Content as Frame;
                if (frame == null) return null;
                var page = frame.Content as Page;
                return page;
            }
        }
    }
}