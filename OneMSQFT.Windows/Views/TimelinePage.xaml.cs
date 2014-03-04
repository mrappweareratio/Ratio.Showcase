using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.Graphics.Display;
using Windows.Storage;
using Windows.UI;
using Windows.UI.StartScreen;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using OneMSQFT.Common;
using OneMSQFT.Common.Services;
using OneMSQFT.Common.Analytics;
using OneMSQFT.Common.Models;
using OneMSQFT.UILogic.Interfaces.ViewModels;
using OneMSQFT.UILogic.Services;
using OneMSQFT.UILogic.Utils;
using OneMSQFT.UILogic.ViewModels;
using Windows.UI.Core;

namespace OneMSQFT.WindowsStore.Views
{
    public sealed partial class TimelinePage
    {
        private bool _loaded;
        private ScrollViewer TimelineGridViewScrollViewer;
        private DispatcherTimer scrollerTimer;
        private Boolean AppBarIsAutoScrolling;
        private NavigationEventArgs _navigationEventArgs;

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

            scrollerTimer = new DispatcherTimer();
            scrollerTimer.Interval = new TimeSpan(0, 0, 0, 0, 500);
            scrollerTimer.Tick += scrollerTimer_Tick;

            var app = AppLocator.Current;
            if (app != null)
            {
                StartupButtonStackPanel.Visibility = app.KioskModeEnabled ? Visibility.Visible : Visibility.Collapsed;
                //PinButton.Visibility = app.KioskModeEnabled ? Visibility.Collapsed : Visibility.Visible;
                if (!app.KioskModeEnabled)
                {
                    _sharing = AppLocator.Current.SharingService;
                    var dataTransferManager = DataTransferManager.GetForCurrentView();
                    dataTransferManager.DataRequested += DataTransferManagerOnDataRequested;
                    dataTransferManager.TargetApplicationChosen += DataTransferManagerTargetApplicationChosen;
                }
            }
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
            if (VideoPopup.IsOpen)
            {
                var selectedMediaContentSource = FlipViewer.SelectedItem as MediaContentSourceItemViewModel;
                if (selectedMediaContentSource == null || !_sharing.TryGetVideoShareUri(selectedMediaContentSource.Media, out uri))
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
            ToggleAppBarButton(this.PinButton, !SecondaryTile.Exists(args.Id));
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
                    TimelineGridViewScrollViewer = VisualTreeUtilities.GetVisualChild<ScrollViewer>(TimelineGridView);
                    TimelineGridViewScrollViewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Visible;
                    TimelineGridViewScrollViewer.HorizontalScrollMode = ScrollMode.Enabled;
                    TimelineGridViewScrollViewer.HorizontalSnapPointsAlignment = SnapPointsAlignment.Center;
                    TimelineGridViewScrollViewer.HorizontalSnapPointsType = SnapPointsType.Mandatory;
                    TimelineGridViewScrollViewer.IsHorizontalRailEnabled = true;
                    TimelineGridViewScrollViewer.IsHorizontalScrollChainingEnabled = true;

                    TimelineGridViewScrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Disabled;
                    TimelineGridViewScrollViewer.VerticalScrollMode = ScrollMode.Disabled;
                    TimelineGridViewScrollViewer.VerticalSnapPointsType = SnapPointsType.None;
                    TimelineGridViewScrollViewer.IsHorizontalRailEnabled = false;
                    TimelineGridViewScrollViewer.IsHorizontalScrollChainingEnabled = false;

                    TimelineGridViewScrollViewer.ZoomMode = ZoomMode.Disabled;
                }
                else
                {
                    VisualStateManager.GoToState(this, "FullScreenPortrait", true);
                    TimelineGridViewScrollViewer = VisualTreeUtilities.GetVisualChild<ScrollViewer>(TimelineGridView);
                    TimelineGridViewScrollViewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled;
                    TimelineGridViewScrollViewer.HorizontalScrollMode = ScrollMode.Disabled;
                    TimelineGridViewScrollViewer.HorizontalSnapPointsAlignment = SnapPointsAlignment.Center;
                    TimelineGridViewScrollViewer.HorizontalSnapPointsType = SnapPointsType.None;
                    TimelineGridViewScrollViewer.IsHorizontalRailEnabled = false;
                    TimelineGridViewScrollViewer.IsHorizontalScrollChainingEnabled = false;

                    TimelineGridViewScrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
                    TimelineGridViewScrollViewer.VerticalScrollMode = ScrollMode.Enabled;
                    TimelineGridViewScrollViewer.VerticalSnapPointsAlignment = SnapPointsAlignment.Center;
                    TimelineGridViewScrollViewer.VerticalSnapPointsType = SnapPointsType.Mandatory;
                    TimelineGridViewScrollViewer.IsHorizontalRailEnabled = true;
                    TimelineGridViewScrollViewer.IsHorizontalScrollChainingEnabled = true;

                    TimelineGridViewScrollViewer.ZoomMode = ZoomMode.Disabled;
                }

            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            // needed for back stack navigation when user has changed resolution on details page
            ProcessWindowSizeChangedEvent();
            this._navigationEventArgs = e;
        }

        private void TimelineGridView_Loaded(object sender, RoutedEventArgs e)
        {
            TimelineGridViewScrollViewer = VisualTreeUtilities.GetVisualChild<ScrollViewer>(TimelineGridView);
            if (_navigationEventArgs != null && _navigationEventArgs.Parameter is String)
            {
                ScrollToEventById(_navigationEventArgs.Parameter as String);
            }
            else
            {
                ScrollToFirstItem();
            }
        }

        private void ScrollToFirstItem()
        {
            var vm = GetDataContextAsViewModel<ITimelinePageViewModel>();
            // scroll to first event item, first item is buffer item
            if (vm.TimeLineItems.Count > 0)
                ScrollToEventById(vm.TimeLineItems[1].Id);
        }

        public override void PopulateTopAppbar(IBasePageViewModel vm)
        {
            base.PopulateTopAppbar(vm);
            AboutButton.Command = AboutButtonClickCommand;
            TwitterButton.Command = TwitterButtonClickCommand;
            InstagramButton.Command = InstagramButtonClickCommand;
        }

        #region Timeline UI

        void scrollerTimer_Tick(object sender, object e)
        {
            ShowTimelineMasks(true);
            scrollerTimer.Stop();
            AppBarIsAutoScrolling = false;
        }

        private void ShowTimelineMasks(bool show)
        {
            if (show)
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
                if (AppBarIsAutoScrolling == false)
                {
                    ShowTimelineMasks(true);
                    if (GetDataContextAsViewModel<TimelinePageViewModel>().IsHorizontal)
                    {
                        SelectItemByOffset(((ScrollViewer) sender).HorizontalOffset);
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
                i = Convert.ToInt32((offset - vm.BufferItemWidth)/vm.EventItemWidth);
            }
            else
            {
                i = Convert.ToInt32((offset - vm.BufferItemHeight) / vm.EventItemHeight);
            }
            vm.SelectedEvent = vm.TimeLineItems[i + 1]; // + 1 to skip the buffer item
        }

        private bool _semanticZoomClosedFromTopAppBarEvent;

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
                LogoGrid.Visibility = Visibility.Visible;
            }

        }

        private void semanticZoom_ViewChangeStarted(object sender, SemanticZoomViewChangedEventArgs e)
        {
            if (e.SourceItem.Item != null && e.IsSourceZoomedInView != true)
            {
                TimelineGridView.Opacity = 0;
            }
            LogoGrid.Visibility = Visibility.Collapsed;
        }

        public override async void TopAppBarEventButtonCommandHandler(String eventId)
        {
            Event ev = await AppLocator.Current.DataService.GetEventById(eventId, new CancellationToken());
            AppLocator.Current.Analytics.TrackAppBarInteractionInTimeline(ev.Name, ev.SquareFootage);

            if (semanticZoom.IsZoomedInViewActive == false)
            {
                _semanticZoomClosedFromTopAppBarEvent = true;
                semanticZoom.ToggleActiveView();
            }
            ScrollToEventById(eventId);
            TopAppBar.IsOpen = false;
            BottomAppBar.IsOpen = false;
        }

        private void ScrollToEventById(String eventId)
        {
            VideoPopup.IsOpen = false;

            if (String.IsNullOrEmpty(eventId))
            {
                // SCROLL HOME
                ScrollToFirstItem();
            }

            var vm = GetDataContextAsViewModel<TimelinePageViewModel>();
            var e = vm.SquareFootEvents.FirstOrDefault(x => x.Id == eventId);
            if (e == null)
                return;
            vm.SelectedEvent = e as EventItemViewModel;
            var itemIndex = vm.SquareFootEvents.IndexOf(e) + 1; // +1 for buffer items

            AppBarIsAutoScrolling = true;
            ShowTimelineMasks(false);
            if (vm.IsHorizontal)
            {
                TimelineGridViewScrollViewer.ChangeView(
                    ((itemIndex * vm.EventItemWidth) - vm.MaskItemWidth - vm.BufferItemWidth), 0, 1);
            }
            else
            {
                TimelineGridViewScrollViewer.ChangeView(
                    0, ((itemIndex * vm.EventItemHeight) - vm.MaskItemHeight - vm.BufferItemHeight), 1);
            }
            scrollerTimer.Start();
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
                ToggleAppBarButton(PinButton, isUnpinned);
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
                ToggleAppBarButton(PinButton, !isPinned);
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
    }
}
