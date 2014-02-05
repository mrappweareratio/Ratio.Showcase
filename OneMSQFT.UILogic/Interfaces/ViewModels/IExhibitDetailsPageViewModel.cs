using System.Collections.ObjectModel;
using OneMSQFT.UILogic.ViewModels;
using Windows.UI.Core;
using Microsoft.Practices.Prism.StoreApps;

namespace OneMSQFT.UILogic.Interfaces.ViewModels
{
    public interface IExhibitDetailsPageViewModel
    {
        ExhibitItemViewModel Exhibit { get; set; }
        double FullScreenHeight { get; set; }
        double FullScreenWidth { get; set; }
        ObservableCollection<EventItemViewModel> SquareFootEvents { get; }
        void WindowSizeChanged(double width, double height);
    }
}