using System.Collections.ObjectModel;
using OneMSQFT.UILogic.Interfaces.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Core;

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
        private double _zoomedOutItemWidth;
        private double _zoomedOutItemHeight;
        public double ZoomedOutItemWidth
        {
            get
            {
                return _zoomedOutItemWidth;
            }
            set
            { 
                if(value != null)
                {
                    SetProperty(ref _zoomedOutItemWidth, value);
                }
            }
        }
        public double ZoomedOutItemHeight
        {
            get
            {
                return _zoomedOutItemHeight;
            }
            set
            {
                if (value != null)
                {
                    SetProperty(ref _zoomedOutItemHeight, value);
                }
            }
        }


        public void WindowSizeChanged(double width, double height)
        {
            ZoomedOutItemWidth = width/6;
            ZoomedOutItemHeight = height / 4;
        }

    }
}