using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
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

namespace OneMSQFT.Windows.Views
{
    public sealed partial class TimelinePage
    {
        private ScrollViewer _timelineGridViewScrollViewer;

        public TimelinePage()
        {
            this.InitializeComponent();
            this.StoryboardSeeker.Begin(); // a nice to have 
            InitAppBar();
            Loaded += TimelinePage_Loaded;
        }

        void TimelinePage_Loaded(object sender, RoutedEventArgs e)
        {
            PopulateTopAppbar(((ITimelinePageViewModel)this.DataContext));
            ITimelinePageViewModel vm = this.DataContext as ITimelinePageViewModel;
            if (vm != null)
            {
                vm.WindowSizeChanged(Window.Current.Bounds.Width, Window.Current.Bounds.Height);
            }
        }

        private void itemsGridView_Loaded(object sender, RoutedEventArgs e)
        {
            _timelineGridViewScrollViewer = VisualTreeUtilities.GetVisualChild<ScrollViewer>(itemsGridView);
            if(_timelineGridViewScrollViewer != null)
            {
                _timelineGridViewScrollViewer.ViewChanged += _timelineGridViewScrollViewer_ViewChanged;
            }
            itemsGridView.Opacity = 1;
        }

        void _timelineGridViewScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            this.StoryboardSeeker.Resume();
            int offSet = Convert.ToInt32(_timelineGridViewScrollViewer.HorizontalOffset);
            this.StoryboardSeeker.Seek(new TimeSpan(0, 0, offSet));
            this.StoryboardSeeker.Pause();
        }

        private void semanticZoom_ViewChangeCompleted(object sender, SemanticZoomViewChangedEventArgs e)
        {
            if (e.SourceItem.Item != null && e.IsSourceZoomedInView != true)
            {
                itemsGridView.ScrollIntoView(((EventItemViewModel)e.SourceItem.Item));
                itemsGridView.Opacity = 1;
            }
        }

        private void semanticZoom_ViewChangeStarted(object sender, SemanticZoomViewChangedEventArgs e)
        {
            if (e.SourceItem.Item != null && e.IsSourceZoomedInView != true)
            {
                itemsGridView.Opacity = 0;
            }
        }

        public override async void OMSQFTAppBarButtonCommandHandler(EventItemViewModel item)
        {
            if (item == null) return;
            ITimelinePageViewModel vm = this.DataContext as ITimelinePageViewModel;
            if (vm != null)
            {
                int itemIndex = vm.SquareFootEvents.IndexOf(item)+1;
                _timelineGridViewScrollViewer.ScrollToHorizontalOffset((itemIndex*vm.FullScreenItemWidth) - 50);
            }
            //itemsGridView.ScrollIntoView(((EventItemViewModel)item));
            TopAppBar.IsOpen = false;
        }

        protected override void WindowSizeChanged(object sender, WindowSizeChangedEventArgs e)
        {
            ITimelinePageViewModel vm = this.DataContext as ITimelinePageViewModel;
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
                semanticZoom.Opacity = .1;
                VideoPopup.IsOpen = true; 
            }
        }

        private void VideoPopup_Closed(object sender, object e)
        {
            semanticZoom.Opacity = 1;
        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            if (VideoPopup.IsOpen)
            {
                VideoPopup.IsOpen = false;
            }
        }

    }
}
