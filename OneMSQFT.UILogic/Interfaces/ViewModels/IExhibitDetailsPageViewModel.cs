using System.Collections.ObjectModel;
using OneMSQFT.Common.Models;
using OneMSQFT.UILogic.ViewModels;
using Windows.UI.Core;
using Microsoft.Practices.Prism.StoreApps;
using System;

namespace OneMSQFT.UILogic.Interfaces.ViewModels
{
    public interface IExhibitDetailsPageViewModel
    {
        ExhibitItemViewModel Exhibit { get; set; }
        ExhibitItemViewModel NextExhibit { get; }
        double FullScreenHeight { get;}
        double FullScreenWidth { get;}
        double ExhibitItemWidth { get; }
        double ExhibitItemHeight { get; }
        DelegateCommand<MediaContentSourceItemViewModel> LaunchVideoCommand { get; }
        DelegateCommand<String> NextExhibitCommand { get; }
        ObservableCollection<EventItemViewModel> SquareFootEvents { get; }
        void WindowSizeChanged(double width, double height);        
    }
}