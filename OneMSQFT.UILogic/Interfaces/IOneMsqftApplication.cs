using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.ApplicationSettings;
using OneMSQFT.Common.Services;

namespace OneMSQFT.UILogic.Interfaces
{
    public interface IOneMsqftApplication
    {
        Task OnLaunchApplication(ILaunchActivatedEventArgs args);
        void OnInitialize(IActivatedEventArgs args);
        bool KioskModeEnabled { get; }
        IList<SettingsCommand> GetSettingsCommands();
        void OnSuspending(ISuspendingEventArgs suspendingEventArgs);
        void OnResuming();
        /// <summary>
        /// Navigate to home based on startup configuration or from a launch
        /// </summary>        
        void GoHome(bool isLaunch = false);
        IDataService DataService { get; set; }
        IAnalyticsService Analytics { get; set; }
    }
}