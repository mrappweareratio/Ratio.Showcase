using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Microsoft.Practices.Prism.StoreApps;
using Ratio.Showcase.UILogic.ViewModels;

namespace Ratio.Showcase.UILogic.Interfaces.ViewModels
{
    public interface ITimelinePageViewModel : IBasePageViewModel
    {
        double ZoomedOutItemHeight { get; }
        double ZoomedOutItemWidth { get; }
        double EventItemWidth { get; }
        double EventItemHeight { get; }        
        double MaskItemWidth { get; }
        double MaskItemHeight { get; }
        double BufferItemWidth { get; }
        double BufferItemHeight { get; }
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
        
        Visibility ClearStartupVisibility { get; set; }
        Visibility SetStartupVisibility { get; set; }
        EventItemViewModel SelectedEvent { get; set; } 
    }
}