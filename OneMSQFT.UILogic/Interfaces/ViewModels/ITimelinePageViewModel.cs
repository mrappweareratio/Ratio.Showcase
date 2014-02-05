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
        double EventItemWidth { get; set; }
        double EventItemHeight { get; set; }
        double FullScreenHeight { get; set; }
        double FullScreenWidth { get; set; }
        double ExhibitItemHeight { get; set; }
        double ExhibitItemWidth { get; set; }
        /// <summary>
        /// Application Navigation Bar Bound Top Level Event Items
        /// </summary>
        ObservableCollection<EventItemViewModel> SquareFootEvents { get; }
        /// <summary>
        /// Timeline Page Zoomed In View Items
        /// </summary>
        ObservableCollection<EventItemViewModel> TimeLineItems{ get; }       
        /// <summary>
        /// Timeline Page Zoomed Out View Items
        /// </summary>
        ObservableCollection<EventItemViewModel> TimeLineMenuItems { get; }
        void WindowSizeChanged(double width, double height);
        DelegateCommand<EventItemViewModel> EventHeroItemClickCommand { get; set; }
        void EventHeroItemClickCommandHandler(EventItemViewModel item);
    }
}