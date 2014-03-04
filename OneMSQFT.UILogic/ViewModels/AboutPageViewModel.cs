﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;
using OneMSQFT.Common.Analytics;
using OneMSQFT.Common.Models;
using OneMSQFT.Common.Services;
using OneMSQFT.UILogic.Interfaces.ViewModels;

namespace OneMSQFT.UILogic.ViewModels
{
    public class AboutPageViewModel : BasePageViewModel, IAboutPageViewModel
    {
        private readonly IDataService _dataService;
        private readonly IAlertMessageService _messageService;
        private readonly IAnalyticsService _analyticsService;

        public AboutPageViewModel(IDataService dataService, IAlertMessageService messageService, IAnalyticsService analyticsService)
        {
            _dataService = dataService;
            _messageService = messageService;
            _analyticsService = analyticsService;
            SquareFootEvents = new ObservableCollection<EventItemViewModel>();
        }


        async public override void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {                        
            var events = await _dataService.GetEvents(new CancellationToken());
            if (events == null)
            {
                await _messageService.ShowAsync("Error", "There was a problem loading events");
                return;
            }

            SquareFootEvents = new ObservableCollection<EventItemViewModel>(events.Select(x => new EventItemViewModel(x, _analyticsService)));
            _analyticsService.TrackPageViewAbout();

            base.OnNavigatedTo(navigationParameter, navigationMode, viewModelState);
        }


        #region resizing       

        public double AboutPageTotalWidth
        {
            get
            {
                return FullScreenWidth + AboutPageTwoThirdsWidth;
            }
        }

        public double AboutPageTwoThirdsWidth
        {
            get
            {
                return FullScreenWidth * .666;
            }
        }

        public double Panel1TextWidth
        {
            get
            {
                return AboutPageTwoThirdsWidth / 9 * 7;
            }
        }
        public double Panel2TextWidth
        {
            get
            {
                return FullScreenWidth / 10 * 8;
            }
        }

        public Thickness Panel1Margin
        {
            get
            {
                return new Thickness(90, FullScreenHeight / 4, 0, 0);
            }
        }
        public Thickness Panel2Margin
        {
            get
            {
                return new Thickness(0, (FullScreenHeight / 4)+5, 0, 0);
            }
        }        

        public override void WindowSizeChanged(double width, double height)
        {
            base.WindowSizeChanged(width, height);
            OnPropertyChanged("AboutPageTotalWidth");
            OnPropertyChanged("AboutPageTwoThirdsWidth");
            OnPropertyChanged("Panel1TextWidth");
            OnPropertyChanged("Panel2TextWidth");
            OnPropertyChanged("Panel1Margin");
            OnPropertyChanged("Panel2Margin");            
        }

        #endregion
    }

}
