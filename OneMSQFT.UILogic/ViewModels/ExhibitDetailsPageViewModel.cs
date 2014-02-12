﻿using System.Collections.Generic;
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
        private readonly IDataService _dataService;
        private readonly IAlertMessageService _messageService;
        private readonly INavigationService _navigationService;

        public ExhibitDetailsPageViewModel(IDataService dataService, IAlertMessageService messageService, INavigationService navigationService)
        {
            _dataService = dataService;
            _messageService = messageService;
            _navigationService = navigationService;
            SquareFootEvents = new ObservableCollection<EventItemViewModel>();
            LaunchVideoCommand = new DelegateCommand<MediaContentSourceItemViewModel>(LaunchVideoCommandHandler);
            NextExhibitCommand = new DelegateCommand<string>(NextExhibitCommandExecuteMethod, NextExhibitCommandCanExecuteMethod);
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

        public double ExhibitItemWidth
        {
            get
            {
                return Window.Current.Bounds.Width * .9;
            }
        }

        public double ExhibitItemHeight
        {
            get
            {
                return FullScreenHeight;
            }
        }

        public double OneThirdPanelWidth
        {
            get { return ExhibitItemWidth / 3; }
        }

        public override void WindowSizeChanged(double width, double height)
        {
            OnPropertyChanged("FullScreenHeight");
            OnPropertyChanged("FullScreenWidth");
            OnPropertyChanged("ExhibitItemHeight");
            OnPropertyChanged("ExhibitItemWidth");
        }

        #endregion


    }
}