using System.Collections.ObjectModel;
using System.ComponentModel;
using Windows.UI.Xaml;
using OneMSQFT.Common.Models;
using OneMSQFT.UILogic.ViewModels;
using Windows.UI.Core;
using Microsoft.Practices.Prism.StoreApps;
using System;

namespace OneMSQFT.UILogic.Interfaces.ViewModels
{
    public interface IExhibitDetailsPageViewModel : IBasePageViewModel
    {
        ExhibitItemViewModel Exhibit { get; set; }
        ExhibitItemViewModel NextExhibit { get; }
        DelegateCommand<MediaContentSourceItemViewModel> LaunchVideoCommand { get; }
        DelegateCommand<String> NextExhibitCommand { get; }       
        MediaContentSourceItemViewModel SelectedMediaContentSource { get; set; }
        double NineSixteenthsOfWidth { get; }
        double OneThirdPanelWidth { get; }
        double PortraitHeaderFooterHeight { get; }
    }
}