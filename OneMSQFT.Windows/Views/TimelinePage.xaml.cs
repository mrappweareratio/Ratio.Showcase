﻿using System;
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
using OneMSQFT.UILogic.Interfaces;
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
        private DispatcherTimer scrollerTimer;
        private Boolean AppBarIsAutoScrolling;

        public TimelinePage()
        {
            this.InitializeComponent();

            InitAppBars();
            Loaded += TimelinePage_Loaded;

            var vm = GetDataContextAsViewModel<ITimelinePageViewModel>();
            vm.FullScreenHeight = Window.Current.Bounds.Height;
            vm.FullScreenWidth = Window.Current.Bounds.Width;
            vm.PropertyChanged += TimelinePage_PropertyChanged;
            scrollerTimer = new DispatcherTimer();
            scrollerTimer.Interval = new TimeSpan(0, 0, 0, 1);
            scrollerTimer.Tick += scrollerTimer_Tick;

            var app = AppLocator.Current;
            if (app != null)
            {
                StartupButtonStackPanel.Visibility = app.KioskModeEnabled ? Visibility.Visible : Visibility.Collapsed;                
            }
        }

        void scrollerTimer_Tick(object sender, object e)
        {
            ShowTimelineMasks(true);
            scrollerTimer.Stop();
            AppBarIsAutoScrolling = false;
        }

        void TimelinePage_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SquareFootEvents")
            {
                if (GetDataContextAsViewModel<ITimelinePageViewModel>().SquareFootEvents.Count > 0)
                {
                    this.PopulateTopAppbar(GetDataContextAsViewModel<TimelinePageViewModel>());
                }
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            this._navigationEventArgs = e;
        }

        void TimelinePage_Loaded(object sender, RoutedEventArgs e)
        {
            var vm = GetDataContextAsViewModel<ITimelinePageViewModel>();
            vm.WindowSizeChanged(Window.Current.Bounds.Width, Window.Current.Bounds.Height);
        }

        private void itemsGridView_Loaded(object sender, RoutedEventArgs e)
        {
            _timelineGridViewScrollViewer = VisualTreeUtilities.GetVisualChild<ScrollViewer>(itemsGridView);
            _timelineGridViewScrollViewer.HorizontalSnapPointsAlignment = SnapPointsAlignment.Center;
            _timelineGridViewScrollViewer.HorizontalSnapPointsType = SnapPointsType.Mandatory;
            _timelineGridViewScrollViewer.ViewChanging += _timelineGridViewScrollViewer_ViewChanging;
            _timelineGridViewScrollViewer.ViewChanged += _timelineGridViewScrollViewer_ViewChanged;
            if (_navigationEventArgs != null && _navigationEventArgs.Parameter is String)
            {
                ScrollToEventById(_navigationEventArgs.Parameter as String);
            }
            else
            {
                // scroll to first event item, first item is buffer item
                ScrollToEventById(this.GetDataContextAsViewModel<TimelinePageViewModel>().TimeLineItems[1].Id);
            }
        }

        private void ShowTimelineMasks(bool show)
        {
            if (show)
            {
                MaskStoryboard.Begin();
            }
            else
            {
                MaskLeft.Opacity = 0;
                MaskRight.Opacity = 0;
            }
        }

        void _timelineGridViewScrollViewer_ViewChanging(object sender, ScrollViewerViewChangingEventArgs e)
        {
            ShowTimelineMasks(false);
        }

        void _timelineGridViewScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            if (e.IsIntermediate == false)
            {
                if (AppBarIsAutoScrolling == false)
                {
                    ShowTimelineMasks(true);
                    SelectItemByOffset(((ScrollViewer)sender).HorizontalOffset);
                }
            }
        }

        private void SelectItemByOffset(double offset)
        {
            var vm = GetDataContextAsViewModel<TimelinePageViewModel>();
            var i = Convert.ToInt32((offset - vm.BufferItemWidth) / vm.EventItemWidth);
            vm.SelectedEvent = vm.TimeLineItems[i + 1]; // + 1 to skip the buffer item
        }

        private bool _semanticZoomClosedFromTopAppBarEvent;
        private NavigationEventArgs _navigationEventArgs;

        private void semanticZoom_ViewChangeCompleted(object sender, SemanticZoomViewChangedEventArgs e)
        {
            if (e.SourceItem.Item != null && e.IsSourceZoomedInView == false)
            {
                if (_semanticZoomClosedFromTopAppBarEvent)
                {
                    _semanticZoomClosedFromTopAppBarEvent = false;
                }
                else
                {
                    ScrollToEventById(((EventItemViewModel)e.SourceItem.Item).Id);
                }
                itemsGridView.Opacity = 1;
            }

            if (e.IsSourceZoomedInView)
            {
                ShowTimelineMasks(false);
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
            if (semanticZoom.IsZoomedInViewActive == false)
            {
                _semanticZoomClosedFromTopAppBarEvent = true;
                semanticZoom.ToggleActiveView();
            }
            ScrollToEventById(eventId);
            TopAppBar.IsOpen = false;
            BottomAppBar.IsOpen = false;
        }

        async private void ScrollToEventById(String eventId)
        {
            VideoPopup.IsOpen = false;
            var vm = GetDataContextAsViewModel<TimelinePageViewModel>();
            vm.WindowSizeChanged(Window.Current.Bounds.Width, Window.Current.Bounds.Height);

            if (String.IsNullOrEmpty(eventId))
            {
                // SCROLL HOME
                await _timelineGridViewScrollViewer.ScrollToHorizontalOffsetWithAnimation(0);
            }

            var e = vm.SquareFootEvents.FirstOrDefault(x => x.Id == eventId);
            if (e == null)
                return;
            vm.SelectedEvent = e as EventItemViewModel;
            var itemIndex = vm.SquareFootEvents.IndexOf(e) + 1; // +1 for buffer items

            AppBarIsAutoScrolling = true;
            ShowTimelineMasks(false);
            await _timelineGridViewScrollViewer.ScrollToHorizontalOffsetWithAnimation((itemIndex * vm.EventItemWidth) - vm.MaskItemWidth - vm.BufferItemWidth, new TimeSpan(0, 0, 0, 1));
            scrollerTimer.Start();
        }

        protected override void WindowSizeChanged(object sender, WindowSizeChangedEventArgs e)
        {
            var vm = DataContext as ITimelinePageViewModel;
            if (vm != null)
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

        private void FlipViewButton_Clicked(object sender, RoutedEventArgs e)
        {
            if (!FlipViewPopup.IsOpen)
            {
                FlipViewPopup.IsOpen = true;
                VideoPopup.IsOpen = false;
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

        public override void PopulateTopAppbar(IBasePageViewModel vm)
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
