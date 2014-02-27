using System.Collections.ObjectModel;
using System.ComponentModel;
using Windows.UI.Xaml;
using OneMSQFT.UILogic.ViewModels;
using Windows.UI.Core;
using Microsoft.Practices.Prism.StoreApps;

namespace OneMSQFT.UILogic.Interfaces.ViewModels
{
    public interface ITimelinePageViewModel : IBasePageViewModel
    {
        double ZoomedOutItemHeight { get; }
        double ZoomedOutItemWidth { get; }
        double EventItemWidth { get; }
        double EventItemHeight { get; }
        double FullScreenHeight { get; set; }
        double FullScreenWidth { get; set; }
        double MaskItemWidth { get; }
        double BufferItemWidth { get; }
        double ExhibitItemHeight { get; }
        double ExhibitItemWidth { get; }        
        /// <summary>
        /// Timeline Page Zoomed In View Items
        /// </summary>
        ObservableCollection<EventItemViewModel> TimeLineItems{ get; }       
        /// <summary>
        /// Timeline Page Zoomed Out View Items
        /// </summary>
        ObservableCollection<EventItemViewModel> TimeLineMenuItems { get; }
        DelegateCommand<EventItemViewModel> EventHeroItemClickCommand { get; set; }
        void EventHeroItemClickCommandHandler(EventItemViewModel item);
        DelegateCommand SetStartupCommand { get; }
        DelegateCommand ClearStartupCommand { get; }
        Visibility ClearStartupVisibility { get; set; }
        Visibility SetStartupVisibility { get; set; }
        EventItemViewModel SelectedEvent { get; set; }
    }
}