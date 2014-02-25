using System;
using Microsoft.Practices.Prism.StoreApps;
using OneMSQFT.Common.Models;
using OneMSQFT.UILogic.Interfaces.ViewModels;
using OneMSQFT.UILogic.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace OneMSQFT.WindowsStore.Views
{
    public partial class ExhibitDetailsPage : BasePageView
    {
        public ExhibitDetailsPage()
        {
            this.InitializeComponent();
            InitAppBars();
            Loaded += ExhibitDetailsPage_Loaded;
            var vm = GetDataContextAsViewModel<IExhibitDetailsPageViewModel>();
            vm.PropertyChanged += ExhibitDetailsPage_PropertyChanged;
            var app = AppLocator.Current;
            if (app != null)
            {
                StartupButtonStackPanel.Visibility = app.KioskModeEnabled ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        protected override void GoBack(object sender, RoutedEventArgs eventArgs)
        {
            if (this.Frame != null && !this.Frame.CanGoBack)
            {
                Frame.Navigate(typeof (TimelinePage), null);
                return;
            }
            base.GoBack(sender, eventArgs);
        }

        void ExhibitDetailsPage_Loaded(object sender, RoutedEventArgs e)
        {            
            GetDataContextAsViewModel<IExhibitDetailsPageViewModel>().WindowSizeChanged(Window.Current.Bounds.Width, Window.Current.Bounds.Height);            
        }

        void ExhibitDetailsPage_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SquareFootEvents")
            {
                if (GetDataContextAsViewModel<ExhibitDetailsPageViewModel>().SquareFootEvents.Count > 0)
                {
                    this.PopulateTopAppbar(GetDataContextAsViewModel<IBasePageViewModel>());
                }
            }
        }

        public override void TopAppBarEventButtonCommandHandler(String eventId)
        {
            this.Frame.Navigate(typeof(TimelinePage), eventId);
            TopAppBar.IsOpen = false;
            BottomAppBar.IsOpen = false;
        }

        public override void PopulateTopAppbar(IBasePageViewModel vm)
        {
            base.PopulateTopAppbar(vm);
            this.HomeButton.Command = this.HomeButtonClickCommand;
            this.AboutButton.Command = this.AboutButtonClickCommand;
        }

        private void LaunchVideoCommand_OnClick(object sender, RoutedEventArgs e)
        {
            if (!VideoPopup.IsOpen)
            {
                ExhibitDetailsPanels.Opacity = 0;
                VideoPopup.IsOpen = true;
            }
        }

        private void CloseVideo_OnClick(object sender, RoutedEventArgs e)
        {
            if (VideoPopup.IsOpen)
            {
                VideoPopup.IsOpen = false;
            }
        }

        private void VideoPopup_Closed(object sender, object e)
        {
            ExhibitDetailsPanels.Opacity = 1;
        }

        private void Pin_OnClick(object sender, RoutedEventArgs e)
        {

        }
    }
}
