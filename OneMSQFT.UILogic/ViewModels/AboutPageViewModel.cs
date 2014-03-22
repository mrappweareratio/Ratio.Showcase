using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;
using Microsoft.Practices.Prism.StoreApps;
using Ratio.Showcase.Shared;
using Ratio.Showcase.Shared.Services;
using Ratio.Showcase.UILogic.Interfaces.ViewModels;

namespace Ratio.Showcase.UILogic.ViewModels
{
    public class AboutPageViewModel : BasePageViewModel, IAboutPageViewModel
    {

        private readonly IDataService _dataService;
        private readonly IAnalyticsService _analyticsService;

        public AboutPageViewModel(IDataService dataService, IAnalyticsService analyticsService)
        {
            _dataService = dataService;
            _analyticsService = analyticsService;
            SquareFootEvents = new ObservableCollection<EventItemViewModel>();
            SetStartupCommand = new DelegateCommand(() => { }, () => false);
            ClearStartupCommand = new DelegateCommand(() => { }, () => false);
        }


        async public override void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            _analyticsService.TrackPageViewAbout();

            Events = await Task.Run(() => _dataService.GetEvents(new CancellationToken()).TryCatchAsync());
            if (Events == null)
            {
                base.OnNavigatedTo(navigationParameter, navigationMode, viewModelState);
                return;
            }

            SquareFootEvents = new ObservableCollection<EventItemViewModel>(Events.Select(x => new EventItemViewModel(x, _analyticsService)));

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
        public double AboutPageTotalHeight
        {
            get
            {
                return FullScreenHeight + AboutPageTwoThirdsHeight;
            }
        }

        public double AboutPageTwoThirdsWidth
        {
            get
            {
                return FullScreenWidth * .666;
            }
        }

        public double AboutPageTwoThirdsHeight
        {
            get
            {
                return FullScreenHeight * .666;
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
                return new Thickness(0, (FullScreenHeight / 4) + 5, 0, 0);
            }
        }

        public override void WindowSizeChanged(double width, double height)
        {
            base.WindowSizeChanged(width, height);
            OnPropertyChanged("AboutPageTotalWidth");
            OnPropertyChanged("AboutPageTwoThirdsWidth");
            OnPropertyChanged("AboutPageTotalHeight");
            OnPropertyChanged("AboutPageTwoThirdsHeight");
            OnPropertyChanged("Panel1TextWidth");
            OnPropertyChanged("Panel2TextWidth");
            OnPropertyChanged("Panel1Margin");
            OnPropertyChanged("Panel2Margin");
        }

        #endregion
    }

}
