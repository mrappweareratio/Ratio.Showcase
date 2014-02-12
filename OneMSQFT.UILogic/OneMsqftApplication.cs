using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.UI.ApplicationSettings;
using Windows.UI.Popups;
using Microsoft.Practices.Prism.StoreApps.Interfaces;
using OneMSQFT.UILogic.Interfaces;
using OneMSQFT.UILogic.Navigation;
using Strings = OneMSQFT.Common.Strings;

namespace OneMSQFT.UILogic
{
    public class OneMsqftApplication : IOneMsqftApplication
    {
        public INavigationService NavigationService { get; set; }

        public OneMsqftApplication(INavigationService navigationService)
        {
            NavigationService = navigationService;
        }

        public Task OnLaunchApplication(ILaunchActivatedEventArgs args)
        {
            NavigationService.Navigate(ViewLocator.Pages.Timeline, null);
            return Task.FromResult<object>(null);
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