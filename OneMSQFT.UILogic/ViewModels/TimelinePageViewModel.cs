using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;
using Microsoft.Practices.Prism.StoreApps;
using Microsoft.Practices.Prism.StoreApps.Interfaces;
using OneMSQFT.Common.Models;
using OneMSQFT.Common.Services;
using OneMSQFT.UILogic.Interfaces.ViewModels;
using OneMSQFT.UILogic.Navigation;

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
            this.SetStartupEventCommand = new DelegateCommand(SetStartupEventCommandExecuteMethod, SetStartupEventCommandCanExecuteMethod);
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
            TimeLineItems = new ObservableCollection<EventItemViewModel>(eventsList.Select(x => new EventItemViewModel(x)));
            TimeLineMenuItems = new ObservableCollection<EventItemViewModel>(eventsList.Select(x => new EventItemViewModel(x)));

            foreach (var eivm in eventsList)
            {
                _totalSquareFeet = _totalSquareFeet + eivm.SquareFootage;
            }
            AddComingSoonItems();
        }

        private void AddComingSoonItems()
        {
            if (TimeLineMenuItems.Count == 4)
            {
                TimeLineMenuItems.Insert(1, new ComingSoonFakeEventItemViewModel());
                TimeLineMenuItems.Insert(2, new ComingSoonFakeEventItemViewModel());
                return;
            }

            if (TimeLineMenuItems.Count == 6)
            {
                TimeLineMenuItems.Insert(2, new ComingSoonFakeEventItemViewModel());
                TimeLineMenuItems.Insert(2, new ComingSoonFakeEventItemViewModel());
                TimeLineMenuItems.Insert(6, new ComingSoonFakeEventItemViewModel());
                TimeLineMenuItems.Insert(8, new ComingSoonFakeEventItemViewModel());
                return;
            }


            if (TimeLineMenuItems.Count == 7)
            {
                TimeLineMenuItems.Insert(2, new ComingSoonFakeEventItemViewModel());
                TimeLineMenuItems.Insert(2, new ComingSoonFakeEventItemViewModel());
                TimeLineMenuItems.Insert(6, new ComingSoonFakeEventItemViewModel());
                TimeLineMenuItems.Insert(9, new ComingSoonFakeEventItemViewModel());
                TimeLineMenuItems.Insert(TimeLineMenuItems.Count-1, new ComingSoonFakeEventItemViewModel());
                return;
            }
        }

        public ObservableCollection<EventItemViewModel> TimeLineItems
        {
            get
            {
                if (((_timeLineItems[0] is BufferItemFakeEventItemViewModel)) &&
                    (_timeLineItems.Last() is BufferItemFakeEventItemViewModel))
                {
                    return _timeLineItems;
                }
                _timeLineItems.Insert(0, new BufferItemFakeEventItemViewModel());
                _timeLineItems.Add(new BufferItemFakeEventItemViewModel());
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
                    SetStartupEventCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public String SinceString
        {
            get
            {
                return "since December 2013";
            }
        }

        private int _totalSquareFeet;
        private ObservableCollection<EventItemViewModel> _timeLineItems;
        private ObservableCollection<EventItemViewModel> _timeLineMenuItems;

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
                return EventItemWidth /2;
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

        public DelegateCommand SetStartupEventCommand { get; set; }

        private void SetStartupEventCommandExecuteMethod()
        {
            _configuration.SetStartupEvent(SelectedEvent.Id);
        }

        protected bool SetStartupEventCommandCanExecuteMethod()
        {
            return SelectedEvent != null;
        }
    }
}