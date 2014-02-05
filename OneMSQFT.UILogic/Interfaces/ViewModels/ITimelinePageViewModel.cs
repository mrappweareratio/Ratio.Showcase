using System.Collections.ObjectModel;
using OneMSQFT.UILogic.ViewModels;
using Windows.UI.Core;
using Microsoft.Practices.Prism.StoreApps;

namespace OneMSQFT.UILogic.Interfaces.ViewModels
{
    public interface ITimelinePageViewModel
    {
        double ZoomedOutItemHeight{get;}
        double ZoomedOutItemWidth { get; }
        double FullScreenItemWidth { get; set; }
        double FullScreenItemHeight { get; set; }
        ObservableCollection<EventItemViewModel> SquareFootEvents { get; }
        ObservableCollection<EventItemViewModel> TimeLineItems{ get; }       
        ObservableCollection<EventItemViewModel> TimeLineMenuItems { get; }
        void WindowSizeChanged(double width, double height);
        DelegateCommand<EventItemViewModel> EventHeroItemClickCommand { get; set; }
        void EventHeroItemClickCommandHandler(EventItemViewModel item);
    }
}