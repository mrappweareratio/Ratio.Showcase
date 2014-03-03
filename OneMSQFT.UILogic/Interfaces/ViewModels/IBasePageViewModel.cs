using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Dynamic;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.StoreApps;
using OneMSQFT.Common.Models;
using OneMSQFT.UILogic.ViewModels;

namespace OneMSQFT.UILogic.Interfaces.ViewModels
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
    }
}