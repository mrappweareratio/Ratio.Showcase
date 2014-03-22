using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.ApplicationSettings;
using Windows.UI.Xaml.Controls;
using Ratio.Showcase.Shared.Services;

namespace Ratio.Showcase.UILogic.Interfaces
{
    public interface IOneMsqftApplication
    {
        Task OnLaunchApplication(ILaunchActivatedEventArgs args);
        void OnInitialize(IActivatedEventArgs args);
        bool KioskModeEnabled { get; }
        ISharingService SharingService { get; }
        IList<SettingsCommand> GetSettingsCommands();
        void OnSuspending(ISuspendingEventArgs suspendingEventArgs);
        void OnResuming();
        /// <summary>
        /// Navigate to home based on startup configuration or from a launch
        /// </summary>        
        void GoHome(bool isLaunch = false);
        Task HandleException(Exception exception, string message);
        bool CanHandleException(Exception exception);
        IDataService DataService { get; set; }
        IAnalyticsService Analytics { get; set; }
        IInternetConnectionService InternetConnection { get; }
        IAlertMessageService MessageService { get; }
        Page CurrentPage { get; }
    }
}