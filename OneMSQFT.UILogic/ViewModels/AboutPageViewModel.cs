using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
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
            var events = await _dataService.GetEvents();
            if (events == null)
            {
                await _messageService.ShowAsync("Error", "There was a problem loading events");
                return;
            }
            
            SquareFootEvents = new ObservableCollection<EventItemViewModel>(events.Select(x => new EventItemViewModel(x)));

            _analyticsService.TrackEvents(new TrackingEventsData() { TrackingEventsData.Events.PageView }, new TrackingContextData() { PageName = TrackingContextData.PageNames.About });

            base.OnNavigatedTo(navigationParameter, navigationMode, viewModelState);
        }


        #region resizing


        public double FullScreenWidth
        {
            get
            {
                return Window.Current.Bounds.Width;
            }
        }

        public double AboutPageTotalWidth
        {
            get
            {
                return Window.Current.Bounds.Width + AboutPageTwoThirdsWidth;
            }
        }

        public double AboutPageTwoThirdsWidth
        {
            get
            {
                return Window.Current.Bounds.Width * .666;
            }
        }

        public override void WindowSizeChanged(double width, double height)
        {
            IsHorizontal = width > height;
            OnPropertyChanged("IsHorizontal");

            OnPropertyChanged("FullScreenWidth");
            OnPropertyChanged("AboutPageTotalWidth");
            OnPropertyChanged("AboutPageTwoThirdsWidth");
        }

        #endregion
    }

}
