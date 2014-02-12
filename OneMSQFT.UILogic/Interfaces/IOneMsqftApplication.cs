using System.Collections.Generic;
using System.Threading.Tasks;
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
    }
}