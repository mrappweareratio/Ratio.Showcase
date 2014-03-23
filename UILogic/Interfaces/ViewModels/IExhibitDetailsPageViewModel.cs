using System;
using Microsoft.Practices.Prism.StoreApps;
using Ratio.Showcase.UILogic.ViewModels;

namespace Ratio.Showcase.UILogic.Interfaces.ViewModels
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