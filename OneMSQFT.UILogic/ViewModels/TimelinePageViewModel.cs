using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Windows.Media;
using Windows.UI.StartScreen;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;
using Microsoft.Practices.Prism.StoreApps;
using Microsoft.Practices.Prism.StoreApps.Interfaces;
using OneMSQFT.Common.Models;
using OneMSQFT.Common.Services;
using OneMSQFT.UILogic.Interfaces.ViewModels;
using OneMSQFT.UILogic.Navigation;
using OneMSQFT.UILogic.Utils;

namespace OneMSQFT.UILogic.ViewModels
{
    public class TimelinePageViewModel : BasePageViewModel, ITimelinePageViewModel
    {
        private readonly IDataService _dataService;
        private readonly IAlertMessageService _messageService;
        private readonly INavigationService _navigationService;
        private readonly IConfigurationService _configuration;
        public DelegateCommand<EventItemViewModel> EventHeroItemClickCommand { get; set; }
        public DelegateCommand<String> ExhibitItemClickCommand { get; set; }

        public TimelinePageViewModel(IDataService dataService, IAlertMessageService messageService, INavigationService navigationService, IConfigurationService configuration)
        {
            _dataService = dataService;
            _messageService = messageService;
            _navigationService = navigationService;
            _configuration = configuration;
            this.SquareFootEvents = new ObservableCollection<EventItemViewModel>();
            this.TimeLineItems = new ObservableCollection<EventItemViewModel>();
            this.TimeLineMenuItems = new ObservableCollection<EventItemViewModel>();
            this.EventHeroItemClickCommand = new DelegateCommand<EventItemViewModel>(EventHeroItemClickCommandHandler);
            this.ExhibitItemClickCommand = new DelegateCommand<String>(ExhibitItemClickCommandHandler);
            this.SetStartupCommand = new DelegateCommand(SetStartupCommandExecuteMethod, SetStartupCommandCanExecuteMethod);
            this.ClearStartupCommand = new DelegateCommand(ClearStartupCommandExecuteMethod, ClearStartupCommandCanExecuteMethod);
            SetStartupVisibility = SetStartupCommand.CanExecute() ? Visibility.Visible : Visibility.Collapsed;
            ClearStartupVisibility = ClearStartupCommand.CanExecute() ? Visibility.Visible : Visibility.Collapsed;
        }

        public override async void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            var events = await _dataService.GetEvents();
            if (events == null)
            {
                await _messageService.ShowAsync("Error", "There was a problem loading events");
                return;
            }
            var eventsList = events as IList<Event> ?? events.ToList();

            SquareFootEvents = new ObservableCollection<EventItemViewModel>(eventsList.Select(x => new EventItemViewModel(x)));

            var timelineEvents = eventsList.Select(x => new EventItemViewModel(x)).ToList();
            timelineEvents.Insert(0, new BufferItemFakeEventItemViewModel()); // first buffer item
            timelineEvents.Add(new BufferItemFakeEventItemViewModel()); // last buffer item
            TimeLineItems = new ObservableCollection<EventItemViewModel>(timelineEvents);

            var timelineMenuEvents = eventsList.Select(x => new EventItemViewModel(x)).ToList();
            timelineMenuEvents = ComingSoonUtils.InsertComingSoonItems(12, timelineMenuEvents);
            TimeLineMenuItems = new ObservableCollection<EventItemViewModel>(timelineMenuEvents);

            foreach (var eivm in eventsList)
            {
                _totalSquareFeet = _totalSquareFeet + eivm.SquareFootage;
            }
        }

        public ObservableCollection<EventItemViewModel> TimeLineItems
        {
            get
            {
                return _timeLineItems;
            }
            private set { SetProperty(ref _timeLineItems, value); }
        }

        public ObservableCollection<EventItemViewModel> TimeLineMenuItems
        {
            get { return _timeLineMenuItems; }
            private set { SetProperty(ref _timeLineMenuItems, value); }
        }

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
                    RaisePinContextChanged();
                    SetStartupCommand.RaiseCanExecuteChanged();
                    SetStartupVisibility = SetStartupCommand.CanExecute() ? Visibility.Visible : Visibility.Collapsed;
                    ClearStartupCommand.RaiseCanExecuteChanged();
                    ClearStartupVisibility = ClearStartupCommand.CanExecute() ? Visibility.Visible : Visibility.Collapsed;

                }
            }
        }

        private int _totalSquareFeet;
        private ObservableCollection<EventItemViewModel> _timeLineItems;
        private ObservableCollection<EventItemViewModel> _timeLineMenuItems;
        private Visibility _clearStartupVisibility;
        private Visibility _setStartupVisibility;

        public String TotalSquareFeet
        {
            get
            {
                return (String.Format(CultureInfo.InvariantCulture, "{0:# ### ###}", _totalSquareFeet)).Trim() + " sqft";
            }
        }

        #region ResizingProperties

        public double ZoomedOutGridHeight
        {
            get
            {
                return FullScreenHeight;
            }
        }

        public double ZoomedOutItemWidth
        {
            get
            {
                return (FullScreenWidth - 44) / 4; // 42 is GridView padding
            }
        }
        public double ZoomedOutItemHeight
        {
            get
            {
                return ((ZoomedOutGridHeight - 165) / 3) - 18;
            }
        }


        public double EventItemWidth
        {
            get
            {
                return FullScreenWidth * .9;
            }
        }

        public double BufferItemWidth
        {
            get
            {
                return EventItemWidth / 2;
            }
        }

        public double MaskItemWidth
        {
            get
            {
                return ((FullScreenWidth - EventItemWidth) / 2) + 1;
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
                return (EventItemWidth / 3) - 9;
            }
        }
        public double ExhibitItemHeight
        {
            get
            {
                return (EventItemHeight / 4) - 6;
            }
        }


        public override void WindowSizeChanged(double width, double height)
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
            OnPropertyChanged("MaskItemWidth");
            OnPropertyChanged("BufferItemWidth");
        }

        #endregion


        public void EventHeroItemClickCommandHandler(EventItemViewModel item)
        {
            if (item == null) return;
            SelectedEvent = item;
        }

        public void ExhibitItemClickCommandHandler(String itemId)
        {
            _navigationService.Navigate(ViewLocator.Pages.ExhibitDetails, itemId);
        }


        public Visibility ClearStartupVisibility
        {
            get { return _clearStartupVisibility; }
            set { SetProperty(ref _clearStartupVisibility, value); }
        }

        public Visibility SetStartupVisibility
        {
            get { return _setStartupVisibility; }
            set { SetProperty(ref _setStartupVisibility, value); }
        }

        public DelegateCommand SetStartupCommand { get; set; }

        private void SetStartupCommandExecuteMethod()
        {
            _configuration.SetStartupEvent(SelectedEvent.Id);
            SetStartupCommand.RaiseCanExecuteChanged();
            SetStartupVisibility = SetStartupCommand.CanExecute() ? Visibility.Visible : Visibility.Collapsed;
            ClearStartupCommand.RaiseCanExecuteChanged();
            ClearStartupVisibility = ClearStartupCommand.CanExecute() ? Visibility.Visible : Visibility.Collapsed;
        }

        protected bool SetStartupCommandCanExecuteMethod()
        {
            return SelectedEvent != null && !(_configuration.StartupItemType == StartupItemType.Event && SelectedEvent.Id.Equals(_configuration.StartupItemId));
        }

        public DelegateCommand ClearStartupCommand { get; private set; }

        private bool ClearStartupCommandCanExecuteMethod()
        {
            return SelectedEvent != null && _configuration.StartupItemType == StartupItemType.Event && SelectedEvent.Id.Equals(_configuration.StartupItemId);
        }

        public void ClearStartupCommandExecuteMethod()
        {
            _configuration.ClearStartupItem();
            SetStartupCommand.RaiseCanExecuteChanged();
            SetStartupVisibility = SetStartupCommand.CanExecute() ? Visibility.Visible : Visibility.Collapsed;
            ClearStartupCommand.RaiseCanExecuteChanged();
            ClearStartupVisibility = ClearStartupCommand.CanExecute() ? Visibility.Visible : Visibility.Collapsed;
        }

        public override Task<SecondaryTileImages> GetSecondaryTileImages()
        {
            return Task.FromResult<SecondaryTileImages>(new SecondaryTileImages());
        }

        public override SecondaryTileArgs GetSecondaryTileArguments()
        {
            return new SecondaryTileArgs()
            {
                Id = PinningUtils.GetSecondaryTileIdByEventId(SelectedEvent.Id),
                ArgumentsName = PinningUtils.GetSecondaryTileIdByEventId(SelectedEvent.Id),
                DisplayName = SelectedEvent.Name,
                ShortName = SelectedEvent.Name,
                BackgroundColor = SelectedEvent.EventColor.ToString(),
            };
        }
    }
}