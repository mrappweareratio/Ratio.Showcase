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
using OneMSQFT.Windows.Extensions;

namespace OneMSQFT.Windows.Views
{
    public sealed partial class TimelinePage
    {
        private ScrollViewer _timelineGridViewScrollViewer;

        public TimelinePage()
        {
            this.InitializeComponent();
            this.StoryboardSeeker.Begin(); // a nice to have 
            InitAppBars();
            Loaded += TimelinePage_Loaded;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter != null && e.Parameter is EventItemViewModel)
            {
                ScrollToThisEvent(e.Parameter as EventItemViewModel);
            }
        }

        void TimelinePage_Loaded(object sender, RoutedEventArgs e)
        {
            PopulateTopAppbar(((BasePageViewModel)this.DataContext));
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
                ScrollToThisEvent(((EventItemViewModel)e.SourceItem.Item));
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

        public override async void TopAppBarEventButtonCommandHandler(EventItemViewModel item)
        {
            if (item == null) return;
            ScrollToThisEvent(item);
            TopAppBar.IsOpen = false;
            BottomAppBar.IsOpen = false;
        }

        private void ScrollToThisEvent(EventItemViewModel item)
        {
            if (item == null) return;
            VideoPopup.IsOpen = false;
            ITimelinePageViewModel vm = this.DataContext as ITimelinePageViewModel;
            vm.WindowSizeChanged(Window.Current.Bounds.Width, Window.Current.Bounds.Height); 

            if (vm != null)
            {
                if (item.Id == null || item.Id == "0")
                {   
                    // SCROLL HOME
                    _timelineGridViewScrollViewer.ScrollToHorizontalOffsetWithAnimation(0);
                    this.Frame.BackStack.Clear();
                }
                foreach (var e in vm.SquareFootEvents)
                {
                    if (e.Id == item.Id)
                    {
                        var itemIndex = vm.SquareFootEvents.IndexOf(e) + 1;
                        _timelineGridViewScrollViewer.ScrollToHorizontalOffsetWithAnimation((itemIndex * vm.EventItemWidth) - 50);
                    }
                }
            }
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
                semanticZoom.Opacity = 0;
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

        public override void PopulateTopAppbar(BasePageViewModel vm)
        {
            base.PopulateTopAppbar(vm);
            this.AboutButton.Command = this.AboutButtonClickCommand;
            this.AdminButton.Command = this.AdminButtonClickCommand;
            this.AdminButton.CommandParameter = this.AdminButton;
        }

    }
}
