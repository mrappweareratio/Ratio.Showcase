using System.Collections.ObjectModel;
using OneMSQFT.UILogic.ViewModels;
using Windows.UI.Core;

namespace OneMSQFT.UILogic.Interfaces.ViewModels
{
    public interface ITimelinePageViewModel
    {
        double ZoomedOutItemHeight{get;}
        double ZoomedOutItemWidth { get; }
        ObservableCollection<EventItemViewModel> SquareFootFutureEvents { get; }
        ObservableCollection<EventItemViewModel> SquareFootPastEvents { get; }
        ObservableCollection<EventItemViewModel> TimeLineItems{ get; }       
        ObservableCollection<EventItemViewModel> TimeLineMenuItems { get; }
        void WindowSizeChanged(double width, double height);
    }
}