using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml;
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

        public ObservableCollection<EventItemViewModel> TimeLineItems { get; private set; }
        public ObservableCollection<EventItemViewModel> TimeLineMenuItems { get; private set; }

        private EventItemViewModel _selectedEvent;
        public EventItemViewModel SelectedEvent
        {
            get
            {
                return _selectedEvent;
            }
            set
            {
                if (value != null)
                {
                    SetProperty(ref _selectedEvent, value);
                }
            }
        }

        #region ResizingProperties

        public double ZoomedOutGridHeight
        {
            get
            {
                return FullScreenHeight * .75;
            }
        }

        public double ZoomedOutItemWidth
        {
            get
            {
                return FullScreenWidth / 6;
            }
        }
        public double ZoomedOutItemHeight
        {
            get
            {
                return FullScreenHeight / 4;
            }
        }

        public double EventItemWidth
        {
            get
            {
                return FullScreenWidth * .9;
            }
        }
        public double EventItemHeight
        {
            get
            {
                return FullScreenHeight;
            }
        }

        public double FullScreenWidth { get; set; }

        public double FullScreenHeight { get; set; }

        public double ExhibitItemWidth
        {
            get
            {
                return (EventItemWidth / 3) - 1;
            }
        }
        public double ExhibitItemHeight
        {
            get
            {
                return (EventItemHeight / 4) - 1;
            }
        }


        public void WindowSizeChanged(double width, double height)
        {
            FullScreenHeight = height;
            FullScreenWidth = width;
            OnPropertyChanged("ZoomedOutItemWidth");
            OnPropertyChanged("ZoomedOutItemHeight");
            OnPropertyChanged("EventItemHeight");
            OnPropertyChanged("EventItemWidth");
            OnPropertyChanged("ExhibitItemWidth");
            OnPropertyChanged("ExhibitItemHeight");
            OnPropertyChanged("ZoomedOutGridHeight");
        }

        #endregion

        public DelegateCommand<EventItemViewModel> EventHeroItemClickCommand { get; set; }

        public void EventHeroItemClickCommandHandler(EventItemViewModel item)
        {
            if (item == null) return;
            SelectedEvent = item;
        }
    }
}