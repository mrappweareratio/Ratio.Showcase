using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;
using Windows.UI;
using Windows.UI.StartScreen;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Microsoft.Practices.Prism.StoreApps;
using OneMSQFT.Common;
using OneMSQFT.Common.Services;
using OneMSQFT.Common.Models;
using OneMSQFT.UILogic.Interfaces.ViewModels;
using OneMSQFT.UILogic.Navigation;
using OneMSQFT.UILogic.Utils;
using OneMSQFT.UILogic.ViewModels;

namespace OneMSQFT.WindowsStore.Views
{
    public sealed partial class TimelinePage
    {
        private bool _loaded;
        private ScrollViewer _timelineGridViewScrollViewer;
        private readonly DispatcherTimer _scrollerTimer;
        private Boolean _appBarIsAutoScrolling;
        /// <summary>
        /// set when the page has navigated to and should be scrolled to an item
        /// </summary>
        private string _navigatedScrollToEventId;

        public TimelinePage()
        {
            this.InitializeComponent();

            InitAppBars();
            this.Loaded += (sender, args) =>
            {
                _loaded = true;
                ProcessWindowSizeChangedEvent();
            };
            var vm = GetDataContextAsViewModel<ITimelinePageViewModel>();
            vm.PropertyChanged += TimelinePage_PropertyChanged;
            vm.PinContextChanged += VmOnPinContextChanged;
            ProcessWindowSizeChangedEvent();

            _scrollerTimer = new DispatcherTimer();
            _scrollerTimer.Interval = new TimeSpan(0, 0, 0, 0, 500);
            _scrollerTimer.Tick += ScrollerTimerTick;

            var app = AppLocator.Current;
            if (app != null)
            {
                StartupButtonStackPanel.Visibility = app.KioskModeEnabled ? Visibility.Visible : Visibility.Collapsed;
                //PinButton.Visibility = app.KioskModeEnabled ? Visibility.Collapsed : Visibility.Visible;
                if (!app.KioskModeEnabled)
                {
                    _sharing = AppLocator.Current.SharingService;
                    _dataTransferManager = DataTransferManager.GetForCurrentView();
                }
            }

            VideoPlayerUserControl.MediaEndedCommand = new DelegateCommand(MediaEndedCommandHandler);

            _timelineGridViewLoadedTaskCompletionSourceCompletion = new TaskCompletionSource<bool>();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            // needed for back stack navigation when user has changed resolution on details page
            ProcessWindowSizeChangedEvent();

            if (e.NavigationMode != NavigationMode.Back)
            {
                _navigatedScrollToEventId = e.Parameter as String;
                if (!String.IsNullOrEmpty(_navigatedScrollToEventId) && _timelineGridViewLoadedTaskCompletionSourceCompletion.Task.IsCompleted)
                {
                    //new navigation but the page has already loaded - coming in from a Pin
                    ScrollToLandingItem();
                }
                else
                {
                    //scroll to landing item on new navigations after the grid view and view model have both loaded
                    Task.WhenAll(_timelineGridViewLoadedTaskCompletionSourceCompletion.Task,
                        GetDataContextAsViewModel<IBasePageViewModel>().LoadedEventsTaskCompletionSource.Task)
                    .ContinueWith(task => ScrollToLandingItem(), TaskScheduler.FromCurrentSynchronizationContext());
                }
            }

            if (!AppLocator.Current.KioskModeEnabled)
            {
                _dataTransferManager.DataRequested += DataTransferManagerOnDataRequested;
                _dataTransferManager.TargetApplicationChosen += DataTransferManagerTargetApplicationChosen;
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            if (e.SourcePageType != null && e.SourcePageType != GetType())
            {
                //navigation cache mode is on, so reset loaded event to wait for re-entrance
                _timelineGridViewLoadedTaskCompletionSourceCompletion = new TaskCompletionSource<bool>();
            }
            if (semanticZoom.IsZoomedInViewActive == false)
            {
                semanticZoom.ToggleActiveView();
            }
            if (!AppLocator.Current.KioskModeEnabled)
            {
                _dataTransferManager.DataRequested -= DataTransferManagerOnDataRequested;
                _dataTransferManager.TargetApplicationChosen -= DataTransferManagerTargetApplicationChosen;
            }
        }

        private void MediaEndedCommandHandler()
        {
            VideoPopup.IsOpen = false;
        }

        #region Sharing

        private readonly ISharingService _sharing;

        private void DataTransferManagerOnDataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            var vm = GetDataContextAsViewModel<ITimelinePageViewModel>();
            if (vm.SelectedEvent == null)
            {
                args.Request.FailWithDisplayText(Strings.SharingFailedDisplayText);
                return;
            }
            var ev = vm.SelectedEvent;
            var evPos = vm.GetEventIndexById(ev.Id);
            Uri uri = null;

            var selectedMediaContentSource = FlipViewer.SelectedItem as MediaContentSourceItemViewModel;
            if (FlipViewPopup.IsOpen && selectedMediaContentSource != null && selectedMediaContentSource.ContentSourceType == ContentSourceType.Video)
            {
                if (!_sharing.TryGetVideoShareUri(selectedMediaContentSource.Media, out uri))
                {
                    args.Request.FailWithDisplayText(Strings.SharingFailedDisplayText);
                    return;
                }
                args.Request.Data.Properties.Title = String.Format(Strings.SharingTitleVideoFromEventFormat, ev.Name);
                args.Request.Data.Properties.Description = ev.Description;
                args.Request.Data.Properties.ContentSourceWebLink = uri;
                args.Request.Data.SetWebLink(uri);
                _targetApplicationChosenDelegate = appName => AppLocator.Current.Analytics.TrackVideoShareInEventView(ev.Name,
                ev.SquareFootage, evPos, selectedMediaContentSource.Media.VideoId, uri.AbsoluteUri, appName);

            }
            else
            {
                if (!_sharing.TryGetEventShareUri(ev.Event, out uri))
                {
                    args.Request.FailWithDisplayText(Strings.SharingFailedDisplayText);
                    return;
                }
                args.Request.Data.Properties.Title = ev.Name;
                args.Request.Data.Properties.Description = ev.Description;
                args.Request.Data.Properties.ContentSourceWebLink = uri;
                args.Request.Data.SetWebLink(uri);
                _targetApplicationChosenDelegate = appName => AppLocator.Current.Analytics.TrackShareEventInteraction(ev.Name,
                    ev.SquareFootage, evPos, uri.AbsoluteUri, appName);
            }
        }

        private Action<String> _targetApplicationChosenDelegate;
        void DataTransferManagerTargetApplicationChosen(DataTransferManager sender, TargetApplicationChosenEventArgs args)
        {
            if (_targetApplicationChosenDelegate == null)
                return;
            _targetApplicationChosenDelegate(args.ApplicationName);
            //unwire and reset the delegate
            _targetApplicationChosenDelegate = null;
        }

        #endregion

        private void VmOnPinContextChanged(object sender, EventArgs eventArgs)
        {
            var args = GetDataContextAsViewModel<IBasePageViewModel>().GetSecondaryTileArguments();
            if (args == null)
                return;
            ToggleAppBarButton(this.PinButton, this.PinButtonImage, !SecondaryTile.Exists(args.Id));
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
            if (e.PropertyName == "IsHorizontal")
            {
                if (!_loaded)
                    return;
                if (GetDataContextAsViewModel<ITimelinePageViewModel>().IsHorizontal)
                {
                    VisualStateManager.GoToState(this, "FullScreenLandscape", true);
                    _timelineGridViewScrollViewer = VisualTreeUtilities.GetVisualChild<ScrollViewer>(TimelineGridView);
                    _timelineGridViewScrollViewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Visible;
                    _timelineGridViewScrollViewer.HorizontalScrollMode = ScrollMode.Enabled;
                    _timelineGridViewScrollViewer.HorizontalSnapPointsAlignment = SnapPointsAlignment.Center;
                    _timelineGridViewScrollViewer.HorizontalSnapPointsType = SnapPointsType.Mandatory;
                    _timelineGridViewScrollViewer.IsHorizontalRailEnabled = true;
                    _timelineGridViewScrollViewer.IsHorizontalScrollChainingEnabled = true;

                    _timelineGridViewScrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Disabled;
                    _timelineGridViewScrollViewer.VerticalScrollMode = ScrollMode.Disabled;
                    _timelineGridViewScrollViewer.VerticalSnapPointsType = SnapPointsType.None;
                    _timelineGridViewScrollViewer.IsHorizontalRailEnabled = false;
                    _timelineGridViewScrollViewer.IsHorizontalScrollChainingEnabled = false;

                    _timelineGridViewScrollViewer.ZoomMode = ZoomMode.Disabled;

                    StackPanelRightAppBarImages.Visibility = PinButtonImage.Visibility = Visibility.Collapsed;
                    StackPanelRightAppBarText.Visibility = PinButton.Visibility = Visibility.Visible;
                }
                else
                {
                    VisualStateManager.GoToState(this, "FullScreenPortrait", true);
                    _timelineGridViewScrollViewer = VisualTreeUtilities.GetVisualChild<ScrollViewer>(TimelineGridView);
                    _timelineGridViewScrollViewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled;
                    _timelineGridViewScrollViewer.HorizontalScrollMode = ScrollMode.Disabled;
                    _timelineGridViewScrollViewer.HorizontalSnapPointsAlignment = SnapPointsAlignment.Center;
                    _timelineGridViewScrollViewer.HorizontalSnapPointsType = SnapPointsType.None;
                    _timelineGridViewScrollViewer.IsHorizontalRailEnabled = false;
                    _timelineGridViewScrollViewer.IsHorizontalScrollChainingEnabled = false;

                    _timelineGridViewScrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
                    _timelineGridViewScrollViewer.VerticalScrollMode = ScrollMode.Enabled;
                    _timelineGridViewScrollViewer.VerticalSnapPointsAlignment = SnapPointsAlignment.Center;
                    _timelineGridViewScrollViewer.VerticalSnapPointsType = SnapPointsType.Mandatory;
                    _timelineGridViewScrollViewer.IsHorizontalRailEnabled = true;
                    _timelineGridViewScrollViewer.IsHorizontalScrollChainingEnabled = true;

                    _timelineGridViewScrollViewer.ZoomMode = ZoomMode.Disabled;

                    StackPanelRightAppBarImages.Visibility = PinButtonImage.Visibility = Visibility.Visible;
                    StackPanelRightAppBarText.Visibility = PinButton.Visibility = Visibility.Collapsed;
                }

            }
        }

        private TaskCompletionSource<bool> _timelineGridViewLoadedTaskCompletionSourceCompletion;
        private void TimelineGridView_Loaded(object sender, RoutedEventArgs e)
        {
            _timelineGridViewScrollViewer = VisualTreeUtilities.GetVisualChild<ScrollViewer>(TimelineGridView);
            _timelineGridViewLoadedTaskCompletionSourceCompletion.TrySetResult(true);
        }

        private void ScrollToLandingItem()
        {
            if (_timelineGridViewScrollViewer == null)
                return;
            if (!String.IsNullOrEmpty(_navigatedScrollToEventId))
            {
                ScrollToEventById(_navigatedScrollToEventId);
                _navigatedScrollToEventId = null;
                _scrolledToLandingItem = true;
                return;
            }
            if (_scrolledToLandingItem)
                return;
            ScrollToFirstItem();
            _scrolledToLandingItem = true;
        }

        private void ScrollToFirstItem()
        {
            var vm = GetDataContextAsViewModel<ITimelinePageViewModel>();
            // scroll to first event item, first item is buffer item
            if (vm.TimeLineItems.Count > 1)
                ScrollToEventById(vm.TimeLineItems[1].Id);
        }

        public override void PopulateTopAppbar(IBasePageViewModel vm)
        {
            base.PopulateTopAppbar(vm);
            AboutButton.Command = AboutButtonImage.Command = AboutButtonClickCommand;
            FacebookButton.Command = FacebookButtonImage.Command = FacebookButtonClickCommand;
            TwitterButton.Command = TwitterButtonImage.Command = TwitterButtonClickCommand;
            InstagramButton.Command = InstagramButtonImage.Command = InstagramButtonClickCommand;
        }

        #region Timeline UI

        void ScrollerTimerTick(object sender, object e)
        {
            ShowTimelineMasks(true);
            _scrollerTimer.Stop();
            _appBarIsAutoScrolling = false;
        }

        private void ShowTimelineMasks(bool show)
        {
            if (show && semanticZoom.IsZoomedInViewActive)
            {
                MaskLeft.Visibility = Visibility.Visible;
                MaskRight.Visibility = Visibility.Visible;
                MaskStoryboard.Begin();
            }
            else
            {
                MaskLeft.Opacity = 0;
                MaskRight.Opacity = 0;
                MaskLeft.Visibility = Visibility.Collapsed;
                MaskRight.Visibility = Visibility.Collapsed;
            }
        }

        void TimelineGridViewScrollViewer_ViewChanging(object sender, ScrollViewerViewChangingEventArgs e)
        {
            ShowTimelineMasks(false);
        }

        void TimelineGridViewScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            // this keeps the selected Event item from changing the data context if the MediaViewerPopUp is Open and the user resizes or rotates.
            if (FlipViewPopup.IsOpen) return;

            if (e.IsIntermediate == false)
            {
                if (_appBarIsAutoScrolling == false)
                {
                    ShowTimelineMasks(true);
                    if (GetDataContextAsViewModel<TimelinePageViewModel>().IsHorizontal)
                    {
                        SelectItemByOffset(((ScrollViewer)sender).HorizontalOffset);
                    }
                    else
                    {
                        SelectItemByOffset(((ScrollViewer)sender).VerticalOffset);
                    }
                }
            }
        }

        private void SelectItemByOffset(double offset)
        {
            var vm = GetDataContextAsViewModel<TimelinePageViewModel>();
            var i = 0;
            if (vm.IsHorizontal)
            {
                i = Convert.ToInt32((offset - vm.BufferItemWidth) / vm.EventItemWidth);
            }
            else
            {
                i = Convert.ToInt32((offset - vm.BufferItemHeight) / vm.EventItemHeight);
            }
            vm.SelectedEvent = vm.TimeLineItems[i + 1]; // + 1 to skip the buffer item
        }

        private bool _semanticZoomClosedFromTopAppBarEvent;
        private readonly DataTransferManager _dataTransferManager;
        /// <summary>
        /// prevents subsequent navigation or re-entrance to for auto scrolling
        /// </summary>
        private bool _scrolledToLandingItem;

        private void semanticZoom_ViewChangeCompleted(object sender, SemanticZoomViewChangedEventArgs e)
        {
            var theApp = AppLocator.Current;
            if (e.SourceItem.Item != null && e.IsSourceZoomedInView == false)
            {
                if (_semanticZoomClosedFromTopAppBarEvent)
                {
                    _semanticZoomClosedFromTopAppBarEvent = false;
                }
                else
                {
                    //Track event selection
                    var vm = GetDataContextAsViewModel<ITimelinePageViewModel>();
                    var ev = vm.SelectedEvent;
                    theApp.Analytics.TrackTimelineSemanticZoomEventInteraction(ev.Name, ev.SquareFootage, vm.TimeLineItems.IndexOf(ev));

                    ScrollToEventById(((EventItemViewModel)e.SourceItem.Item).Id);
                }
                TimelineGridView.Opacity = 1;
            }

            if (e.IsSourceZoomedInView)
            {
                //Track going into ZoomedOut view
                theApp.Analytics.TrackTimelineSemanticZoom();

                ShowTimelineMasks(false);
                this.semanticZoom.Background = new SolidColorBrush(Colors.Transparent);
                ZoomedInLogo.Visibility = Visibility.Visible;
                ZoomedOutLogo.Visibility = Visibility.Collapsed;
                MaskLeft.Visibility = Visibility.Collapsed;
                MaskRight.Visibility = Visibility.Collapsed;
            }
            else
            {
                ZoomedInLogo.Visibility = Visibility.Collapsed;
                ZoomedOutLogo.Visibility = Visibility.Visible;
                MaskLeft.Visibility = Visibility.Visible;
                MaskRight.Visibility = Visibility.Visible;

            }

        }

        private void semanticZoom_ViewChangeStarted(object sender, SemanticZoomViewChangedEventArgs e)
        {
            if (e.SourceItem.Item != null && e.IsSourceZoomedInView != true)
            {
                TimelineGridView.Opacity = 0;
            }
        }

        public override void TopAppBarEventButtonCommandHandler(String eventId)
        {
            var ev = GetDataContextAsViewModel<IBasePageViewModel>().SquareFootEvents.FirstOrDefault(x => x.Id == eventId);
            AppLocator.Current.Analytics.TrackAppBarEventInteraction(ev.Name, ev.SquareFootage, GetDataContextAsViewModel<IBasePageViewModel>().GetEventIndexById(ev.Id), "events");

            if (semanticZoom.IsZoomedInViewActive == false)
            {
                _semanticZoomClosedFromTopAppBarEvent = true;
                semanticZoom.ToggleActiveView();
            }
            ScrollToEventById(eventId);
            TopAppBar.IsOpen = false;
            BottomAppBar.IsOpen = false;
            VideoPopup.IsOpen = false;
            FlipViewPopup.IsOpen = false;
        }

        private void ScrollToEventById(String eventId)
        {
            VideoPopup.IsOpen = false;

            if (String.IsNullOrEmpty(eventId))
            {
                // SCROLL HOME
                ScrollToFirstItem();
            }

            var vm = GetDataContextAsViewModel<ITimelinePageViewModel>();
            var e = vm.SquareFootEvents.FirstOrDefault(x => x.Id == eventId);
            if (e == null)
                return;
            vm.SelectedEvent = e;
            var itemIndex = vm.SquareFootEvents.IndexOf(e) + 1; // +1 for buffer items

            _appBarIsAutoScrolling = true;
            ShowTimelineMasks(false);
            if (vm.IsHorizontal)
            {
                _timelineGridViewScrollViewer.ChangeView(
                    ((itemIndex * vm.EventItemWidth) - vm.MaskItemWidth - vm.BufferItemWidth), 0, 1);
            }
            else
            {
                _timelineGridViewScrollViewer.ChangeView(
                    0, ((itemIndex * vm.EventItemHeight) - vm.MaskItemHeight - vm.BufferItemHeight), 1);
            }
            _scrollerTimer.Start();
        }

        #endregion

        #region resizing


        #endregion

        #region MediaViewer

        private void VideoButton_Clicked(object sender, RoutedEventArgs e)
        {
            if (!VideoPopup.IsOpen)
            {
                //track video plays
                var vm = GetDataContextAsViewModel<ITimelinePageViewModel>();
                var ev = vm.SelectedEvent;
                var mediaItem = FlipViewer.SelectedItem as MediaContentSourceItemViewModel;
                if (ev != null && mediaItem != null)
                    AppLocator.Current.Analytics.TrackVideoPlayInEventView(ev.Name, mediaItem.Media.VideoId, ev.SquareFootage, vm.GetEventIndexById(ev.Id));

                VideoPopup.IsOpen = true;
                VideoPlayerUserControl.SelectedMediaContentSource = mediaItem;
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

        private void VideoPopUpCloseButton_Click(object sender, RoutedEventArgs e)
        {
            if (VideoPopup.IsOpen)
            {
                VideoPopup.IsOpen = false;
            }
        }

        private void FlipViewPopUpCloseButton_Click(object sender, RoutedEventArgs e)
        {
            if (FlipViewPopup.IsOpen)
            {
                FlipViewPopup.IsOpen = false;
                // reposition if item has drifted while user rotated/resized while MediaViewer Popup was open
                ScrollToEventById(GetDataContextAsViewModel<TimelinePageViewModel>().SelectedEvent.Id);
            }
        }

        #endregion

        #region Pinning

        async private void Pin_OnClick(object sender, RoutedEventArgs e)
        {
            BottomAppBar.IsSticky = true;


            var vm = GetDataContextAsViewModel<IBasePageViewModel>();
            var args = vm.GetSecondaryTileArguments();
            var analytics = AppLocator.Current.Analytics;


            if (SecondaryTile.Exists(args.Id))
            {
                //track Event unpinning interaction
                if (analytics != null)
                    analytics.TrackUnPinEventInteraction(args.DisplayName);

                var secondaryTile = new SecondaryTile(args.Id);
                bool isUnpinned = await secondaryTile.RequestDeleteForSelectionAsync(GetElementRect((FrameworkElement)sender));
                ToggleAppBarButton(PinButton, PinButtonImage, isUnpinned);
            }
            else
            {
                var square150x150Logo = await RenderPinningLogo(150, 150);
                //track Event pinning interaction
                if (analytics != null)
                    analytics.TrackPinEventInteraction(args.DisplayName);

                var secondaryTile = new SecondaryTile(args.Id,
                    args.DisplayName,
                    args.ArgumentsName,
                    square150x150Logo,
                    TileSize.Square150x150);

                secondaryTile.VisualElements.BackgroundColor = ColorUtils.GetColorFromARGBString(args.BackgroundColor, Colors.White);
                secondaryTile.VisualElements.ForegroundText = ForegroundText.Dark;

                // If the asset for the small tile size is not provided, it will be created by scaling down the medium tile size asset.
                // Thus, providing the asset for the small tile size is not mandatory, though is recommended to avoid scaling artifacts and can be overridden as shown below. 
                // Note that the asset for the small tile size must be explicitly provided if alternates for the small tile size are also explicitly provided.
                secondaryTile.VisualElements.Square70x70Logo = await RenderPinningLogo(70, 70);

                // Only support of the small and medium tile sizes is mandatory.
                // To have the larger tile sizes available the assets must be provided.                
                secondaryTile.VisualElements.Wide310x150Logo = await RenderPinningLogo(310, 150);

                // The display of the secondary tile name can be controlled for each tile size.
                // The default is false.                
                secondaryTile.VisualElements.ShowNameOnSquare150x150Logo = false;
                secondaryTile.VisualElements.ShowNameOnWide310x150Logo = false;

                secondaryTile.RoamingEnabled = true;

                bool isPinned = await secondaryTile.RequestCreateForSelectionAsync(GetElementRect((FrameworkElement)sender));
                ToggleAppBarButton(PinButton, PinButtonImage, !isPinned);
            }
            BottomAppBar.IsSticky = false;
        }

        private async Task<Uri> RenderPinningLogo(uint width, uint height)
        {
            var vm = this.GetDataContextAsViewModel<TimelinePageViewModel>();
            RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap();

            if (width == 70)
            {
                RenderedGridSmall.Background = new SolidColorBrush(vm.SelectedEvent.EventColor);
                await renderTargetBitmap.RenderAsync(RenderedGridSmall, 70, 70);
            }
            else if (width == 150)
            {
                RenderedGridSquare.Background = new SolidColorBrush(vm.SelectedEvent.EventColor);
                await renderTargetBitmap.RenderAsync(RenderedGridSquare, 150, 150);
            }
            else if (width == 310)
            {
                RenderedGridWide.Background = new SolidColorBrush(vm.SelectedEvent.EventColor);
                await renderTargetBitmap.RenderAsync(RenderedGridWide, 310, 150);
            }

            // create or get the named folder under LocalFolder
            var folder = await ApplicationData.Current.LocalFolder.CreateFolderAsync("SecondaryTiles", CreationCollisionOption.OpenIfExists);
            try
            {
                // create the file from the Uri                                                                                                
                var fileName = String.Format("event_{0}_{1}x{2}.jpg", vm.SelectedEvent.Id, width, height);
                var file = await folder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
                if ((await file.GetBasicPropertiesAsync()).Size > 0 || await RenderTargetBitmapToStorageFile(file, renderTargetBitmap))
                {
                    // use the storage file name and the path to the local folder 
                    // to set the image art must be ms-appdata:/// or ms-appx:///
                    var uri = new Uri("ms-appdata:///local/SecondaryTiles/" + file.Name, UriKind.Absolute);
                    return uri;
                }
            }
            catch (Exception)
            {
            }
            return new Uri("ms-appx:///Assets/Logo.scale-100.png", UriKind.Absolute);
        }

        #endregion

        private void VisualStateTransition_Completed(object sender, object e)
        {
            if (GetDataContextAsViewModel<ITimelinePageViewModel>().SelectedEvent != null)
            {
                Task.Delay(500).ContinueWith((task) => ScrollToEventById(GetDataContextAsViewModel<ITimelinePageViewModel>().SelectedEvent.Id), TaskScheduler.FromCurrentSynchronizationContext());                
            }
        }
    }
}
