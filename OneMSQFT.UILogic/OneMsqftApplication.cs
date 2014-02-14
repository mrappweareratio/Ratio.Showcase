using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.UI.ApplicationSettings;
using Windows.UI.Popups;
using Microsoft.Practices.Prism.StoreApps.Interfaces;
using OneMSQFT.Common.Models;
using OneMSQFT.Common.Services;
using OneMSQFT.UILogic.Interfaces;
using OneMSQFT.UILogic.Navigation;
using Strings = OneMSQFT.Common.Strings;

namespace OneMSQFT.UILogic
{
    public class OneMsqftApplication : IOneMsqftApplication
    {
        public IConfigurationService Configuration { get; private set; }
        public IDataService DataService { get; private set; }
        public INavigationService NavigationService { get; private set; }

        public OneMsqftApplication(INavigationService navigationService, IDataService dataService, IConfigurationService configuration)
        {
            Configuration = configuration;
            DataService = dataService;
            NavigationService = navigationService;
        }

        async public Task OnLaunchApplication(ILaunchActivatedEventArgs args)
        {
            if (args.PreviousExecutionState != ApplicationExecutionState.Running)
            {
                await DataService.GetEvents();
            }
            switch (Configuration.StartupItemType)
            {
                case StartupItemType.None:
                    NavigationService.Navigate(ViewLocator.Pages.Timeline, null);            
                    break;
                case StartupItemType.Event:
                    NavigationService.Navigate(ViewLocator.Pages.Timeline, Configuration.StartupItemId);            
                    break;
                case StartupItemType.Exhibit:
                    break;                
            }            
        }

        public void OnInitialize(IActivatedEventArgs args)
        {
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
                commands.Add(
                    new SettingsCommand("Config", Strings.Config, async command =>
                    {
                        await new MessageDialog("Config or Admin section TBD").ShowAsync();
                    }));
            }
            return commands;
        }
    }
}