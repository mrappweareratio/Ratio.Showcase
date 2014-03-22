using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Microsoft.Practices.Prism.StoreApps;
using Ratio.Showcase.Shared.Models;
using Ratio.Showcase.UILogic.ViewModels;

namespace Ratio.Showcase.UILogic.Interfaces.ViewModels
{
    public interface IBasePageViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Application Navigation Bar Bound Top Level Event Items
        /// </summary>
        ObservableCollection<EventItemViewModel> SquareFootEvents { get; }
        void WindowSizeChanged(double width, double height);
        /// <summary>
        /// Fires when the pin item changes and need to be check for pin vs unpin
        /// </summary>
        event EventHandler<EventArgs> PinContextChanged;              
        SecondaryTileArgs GetSecondaryTileArguments();
        Task<SecondaryTileImages> GetSecondaryTileImages();
        bool IsHorizontal { get; }        
        double FullScreenHeight { get; }
        double FullScreenWidth { get; }
        DelegateCommand SetStartupCommand { get; }
        DelegateCommand ClearStartupCommand { get; }
        Visibility ClearStartupVisibility { get;  }
        Visibility SetStartupVisibility { get;  }
        /// <summary>
        /// Returns the position of an event on the Timeline Page
        /// Method added once buffer items became insterted into TimelineEvents for visual effects
        /// </summary>
        /// <param name="id"></param>
        /// <returns>null if not found</returns>
        int? GetEventIndexById(string id);
        TaskCompletionSource<bool> LoadedEventsTaskCompletionSource { get; }
    }
}