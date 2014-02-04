using System.Collections.ObjectModel;
using OneMSQFT.UILogic.ViewModels;

namespace OneMSQFT.UILogic.Interfaces.ViewModels
{
    public interface ITimelinePageViewModel
    {
        ObservableCollection<EventItemViewModel> SquareFootFutureEvents { get; }
        ObservableCollection<EventItemViewModel> SquareFootPastEvents { get; }
        ObservableCollection<EventItemViewModel> TimeLineItems{ get; }       
        ObservableCollection<EventItemViewModel> TimeLineMenuItems { get; }        
    }
}