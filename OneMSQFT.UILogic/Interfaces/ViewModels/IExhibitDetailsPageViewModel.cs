using System.Collections.ObjectModel;
using OneMSQFT.UILogic.ViewModels;
using Windows.UI.Core;
using Microsoft.Practices.Prism.StoreApps;
using System;

namespace OneMSQFT.UILogic.Interfaces.ViewModels
{
    public interface IExhibitDetailsPageViewModel
    {
        ExhibitItemViewModel Exhibit { get; set; }
        double FullScreenHeight { get;}
        double FullScreenWidth { get;}
        ObservableCollection<EventItemViewModel> SquareFootEvents { get; }
        void WindowSizeChanged(double width, double height);
        ObservableCollection<MediaContentSourceItemViewModel> MediaContentCollection { get; set; }
    }
}