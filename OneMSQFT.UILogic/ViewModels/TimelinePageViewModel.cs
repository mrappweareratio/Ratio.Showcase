using System.Collections.ObjectModel;
using OneMSQFT.UILogic.Interfaces.ViewModels;

namespace OneMSQFT.UILogic.ViewModels
{
    public class TimelinePageViewModel : BasePageViewModel, ITimelinePageViewModel
    {
        public TimelinePageViewModel()
        {
            this.SquareFootFutureEvents = new ObservableCollection<EventItemViewModel>();
            this.SquareFootPastEvents = new ObservableCollection<EventItemViewModel>();
            this.TimeLineItems = new ObservableCollection<EventItemViewModel>();
            this.TimeLineMenuItems = new ObservableCollection<EventItemViewModel>();
        }

        public ObservableCollection<EventItemViewModel> SquareFootFutureEvents { get; private set; }
        public ObservableCollection<EventItemViewModel> SquareFootPastEvents { get; private set; }
        public ObservableCollection<EventItemViewModel> TimeLineItems { get; private set; }
        public ObservableCollection<EventItemViewModel> TimeLineMenuItems { get; private set; }
    }
}