using System.Collections.ObjectModel;
using System.ComponentModel;
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
        double FullScreenHeight { get;}
        double FullScreenWidth { get;}
        DelegateCommand<MediaContentSourceItemViewModel> LaunchVideoCommand { get; }
        DelegateCommand<String> NextExhibitCommand { get; }
        DelegateCommand SetStartupCommand { get; }
        DelegateCommand ClearStartupCommand { get; }     
    }
}