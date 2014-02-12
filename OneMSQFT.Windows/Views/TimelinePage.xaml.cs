using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.ApplicationSettings;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Microsoft.Practices.Prism.StoreApps;
using OneMSQFT.UILogic.Interfaces.ViewModels;
using OneMSQFT.UILogic.ViewModels;
using Windows.UI.Core;
using OneMSQFT.Windows.DesignViewModels;
using OneMSQFT.Windows.Extensions;

namespace OneMSQFT.Windows.Views
{
    public sealed partial class TimelinePage
    {
        private ScrollViewer _timelineGridViewScrollViewer;

        public TimelinePage()
        {
            this.InitializeComponent();

            InitAppBars();
            Loaded += TimelinePage_Loaded;

            var vm = this.DataContext as ITimelinePageViewModel;
            vm.FullScreenHeight = Window.Current.Bounds.Height;
            vm.FullScreenWidth = Window.Current.Bounds.Width;
            ((TimelinePageViewModel)this.DataContext).PropertyChanged += TimelinePage_PropertyChanged;
        }

        void TimelinePage_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SquareFootEvents")
            {
                if (((TimelinePageViewModel)this.DataContext).SquareFootEvents.Count > 0)
                {
                    this.PopulateTopAppbar(((BasePageViewModel)this.DataContext));
                }
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter != null && e.Parameter is String)
            {
                ScrollToEventById(e.Parameter as String);
            }
        }

        void TimelinePage_Loaded(object sender, RoutedEventArgs e)
        {
            var vm = this.DataContext as ITimelinePageViewModel;
            if (vm != null)
            {
                vm.WindowSizeChanged(Window.Current.Bounds.Width, Window.Current.Bounds.Height);
            }
        }

        private void itemsGridView_Loaded(object sender, RoutedEventArgs e)
        {
            _timelineGridViewScrollViewer = VisualTreeUtilities.GetVisualChild<ScrollViewer>(itemsGridView);
           // itemsGridView.Opacity = 1;
        }

        private void semanticZoom_ViewChangeCompleted(object sender, SemanticZoomViewChangedEventArgs e)
        {
            if (e.SourceItem.Item != null && e.IsSourceZoomedInView != true)
            {
                ScrollToEventById(((EventItemViewModel)e.SourceItem.Item).Id);
                itemsGridView.Opacity = 1;
            }

            if (e.IsSourceZoomedInView == true)
            {
                this.semanticZoom.Background = new SolidColorBrush(Colors.Transparent);
                LogoGrid.Visibility = Visibility.Visible;
            }

        }

        private void semanticZoom_ViewChangeStarted(object sender, SemanticZoomViewChangedEventArgs e)
        {
            if (e.SourceItem.Item != null && e.IsSourceZoomedInView != true)
            {
                itemsGridView.Opacity = 0;
            }
            LogoGrid.Visibility = Visibility.Collapsed;
        }

        public override async void TopAppBarEventButtonCommandHandler(String eventId)
        {
            ScrollToEventById(eventId);
            TopAppBar.IsOpen = false;
            BottomAppBar.IsOpen = false;
        }

        async private void ScrollToEventById(String eventId)
        {
            VideoPopup.IsOpen = false;
            var vm = DataContext as ITimelinePageViewModel;
            vm.WindowSizeChanged(Window.Current.Bounds.Width, Window.Current.Bounds.Height); 

            if (String.IsNullOrEmpty(eventId))
            {   
                // SCROLL HOME
                await _timelineGridViewScrollViewer.ScrollToHorizontalOffsetWithAnimation(0);
            }

            var e = vm.SquareFootEvents.FirstOrDefault(x => x.Id == eventId);
            if (e == null)
                return;
            var itemIndex = vm.SquareFootEvents.IndexOf(e);
            await _timelineGridViewScrollViewer.ScrollToHorizontalOffsetWithAnimation((itemIndex * vm.EventItemWidth));
        }

        protected override void WindowSizeChanged(object sender, WindowSizeChangedEventArgs e)
        {
            var vm = DataContext as ITimelinePageViewModel;
            if(vm != null)
            {
                vm.WindowSizeChanged(Window.Current.Bounds.Width, Window.Current.Bounds.Height);
            }
            base.WindowSizeChanged(sender, e);
        }

        private void VideoButton_Clicked(object sender, RoutedEventArgs e)
        {
            if (!VideoPopup.IsOpen)
            {
                VideoPopup.IsOpen = true; 
            }
        }

        private void VideoPopup_Closed(object sender, object e)
        {

        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            if (VideoPopup.IsOpen)
            {
                VideoPopup.IsOpen = false;
            }
        }

        public override void PopulateTopAppbar(BasePageViewModel vm)
        {
            base.PopulateTopAppbar(vm);
            AboutButton.Command = AboutButtonClickCommand;
        }

        private void AdminButton_OnClick(object sender, RoutedEventArgs e)
        {
            SettingsPane.Show();
        }
    }
}
