﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
        private readonly IAnalyticsService _analyticsService;

        public ExhibitDetailsPageViewModel(IDataService dataService, IAlertMessageService messageService, INavigationService navigationService, IConfigurationService configuration, IAnalyticsService analyticsService)
        {
            _dataService = dataService;
            _messageService = messageService;
            _navigationService = navigationService;
            _configuration = configuration;
            _analyticsService = analyticsService;
            SquareFootEvents = new ObservableCollection<EventItemViewModel>();
            LaunchVideoCommand = new DelegateCommand<MediaContentSourceItemViewModel>(LaunchVideoCommandHandler);
            NextExhibitCommand = new DelegateCommand<string>(NextExhibitCommandExecuteMethod, NextExhibitCommandCanExecuteMethod);
            SetStartupCommand = new DelegateCommand(SetStartupCommandExecuteMethod, SetStartupCommandCanExecuteMethod);
            ClearStartupCommand = new DelegateCommand(ClearStartupCommandExecuteMethod, ClearStartupCommandCanExecuteMethod);
            SetStartupVisibility = SetStartupCommand.CanExecute() ? Visibility.Visible : Visibility.Collapsed;
            ClearStartupVisibility = ClearStartupCommand.CanExecute() ? Visibility.Visible : Visibility.Collapsed;
            LoadedEventsTaskCompletionSource = new TaskCompletionSource<bool>();
        }

        private bool NextExhibitCommandCanExecuteMethod(string s)
        {
            return NextExhibit != null;
        }

        private async void NextExhibitCommandExecuteMethod(string exhibitId)
        {
            //track goto next exhibit interaction
            var exTo = await _dataService.GetExhibitDetailByExhibitId(exhibitId, new CancellationToken());
            if (_analyticsService != null && exTo != null) 
                _analyticsService.TrackNextExhibitInteraction(exTo.Exhibit.Name);
            
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
            Events = await Task.Run(() => _dataService.GetEvents(new CancellationToken()).TryCatchAsync());
            if (Events == null)
            {
                await _messageService.ShowAsync(Strings.SiteDataFailureMessage, String.Empty);
                return;
            }

            SquareFootEvents = new ObservableCollection<EventItemViewModel>(Events.Select(x => new EventItemViewModel(x, _analyticsService)));

            var ed = await Task.Run(() =>_dataService.GetExhibitDetailByExhibitId(navigationParameter as String, new CancellationToken()));
            if (ed == null)
            {
                await _messageService.ShowAsync(Strings.SiteDataFailureMessage, String.Empty);
                return;
            }

            Exhibit = new ExhibitItemViewModel(ed.Exhibit, _analyticsService);
            NextExhibit = ed.NextExhibit == null ? null : new ExhibitItemViewModel(ed.NextExhibit, _analyticsService);

            LocalMediaCollection = Exhibit.MediaContent;
            LocalMediaCollection.Add(new FooterFakeMediaItemViewModel());
            LocalMediaCollection.Insert(0, new HeaderFakeMediaItemViewModel());

            LoadedEventsTaskCompletionSource.TrySetResult(true);

            base.OnNavigatedTo(navigationParameter, navigationMode, viewModelState);
        }

        public void LaunchVideoCommandHandler(MediaContentSourceItemViewModel item)
        {
            if (item == null) return;
            
            //track video plays
            var ex = this.Exhibit;
            var mediaItem = SelectedMediaContentSource;
            if (mediaItem != null && _analyticsService != null)
                _analyticsService.TrackVideoPlayInExhibitView(ex.Name, ex.Id, mediaItem.Media.VideoId);

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
                    RaisePinContextChanged();
                    SetStartupCommand.RaiseCanExecuteChanged();
                    SetStartupVisibility = SetStartupCommand.CanExecute() ? Visibility.Visible : Visibility.Collapsed;
                    ClearStartupCommand.RaiseCanExecuteChanged();
                    ClearStartupVisibility = ClearStartupCommand.CanExecute() ? Visibility.Visible : Visibility.Collapsed;
                    
                }
            }
        }      

        #endregion

        #region ResizingProperties
        
        public double NineSixteenthsOfWidth
        {
            get
            {
                return (FullScreenWidth/16)*9;
            }
        }
        public double OneThirdPanelWidth
        {
            get { return FullScreenWidth * .325; } // approx one third (per comp)
        }

        public double PortraitHeaderFooterHeight
        {
            get { return FullScreenHeight * .275; } // approx one third (per comp)
        }              

        public override void WindowSizeChanged(double width, double height)
        {
            base.WindowSizeChanged(width, height);            
            OnPropertyChanged("NineSixteenthsOfWidth");
            OnPropertyChanged("OneThirdPanelWidth");
            OnPropertyChanged("PortraitHeaderFooterHeight");
        }


        #endregion

        #region Pinning + Set Start Up

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

        public override SecondaryTileArgs GetSecondaryTileArguments()
        {
            if (Exhibit == null)
                return null;
            return new SecondaryTileArgs()
            {
                Id = PinningUtils.GetSecondaryTileIdByExhibitId(Exhibit.Id),
                ArgumentsName = PinningUtils.GetSecondaryTileIdByExhibitId(Exhibit.Id),
                ShortName = Exhibit.Name,
                DisplayName = Exhibit.Name
            };
        }

        #endregion
    }
}