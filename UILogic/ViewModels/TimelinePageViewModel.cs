using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Windows.Graphics.Display;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;
using Microsoft.Practices.Prism.StoreApps;
using Microsoft.Practices.Prism.StoreApps.Interfaces;
using Ratio.Showcase.Shared;
using Ratio.Showcase.Shared.Models;
using Ratio.Showcase.Shared.Services;
using Ratio.Showcase.UILogic.Interfaces.ViewModels;
using Ratio.Showcase.UILogic.Navigation;
using Ratio.Showcase.UILogic.Utils;

namespace Ratio.Showcase.UILogic.ViewModels
{
    public class TimelinePageViewModel : BasePageViewModel, ITimelinePageViewModel
    {
        private readonly IDataService _dataService;
        private readonly IAnalyticsService _analyticsService;
        private readonly IDispatcherService _dispatcherService;
        private readonly IAlertMessageService _messageService;
        private readonly INavigationService _navigationService;
        private readonly IConfigurationService _configuration;
        public DelegateCommand<EventItemViewModel> EventHeroItemClickCommand { get; set; }
        public DelegateCommand<ExhibitItemViewModel> ExhibitItemClickCommand { get; set; }

        public TimelinePageViewModel(IDataService dataService, IAlertMessageService messageService, INavigationService navigationService, IConfigurationService configuration, IAnalyticsService analyticsService)
        {
            _dataService = dataService;
            _messageService = messageService;
            _navigationService = navigationService;
            _configuration = configuration;
            _analyticsService = analyticsService;
            this.SquareFootEvents = new ObservableCollection<EventItemViewModel>();
            this.TimeLineItems = new ObservableCollection<EventItemViewModel>();
            this.TimeLineMenuItems = new ObservableCollection<EventItemViewModel>();
            this.EventHeroItemClickCommand = new DelegateCommand<EventItemViewModel>(EventHeroItemClickCommandHandler);
            this.ExhibitItemClickCommand = new DelegateCommand<ExhibitItemViewModel>(ExhibitItemClickCommandHandler);
            this.SetStartupCommand = new DelegateCommand(SetStartupCommandExecuteMethod, SetStartupCommandCanExecuteMethod);
            this.ClearStartupCommand = new DelegateCommand(ClearStartupCommandExecuteMethod, ClearStartupCommandCanExecuteMethod);
            SetStartupVisibility = SetStartupCommand.CanExecute() ? Visibility.Visible : Visibility.Collapsed;
            ClearStartupVisibility = ClearStartupCommand.CanExecute() ? Visibility.Visible : Visibility.Collapsed;
            LoadedEventsTaskCompletionSource = new TaskCompletionSource<bool>();
        }
        
        public override async void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            //Events = await _dataService.GetEvents(new CancellationToken()).TryCatchAsync();
            Events = await Task.Run(() => _dataService.GetEvents(new CancellationToken()).TryCatchAsync());
            if (Events == null)
            {
                await _messageService.ShowAsync(Strings.SiteDataFailureMessage, String.Empty);
                return;
            }
            _eventsList = Events as IList<Event> ?? Events.ToList();

            SquareFootEvents = new ObservableCollection<EventItemViewModel>(_eventsList.Select(x => new EventItemViewModel(x, _analyticsService)));

            var timelineEvents = _eventsList.Select(x => new EventItemViewModel(x, _analyticsService)).ToList();
            timelineEvents.Insert(0, new BufferItemFakeEventItemViewModel()); // first buffer item
            timelineEvents.Add(new BufferItemFakeEventItemViewModel()); // last buffer item
            TimeLineItems = new ObservableCollection<EventItemViewModel>(timelineEvents);            

            var timelineMenuEvents = _eventsList.Select(x => new EventItemViewModel(x, _analyticsService)).ToList();
            timelineMenuEvents = ComingSoonUtils.InsertComingSoonItems(12, timelineMenuEvents);
            TimeLineMenuItems = new ObservableCollection<EventItemViewModel>(timelineMenuEvents);            
            
            LoadedEventsTaskCompletionSource.TrySetResult(true);
        }

        /// <summary>
        /// TimeLineItems Bound To Events List
        /// Contains EventItemViewModel and BufferItemFakeEventItemViewModel
        /// </summary>
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

        private string _totalSquareFeet;
        private ObservableCollection<EventItemViewModel> _timeLineItems;
        private ObservableCollection<EventItemViewModel> _timeLineMenuItems;
        private Visibility _clearStartupVisibility;
        private Visibility _setStartupVisibility;
        private IList<Event> _eventsList;        

        public String TotalSquareFeet
        {
            get { return _totalSquareFeet; }
            set { SetProperty(ref _totalSquareFeet, value); }
        }

        #region Resizing Properties

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

        public double BufferItemWidth
        {
            get { return IsHorizontal ? EventItemWidth / 2 : FullScreenWidth; }
        }

        public double BufferItemHeight
        {
            get { return IsHorizontal ? FullScreenHeight : FullScreenHeight / 4; }
        }

        public double MaskItemWidth
        {
            get
            {
                return IsHorizontal
                    ? ((FullScreenWidth - EventItemWidth) / 2) + 1
                    : FullScreenWidth;
            }
        }

        public double MaskItemHeight
        {
            get
            {
                return IsHorizontal
                    ? FullScreenHeight
                    : ((FullScreenHeight - EventItemHeight) / 2) + 1;
            }
        }

        public double EventItemWidth
        {
            get
            {
                return IsHorizontal
                    ? FullScreenWidth * .9
                    : FullScreenWidth;
            }
        }

        public double EventItemHeight
        {
            get
            {
                return IsHorizontal
                    ? FullScreenHeight
                    : FullScreenHeight * .9;
            }
        }

        public double ExhibitItemWidth
        {
            get
            {
                return IsHorizontal
                    ? (EventItemWidth / 3) - 9
                    : (EventItemWidth / 2) - 9;
            }
        }

        public double ExhibitItemHeight
        {
            get
            {
                return IsHorizontal
                    ? (EventItemHeight / 4) - 6
                    : (EventItemHeight / 6) - 6;
            }
        }

        public double LogoTransformX
        {
            get
            {
                return IsHorizontal
                    ? GetHLogoX()
                    : GetVLogoX();
            }
        }

        private int GetHLogoX()
        {
            var dpi = DisplayInformation.GetForCurrentView().LogicalDpi;
            var delta = Convert.ToDouble(WidthDelta);
            if (dpi <= 96.0)
            {
                if (delta < 0.54)
                {
                    return 180; // 27" @ 100%
                }
                if (delta < 0.72)
                {
                    return 146; // 23" @ 100%
                }
                if (delta == 1.0)
                {
                    return 110; // 10.6" 1366x768 @ 100%
                }
                if (delta < 1.1)
                {
                    return 110; // 12" @ 100%
                }
                if (delta < 1.35)
                {
                    return 96; // 10.6" 1024x768 @ 100%
                }
                return 110;
            }
            if (dpi <= 140.0)
            {
                if (delta < 1.0)
                {
                    return 112; // 10.6" 1920x1080 @ 140%
                    //    7" 1920x1200 @ 140%
                }
                if (delta < 1.35)
                {
                    return 96; // 7.5" 1044x1080 @ 140%
                }
                return 112;
            }
            if (dpi <= 180.0)
            {
                if (delta < 1.0)
                {
                    return 110; // 10.6" 2560x1440 @ 180%
                }
                return 110;
            }
            return 110;
        }
        private int GetVLogoX()
        {
            var dpi = DisplayInformation.GetForCurrentView().LogicalDpi;
            var delta = Convert.ToDouble(HeightDelta);
            if (dpi <= 96.0)
            {
                //if (delta < 0.54)
                //{
                //    return 180; // 27" @ 100%
                //}
                //if (delta < 0.72)
                //{
                //    return 146; // 23" @ 100%
                //}
                //if (delta == 1.0)
                //{
                //    return 110; // 10.6" 1366x768 @ 100%
                //}
                //if (delta < 1.1)
                //{
                //    return 110; // 12" @ 100%
                //}
                //if (delta < 1.35)
                //{
                //    return 96; // 10.6" 1024x768 @ 100%
                //}
                return 50;
            }
            if (dpi <= 140.0)
            {
                //if (delta < 1.0)
                //{
                //    return 112; // 10.6" 1920x1080 @ 140%
                //    //    7" 1920x1200 @ 140%
                //}
                //if (delta < 1.35)
                //{
                //    return 96; // 7.5" 1044x1080 @ 140%
                //}
                return 50;
            }
            if (dpi <= 180.0)
            {
                //if (delta < 1.0)
                //{
                //    return 110; // 10.6" 2560x1440 @ 180%
                //}
                return 50;
            }
            return 50;
        }

        public double LogoTransformY
        {
            get
            {
                return IsHorizontal
                    ? GetHLogoY()
                    : GetVLogoY();
            }
        }
        private int GetHLogoY()
        {
            var dpi = DisplayInformation.GetForCurrentView().LogicalDpi;
            var delta = Convert.ToDouble(WidthDelta);
            if (dpi <= 96.0)
            {
                if (delta < 0.54)
                {
                    return 46; // 27" @ 100%
                }
                if (delta < 0.72)
                {
                    return 40; // 23" @ 100%
                }
                if (delta == 1.0)
                {
                    return 40; // 10.6" 1366x768 @ 100%
                }
                if (delta < 1.1)
                {
                    return 40; // 12" @ 100%
                }
                if (delta < 1.35)
                {
                    return 40; // 10.6" 1024x768 @ 100%
                }
                return 40;
            }
            if (dpi <= 140.0)
            {
                if (delta < 1.0)
                {
                    return 40; // 10.6" 1920x1080 @ 140%
                    //    7" 1920x1200 @ 140%
                }
                if (delta < 1.35)
                {
                    return 40; // 7.5" 1044x1080 @ 140%
                }
                return 40;
            }
            if (dpi <= 180.0)
            {
                if (delta < 1.0)
                {
                    return 40; // 10.6" 2560x1440 @ 180%
                }
                return 40;
            }
            return 40;
        }
        private int GetVLogoY()
        {
            var dpi = DisplayInformation.GetForCurrentView().LogicalDpi;
            var delta = Convert.ToDouble(HeightDelta);
            if (dpi <= 96.0)
            {
                if (delta < 0.54)
                {
                    return 190; // 27" @ 100%
                }
                if (delta < 0.72)
                {
                    return 145; // 23" @ 100%
                }
                if (delta == 1.0)
                {
                    return 100; // 10.6" 1366x768 @ 100%
                }
                if (delta < 1.1)
                {
                    return 100; // 12" @ 100%
                }
                if (delta < 1.35)
                {
                    return 70; // 10.6" 1024x768 @ 100%
                }
                return 70;
            }
            if (dpi <= 140.0)
            {
                if (delta < 1.0)
                {
                    return 100; // 10.6" 1920x1080 @ 140%
                    //    7" 1920x1200 @ 140%
                }
                if (delta < 1.35)
                {
                    return 70; // 7.5" 1044x1080 @ 140%
                }
                return 70;
            }
            if (dpi <= 180.0)
            {
                if (delta < 1.0)
                {
                    return 100; // 10.6" 2560x1440 @ 180%
                }
                return 70;
            }
            return 100;
        }

        public double LogoScale
        {
            get
            {
                return IsHorizontal
                    ? GetHLogoScale()
                    : GetVLogoScale();
            }
        }

        private double GetHLogoScale()
        {
            var dpi = DisplayInformation.GetForCurrentView().LogicalDpi;
            var delta = Convert.ToDouble(WidthDelta);
            if (dpi <= 96.0)
            {
                if (delta < 0.54)
                {
                    return 2.1; // 27" @ 100%
                }
                if (delta < 0.72)
                {
                    return 1.5; // 23" @ 100%
                }
                if (delta == 1.0)
                {
                    return 1; // 10.6" 1366x768 @ 100%
                }
                if (delta < 1.1)
                {
                    return 1; // 12" @ 100%
                }
                if (delta < 1.35)
                {
                    return 1; // 10.6" 1024x768 @ 100%
                }
                return 1;
            }
            if (dpi <= 140.0)
            {
                if (delta < 1.0)
                {
                    return 1.1; // 10.6" 1920x1080 @ 140%
                    //    7" 1920x1200 @ 140%
                }
                if (delta < 1.35)
                {
                    return 1.1; // 7.5" 1044x1080 @ 140%
                }
                return 1.1;
            }
            if (dpi <= 180.0)
            {
                if (delta < 1.0)
                {
                    return 1.0; // 10.6" 2560x1440 @ 180%
                }
                return 1.0;
            }
            return 1.0;
        }
        private double GetVLogoScale()
        {
            var dpi = DisplayInformation.GetForCurrentView().LogicalDpi;
            var delta = Convert.ToDouble(HeightDelta);
            if (dpi <= 96.0)
            {
                if (delta < 0.54)
                {
                    return 2.1; // 27" @ 100%
                }
                if (delta < 0.72)
                {
                    return 1.5; // 23" @ 100%
                }
                if (delta == 1.0)
                {
                    return 1; // 10.6" 1366x768 @ 100%
                }
                if (delta < 1.1)
                {
                    return 1; // 12" @ 100%
                }
                if (delta < 1.35)
                {
                    return .9; // 10.6" 1024x768 @ 100%
                }
                return .5;
            }
            if (dpi <= 140.0)
            {
                if (delta < 1.0)
                {
                    return 1.1; // 10.6" 1920x1080 @ 140%
                                //    7" 1920x1200 @ 140%
                }
                if (delta < 1.35)
                {
                    return .9; // 7.5" 1044x1080 @ 140%
                }
                return .5;
            }
            if (dpi <= 180.0)
            {
                if (delta < 1.2)
                {
                    return 1.2; // 10.6" 2560x1440 @ 180%
                }
                return .5;
            }
            return 1;
        }


        public override void WindowSizeChanged(double width, double height)
        {
            base.WindowSizeChanged(width, height);
            OnPropertyChanged("ZoomedOutGridHeight");
            OnPropertyChanged("ZoomedOutItemWidth");
            OnPropertyChanged("ZoomedOutItemHeight");
            OnPropertyChanged("BufferItemWidth");
            OnPropertyChanged("MaskItemWidth");
            OnPropertyChanged("MaskItemHeight");
            OnPropertyChanged("EventItemWidth");
            OnPropertyChanged("EventItemHeight");
            OnPropertyChanged("ExhibitItemWidth");
            OnPropertyChanged("ExhibitItemHeight");
            OnPropertyChanged("LogoTransformX");
            OnPropertyChanged("LogoTransformY");
            OnPropertyChanged("LogoScale");
        }

        #endregion


        public void EventHeroItemClickCommandHandler(EventItemViewModel item)
        {
            if (item == null) return;
            SelectedEvent = item;
        }

        public void ExhibitItemClickCommandHandler(ExhibitItemViewModel exhibitItemViewModel)
        {
            //Track Exhibit user interaction            
            var ev = SelectedEvent;
            if (ev != null && _analyticsService != null)
            {
                var evPos = GetEventIndexById(ev.Id);
                _analyticsService.TrackExhibitInteractionInTimeline(
                    ev.Name,
                    exhibitItemViewModel.Name,
                    evPos.GetValueOrDefault(-1),
                    ev.Exhibits.Contains(exhibitItemViewModel) ? ev.Exhibits.IndexOf(exhibitItemViewModel) : -1);
            }

            _navigationService.Navigate(ViewLocator.Pages.ExhibitDetails, exhibitItemViewModel.Id);
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