using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.ApplicationSettings;

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
        /// Navigate to home or the startup item in configuration
        /// </summary>
        /// <param name="args"></param>
        void GoHome(ILaunchActivatedEventArgs args = null);
    }
}