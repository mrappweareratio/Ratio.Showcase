﻿using System.Collections.Generic;
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

        public ObservableCollection<EventItemViewModel> TimeLineItems { get; private set; }
        public ObservableCollection<EventItemViewModel> TimeLineMenuItems { get; private set; }

        private double _zoomedOutItemWidth;
        private double _zoomedOutItemHeight;
        private double _fullScreenItemWidth;
        private double _fullScreenItemHeight;
        private double _fullScreenWidth;
        private double _fullScreenHeight;
        private double _eventItemHeight;
        private double _eventItemWidth;
        private double _exhibitItemWidth;
        private double _exhibitItemHeight;

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
            FullScreenHeight = height;
            FullScreenWidth = width;
            ZoomedOutItemWidth = width / 6;
            ZoomedOutItemHeight = height / 4;
            EventItemHeight = height;
            EventItemWidth = width * .9;
            ExhibitItemWidth = (EventItemWidth / 3) - 1;
            ExhibitItemHeight = (EventItemHeight / 4) - 1;
        }

        public double EventItemWidth
        {
            get { return _eventItemWidth; }
            set { SetProperty(ref _eventItemWidth, value); }
        }

        public double EventItemHeight
        {
            get { return _eventItemHeight; }
            set { SetProperty(ref _eventItemHeight, value); }
        }

        public double FullScreenHeight
        {
            get { return _fullScreenHeight; }
            set { SetProperty(ref _fullScreenHeight, value); }
        }

        public double FullScreenWidth
        {
            get { return _fullScreenWidth; }
            set { SetProperty(ref _fullScreenWidth, value); }
        }
        public double ExhibitItemWidth
        {
            get
            {
                return _exhibitItemWidth;
            }
            set
            {
                SetProperty(ref _exhibitItemWidth, value);
            }
        }
        public double ExhibitItemHeight
        {
            get
            {
                return _exhibitItemHeight;
            }
            set
            {
                SetProperty(ref _exhibitItemHeight, value);
            }
        }
        public DelegateCommand<EventItemViewModel> EventHeroItemClickCommand { get; set; }
        public void EventHeroItemClickCommandHandler(EventItemViewModel item)
        {
            throw new System.NotImplementedException();
        }
    }
}