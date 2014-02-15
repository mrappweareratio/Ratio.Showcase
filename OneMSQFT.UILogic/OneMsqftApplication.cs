using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.UI.ApplicationSettings;
using Windows.UI.Popups;
using Microsoft.Practices.Prism.StoreApps.Interfaces;
using OneMSQFT.Common.Services;
using OneMSQFT.UILogic.Interfaces;
using OneMSQFT.UILogic.Navigation;
using Strings = OneMSQFT.Common.Strings;

namespace OneMSQFT.UILogic
{
    public class OneMsqftApplication : IOneMsqftApplication
    {
        public IDataService DataService { get; private set; }
        public INavigationService NavigationService { get; private set; }

        public OneMsqftApplication(INavigationService navigationService, IDataService dataService)
        {
            DataService = dataService;
            NavigationService = navigationService;
        }

        async public Task OnLaunchApplication(ILaunchActivatedEventArgs args)
        {
            if (args.PreviousExecutionState != ApplicationExecutionState.Running)
            {
                await DataService.GetEvents();
            }
            NavigationService.Navigate(ViewLocator.Pages.Timeline, null);            
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