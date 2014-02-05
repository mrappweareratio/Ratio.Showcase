using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;
using Microsoft.Practices.Prism.StoreApps;
using OneMSQFT.Common.Services;
using OneMSQFT.UILogic.Interfaces.ViewModels;

namespace OneMSQFT.UILogic.ViewModels
{
    public class TimelinePageViewModel : BasePageViewModel, ITimelinePageViewModel
    {
        public TaskCompletionSource<bool> LoadingTaskCompletionSource { get; set; }
        private readonly IDataService _dataService;
        private readonly IAlertMessageService _messageService;

        public TimelinePageViewModel(IDataService dataService, IAlertMessageService messageService)
        {
            _dataService = dataService;
            _messageService = messageService;
            this.SquareFootEvents = new ObservableCollection<EventItemViewModel>();
            this.TimeLineItems = new ObservableCollection<EventItemViewModel>();
            this.TimeLineMenuItems = new ObservableCollection<EventItemViewModel>();
            this.EventHeroItemClickCommand = new DelegateCommand<EventItemViewModel>(EventHeroItemClickCommandHandler);
        }

        public override async void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            LoadingTaskCompletionSource = new TaskCompletionSource<bool>();
            try
            {
                var events = await _dataService.GetEvents();
                if (events == null)
                {
                    await _messageService.ShowAsync("Error", "There was a problem loading events");
                    return;
                }
                SquareFootEvents = new ObservableCollection<EventItemViewModel>(events.Select(x => new EventItemViewModel(x)));
                TimeLineItems = new ObservableCollection<EventItemViewModel>(events.Select(x => new EventItemViewModel(x)));
                TimeLineMenuItems = new ObservableCollection<EventItemViewModel>(events.Select(x => new EventItemViewModel(x)));
            }
            finally
            {
                LoadingTaskCompletionSource.SetResult(true);
            }

        }

        public ObservableCollection<EventItemViewModel> SquareFootEvents { get; private set; }
        public ObservableCollection<EventItemViewModel> SquareFootPastEvents { get; private set; }
        public ObservableCollection<EventItemViewModel> TimeLineItems { get; private set; }
        public ObservableCollection<EventItemViewModel> TimeLineMenuItems { get; private set; }

        private double _zoomedOutItemWidth;
        private double _zoomedOutItemHeight;
        private double _fullScreenItemWidth;
        private double _fullScreenItemHeight;

        public double ZoomedOutItemWidth
        {
            get
            {
                return _zoomedOutItemWidth;
            }
            set
            {
                if (value != null)
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

        public double FullScreenItemWidth
        {
            get { return _fullScreenItemWidth; }
            set { SetProperty(ref _fullScreenItemWidth, value); }
        }

        public double FullScreenItemHeight
        {
            get { return _fullScreenItemHeight; }
            set { SetProperty(ref _fullScreenItemHeight, value); }
        }

        public void WindowSizeChanged(double width, double height)
        {
            ZoomedOutItemWidth = width / 6;
            ZoomedOutItemHeight = height / 4;
            FullScreenItemHeight = height;
            FullScreenItemWidth = width - 100;
        }

        public Microsoft.Practices.Prism.StoreApps.DelegateCommand<EventItemViewModel> EventHeroItemClickCommand { get; set; }        

        public void EventHeroItemClickCommandHandler(EventItemViewModel item)
        {
            throw new System.NotImplementedException();
        }
    }
}