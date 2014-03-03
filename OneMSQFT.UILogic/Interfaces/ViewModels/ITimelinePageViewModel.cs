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
        /// <summary>
        /// Returns the position of an event on the Timeline Page
        /// Method added once buffer items became insterted into TimelineEvents for visual effects
        /// </summary>
        /// <param name="id"></param>
        /// <returns>null if not found</returns>
        int? GetEventIndexById(string id);
    }
}