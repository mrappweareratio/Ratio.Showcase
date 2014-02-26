using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Microsoft.Practices.Prism.StoreApps;
using Microsoft.Practices.Prism.StoreApps.Interfaces;
using OneMSQFT.Common;
using OneMSQFT.Common.Models;
using OneMSQFT.UILogic.Interfaces.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Core;
using OneMSQFT.Common.Services;
using System;
using OneMSQFT.UILogic.Navigation;
using OneMSQFT.UILogic.Utils;

namespace OneMSQFT.UILogic.ViewModels
{
    public class ExhibitDetailsPageViewModel : BasePageViewModel, IExhibitDetailsPageViewModel
    {
        public DelegateCommand<MediaContentSourceItemViewModel> LaunchVideoCommand { get; set; }
        public DelegateCommand<string> NextExhibitCommand { get; private set; }
        public DelegateCommand SetStartupCommand { get; private set; }
        public DelegateCommand ClearStartupCommand { get; private set; }
        private readonly IDataService _dataService;
        private readonly IAlertMessageService _messageService;
        private readonly INavigationService _navigationService;
        private readonly IConfigurationService _configuration;

        public ExhibitDetailsPageViewModel(IDataService dataService, IAlertMessageService messageService, INavigationService navigationService, IConfigurationService configuration)
        {
            _dataService = dataService;
            _messageService = messageService;
            _navigationService = navigationService;
            _configuration = configuration;
            SquareFootEvents = new ObservableCollection<EventItemViewModel>();
            LaunchVideoCommand = new DelegateCommand<MediaContentSourceItemViewModel>(LaunchVideoCommandHandler);
            NextExhibitCommand = new DelegateCommand<string>(NextExhibitCommandExecuteMethod, NextExhibitCommandCanExecuteMethod);
            SetStartupCommand = new DelegateCommand(SetStartupCommandExecuteMethod, SetStartupCommandCanExecuteMethod);
            ClearStartupCommand = new DelegateCommand(ClearStartupCommandExecuteMethod, ClearStartupCommandCanExecuteMethod);
            SetStartupVisibility = SetStartupCommand.CanExecute() ? Visibility.Visible : Visibility.Collapsed;
            ClearStartupVisibility = ClearStartupCommand.CanExecute() ? Visibility.Visible : Visibility.Collapsed;
        }

        private bool NextExhibitCommandCanExecuteMethod(string s)
        {
            return NextExhibit != null;
        }

        private void NextExhibitCommandExecuteMethod(string exhibitId)
        {
            _navigationService.Navigate(ViewLocator.Pages.ExhibitDetails, exhibitId);
        }

        private ExhibitItemViewModel _nextExhibit;
        public ExhibitItemViewModel NextExhibit
        {
            get { return _nextExhibit; }
            set
            {
                SetProperty(ref _nextExhibit, value);
                NextExhibitCommand.RaiseCanExecuteChanged();
            }
        }

        async public override void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            var events = await _dataService.GetEvents();
            if (events == null)
            {
                await _messageService.ShowAsync("Error", "There was a problem loading events");
                return;
            }
            SquareFootEvents = new ObservableCollection<EventItemViewModel>(events.Select(x => new EventItemViewModel(x)));

            var ed = await _dataService.GetExhibitDetailByExhibitId(navigationParameter as String);
            Exhibit = new ExhibitItemViewModel(ed.Exhibit);
            NextExhibit = ed.NextExhibit == null ? null : new ExhibitItemViewModel(ed.NextExhibit);

            LocalMediaCollection = Exhibit.MediaContent;
            LocalMediaCollection.Add(new FooterFakeMediaItemViewModel());
            LocalMediaCollection.Insert(0, new HeaderFakeMediaItemViewModel());

            base.OnNavigatedTo(navigationParameter, navigationMode, viewModelState);
        }

        public void LaunchVideoCommandHandler(MediaContentSourceItemViewModel item)
        {
            if (item == null) return;
            SelectedMediaContentSource = item;
        }

        #region ContentProperties

        private MediaContentSourceItemViewModel _selectedMediaContentSource;
        public MediaContentSourceItemViewModel SelectedMediaContentSource
        {
            get
            {
                return _selectedMediaContentSource;
            }
            set
            {
                if (value != null)
                {
                    SetProperty(ref _selectedMediaContentSource, value);
                }
            }
        }

        private ExhibitItemViewModel _exhibit;
        private string _exhibitDetailTitle;
        private Visibility _setStartupVisibility;
        private Visibility _clearStartupVisibility;

        private ObservableCollection<MediaContentSourceItemViewModel> _localMediaCollection;
        public ObservableCollection<MediaContentSourceItemViewModel> LocalMediaCollection
        {
            get
            {
                return _localMediaCollection;
            }
            set
            {
                if (value != null)
                {
                    SetProperty(ref _localMediaCollection, value);
                }
            }
        }

        public ExhibitItemViewModel Exhibit
        {
            get
            {
                return _exhibit;
            }
            set
            {
                if (value != null)
                {
                    SetProperty(ref _exhibit, value);
                    ExhibitDetailTitle = String.Format(Strings.SquareFeetAtNameFormat, StringUtils.ToSquareFeet(Exhibit.SquareFootage), value.Name);
                    RaisePinContextChanged();
                    SetStartupCommand.RaiseCanExecuteChanged();
                    SetStartupVisibility = SetStartupCommand.CanExecute() ? Visibility.Visible : Visibility.Collapsed;
                    ClearStartupCommand.RaiseCanExecuteChanged();
                    ClearStartupVisibility = ClearStartupCommand.CanExecute() ? Visibility.Visible : Visibility.Collapsed;
                    
                }
            }
        }

        public String ExhibitDetailTitle
        {
            get { return _exhibitDetailTitle; }
            set { SetProperty(ref _exhibitDetailTitle, value); }
        }

        #endregion

        #region ResizingProperties

        public double FullScreenWidth
        {
            get
            {
                return Window.Current.Bounds.Width;
            }
        }
        public double FullScreenHeight
        {
            get
            {
                return Window.Current.Bounds.Height;
            }
        }

        public double OneThirdPanelWidth
        {
            get { return FullScreenWidth * .325; } // approx one third (per comp)
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

        public override void WindowSizeChanged(double width, double height)
        {
            OnPropertyChanged("FullScreenHeight");
            OnPropertyChanged("FullScreenWidth");
            OnPropertyChanged("ExhibitItemHeight");
            OnPropertyChanged("ExhibitItemWidth");
        }

        #endregion

        private bool ClearStartupCommandCanExecuteMethod()
        {
            return Exhibit != null && _configuration.StartupItemType == StartupItemType.Exhibit && Exhibit.Id.Equals(_configuration.StartupItemId);
        }

        private void ClearStartupCommandExecuteMethod()
        {
            _configuration.ClearStartupItem();
            SetStartupCommand.RaiseCanExecuteChanged();
            SetStartupVisibility = SetStartupCommand.CanExecute() ? Visibility.Visible : Visibility.Collapsed;
            ClearStartupCommand.RaiseCanExecuteChanged();
            ClearStartupVisibility = ClearStartupCommand.CanExecute() ? Visibility.Visible : Visibility.Collapsed;
        }

        private bool SetStartupCommandCanExecuteMethod()
        {
            if (Exhibit == null)
                return false;
            if (_configuration.StartupItemType == StartupItemType.None)
                return true;
            if (_configuration.StartupItemType == StartupItemType.Event)
                return true;
            return !_configuration.StartupItemId.Equals(Exhibit.Id);
        }

        private void SetStartupCommandExecuteMethod()
        {
            _configuration.SetStartupExhibit(Exhibit.Id);
            SetStartupCommand.RaiseCanExecuteChanged();
            SetStartupVisibility = SetStartupCommand.CanExecute() ? Visibility.Visible : Visibility.Collapsed;
            ClearStartupCommand.RaiseCanExecuteChanged();
            ClearStartupVisibility = ClearStartupCommand.CanExecute() ? Visibility.Visible : Visibility.Collapsed;
        }

        #region Pinning

        public override SecondaryTileArgs GetSecondaryTileArguments()
        {
            if (Exhibit == null)
                return null;
            return new SecondaryTileArgs()
            {
                Id = PinningUtils.GetSecondaryTileIdByExhibitId(Exhibit.Id),
                ArgumentsName = PinningUtils.GetSecondaryTileIdByExhibitId(Exhibit.Id),
                ShortName = Exhibit.Name,
                DisplayName = ExhibitDetailTitle
            };
        }

        #endregion
    }
}