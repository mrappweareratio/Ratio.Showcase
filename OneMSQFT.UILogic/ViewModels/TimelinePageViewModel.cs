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
            this.SquareFootEvents = new ObservableCollection<EventItemViewModel>();
            this.SquareFootPastEvents = new ObservableCollection<EventItemViewModel>();
            this.TimeLineItems = new ObservableCollection<EventItemViewModel>();
            this.TimeLineMenuItems = new ObservableCollection<EventItemViewModel>();
        }

        public ObservableCollection<EventItemViewModel> SquareFootEvents { get; private set; }
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
            ZoomedOutItemWidth = width / 6;
            ZoomedOutItemHeight = height / 4;
        }



        public Microsoft.Practices.Prism.StoreApps.DelegateCommand<EventItemViewModel> EventHeroItemClickCommand
        {
            get
            {
               throw new System.NotImplementedException();
            }
            set
            {
                throw new System.NotImplementedException();
            }
        }

        public void EventHeroItemClickCommandHandler(EventItemViewModel item)
        {
            throw new System.NotImplementedException();
        }

        double ITimelinePageViewModel.ZoomedOutItemHeight
        {
            get { throw new System.NotImplementedException(); }
        }

        double ITimelinePageViewModel.ZoomedOutItemWidth
        {
            get { throw new System.NotImplementedException(); }
        }

        double ITimelinePageViewModel.FullScreenItemWidth
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
                throw new System.NotImplementedException();
            }
        }

        double ITimelinePageViewModel.FullScreenItemHeight
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
                throw new System.NotImplementedException();
            }
        }

        ObservableCollection<EventItemViewModel> ITimelinePageViewModel.SquareFootEvents
        {
            get { throw new System.NotImplementedException(); }
        }

        ObservableCollection<EventItemViewModel> ITimelinePageViewModel.TimeLineItems
        {
            get { throw new System.NotImplementedException(); }
        }

        ObservableCollection<EventItemViewModel> ITimelinePageViewModel.TimeLineMenuItems
        {
            get { throw new System.NotImplementedException(); }
        }

        void ITimelinePageViewModel.WindowSizeChanged(double width, double height)
        {
            throw new System.NotImplementedException();
        }

        Microsoft.Practices.Prism.StoreApps.DelegateCommand<EventItemViewModel> ITimelinePageViewModel.EventHeroItemClickCommand
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
                throw new System.NotImplementedException();
            }
        }

        void ITimelinePageViewModel.EventHeroItemClickCommandHandler(EventItemViewModel item)
        {
            throw new System.NotImplementedException();
        }
    }
}