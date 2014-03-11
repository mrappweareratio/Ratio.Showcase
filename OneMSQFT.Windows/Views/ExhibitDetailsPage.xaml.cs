using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.StartScreen;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Microsoft.Practices.Prism.StoreApps;
using OneMSQFT.Common;
using OneMSQFT.Common.Models;
using OneMSQFT.Common.Services;
using OneMSQFT.UILogic.Interfaces.ViewModels;
using OneMSQFT.UILogic.Services;
using OneMSQFT.UILogic.Utils;
using OneMSQFT.UILogic.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace OneMSQFT.WindowsStore.Views
{
    public partial class ExhibitDetailsPage : BasePageView
    {
        private bool _loaded;
        private ScrollViewer _detailsGridViewScrollViewer;

        public ExhibitDetailsPage()
        {
            InitializeComponent();
            InitAppBars();

            var vm = GetDataContextAsViewModel<IExhibitDetailsPageViewModel>();
            vm.PropertyChanged += ExhibitDetailsPage_PropertyChanged;
            vm.PinContextChanged += VmPinContextChanged;
            ProcessWindowSizeChangedEvent();
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
        }
        private void MediaEndedCommandHandler()
        {
            VideoPopup.IsOpen = false;
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {            
            base.OnNavigatedTo(e);                        
            if (!AppLocator.Current.KioskModeEnabled)
            {
                _dataTransferManager.DataRequested += DataTransferManagerOnDataRequested;
                _dataTransferManager.TargetApplicationChosen += DataTransferManagerTargetApplicationChosen;
            }

            _mediaListViewLoadedTaskCompletionSource = new TaskCompletionSource<bool>();
            Task.WhenAll(_mediaListViewLoadedTaskCompletionSource.Task,
                GetDataContextAsViewModel<IBasePageViewModel>().LoadedEventsTaskCompletionSource.Task)
                .ContinueWith((task) => SetIndicatorTextByOffset(0), TaskScheduler.FromCurrentSynchronizationContext());
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            if (!AppLocator.Current.KioskModeEnabled)
            {
                _dataTransferManager.DataRequested -= DataTransferManagerOnDataRequested;
                _dataTransferManager.TargetApplicationChosen -= DataTransferManagerTargetApplicationChosen;
            }
        }

        #region Sharing

        private readonly ISharingService _sharing;
        async private void DataTransferManagerOnDataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            var vm = GetDataContextAsViewModel<IExhibitDetailsPageViewModel>();
            if (vm.Exhibit == null)
            {
                args.Request.FailWithDisplayText(Strings.SharingFailedDisplayText);
                return;
            }

            Uri uri = null;
            if (VideoPopup.IsOpen)
            {
                var selectedMediaContentSource = vm.SelectedMediaContentSource;
                if (selectedMediaContentSource == null || !_sharing.TryGetVideoShareUri(selectedMediaContentSource.Media, out uri))
                {
                    args.Request.FailWithDisplayText(Strings.SharingFailedDisplayText);
                    return;
                }
                args.Request.Data.Properties.Title = String.Format(Strings.SharingTitleVideoFromEventFormat, vm.Exhibit.Name);
                args.Request.Data.Properties.Description = vm.Exhibit.Description;
                args.Request.Data.Properties.ContentSourceWebLink = uri;
                args.Request.Data.SetWebLink(uri);
                //get thumbnail for video;
                Uri videoThumbnailUri;
                if (_sharing.TryGetSharingThumbnailUri(selectedMediaContentSource.Media, out videoThumbnailUri))
                {
                    RandomAccessStreamReference imageStreamRef = RandomAccessStreamReference.CreateFromUri(videoThumbnailUri);
                    args.Request.Data.Properties.Thumbnail = imageStreamRef;
                    args.Request.Data.SetBitmap(imageStreamRef);
                }  
                _targetApplicationChosenDelegate = appName => AppLocator.Current.Analytics.TrackVideoShareInExhibitView(vm.Exhibit.Name,
                    selectedMediaContentSource.Media.VideoId, uri.AbsoluteUri, appName);
            }
            else
            {
                if (String.IsNullOrEmpty(vm.Exhibit.ExhibitModel.EventId))
                {
                    args.Request.FailWithDisplayText(Strings.SharingFailedDisplayText);
                    return;
                }
                var deferral = args.Request.GetDeferral();
                var evt = await AppLocator.Current.DataService.GetEventById(vm.Exhibit.ExhibitModel.EventId, new CancellationToken());
                if (evt == null)
                {
                    args.Request.FailWithDisplayText(Strings.SharingFailedDisplayText);
                    deferral.Complete();
                    return;
                }
                if (!_sharing.TryGetExhibitShareUri(evt, vm.Exhibit.ExhibitModel, out uri))
                {
                    args.Request.FailWithDisplayText(Strings.SharingFailedDisplayText);
                    deferral.Complete();
                    return;
                }
                args.Request.Data.Properties.Title = vm.Exhibit.Name;
                args.Request.Data.Properties.Description = vm.Exhibit.Description;
                args.Request.Data.Properties.ContentSourceWebLink = uri;
                args.Request.Data.SetWebLink(uri);
                //get thumbnail for exhibit;
                Uri thumbnailUri;
                if (_sharing.TryGetSharingThumbnailUri(vm.Exhibit.ExhibitModel, out thumbnailUri))
                {
                    RandomAccessStreamReference imageStreamRef = RandomAccessStreamReference.CreateFromUri(thumbnailUri);
                    args.Request.Data.Properties.Thumbnail = imageStreamRef;
                    args.Request.Data.SetBitmap(imageStreamRef);
                }  
                _targetApplicationChosenDelegate = appName => AppLocator.Current.Analytics.TrackShareExhibitInteraction(vm.Exhibit.Name,
                  uri.AbsoluteUri, appName);
                deferral.Complete();
            }
        }

        private Action<String> _targetApplicationChosenDelegate;
        private readonly DataTransferManager _dataTransferManager;

        void DataTransferManagerTargetApplicationChosen(DataTransferManager sender, TargetApplicationChosenEventArgs args)
        {
            if (_targetApplicationChosenDelegate == null)
                return;
            _targetApplicationChosenDelegate(args.ApplicationName);
            //unwire and reset the delegate
            _targetApplicationChosenDelegate = null;
        }

        #endregion

        protected override void GoBack(object sender, RoutedEventArgs eventArgs)
        {
            if (this.Frame != null && !this.Frame.CanGoBack)
            {
                Frame.Navigate(typeof(TimelinePage), null);
                return;
            }
            base.GoBack(sender, eventArgs);
        }

        void ExhibitDetailsPage_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SquareFootEvents")
            {
                if (GetDataContextAsViewModel<IBasePageViewModel>().SquareFootEvents.Count > 0)
                {
                    this.PopulateTopAppbar(GetDataContextAsViewModel<IBasePageViewModel>());
                }                
            }

            if (e.PropertyName == "IsHorizontal")
            {
                if (!_loaded)
                    return;

                _detailsGridViewScrollViewer = _detailsGridViewScrollViewer ?? VisualTreeUtilities.GetVisualChild<ScrollViewer>(MediaListView);

                if (GetDataContextAsViewModel<IBasePageViewModel>().IsHorizontal)
                {
                    VisualStateManager.GoToState(this, "FullScreenLandscape", true);
                    
                    StackPanelRightAppBarImages.Visibility = PinButtonImage.Visibility = Visibility.Collapsed;
                    StackPanelRightAppBarText.Visibility = PinButton.Visibility = Visibility.Visible;

                    if (_detailsGridViewScrollViewer != null)
                    {
                        _detailsGridViewScrollViewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Visible;
                        _detailsGridViewScrollViewer.HorizontalScrollMode = ScrollMode.Enabled;
                        _detailsGridViewScrollViewer.HorizontalSnapPointsAlignment = SnapPointsAlignment.Center;
                        _detailsGridViewScrollViewer.HorizontalSnapPointsType = SnapPointsType.Mandatory;
                        _detailsGridViewScrollViewer.IsHorizontalRailEnabled = true;
                        _detailsGridViewScrollViewer.IsHorizontalScrollChainingEnabled = true;

                        _detailsGridViewScrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Disabled;
                        _detailsGridViewScrollViewer.VerticalScrollMode = ScrollMode.Disabled;
                        _detailsGridViewScrollViewer.VerticalSnapPointsType = SnapPointsType.None;
                        _detailsGridViewScrollViewer.IsVerticalRailEnabled = false;
                        _detailsGridViewScrollViewer.IsVerticalScrollChainingEnabled = false;

                        _detailsGridViewScrollViewer.ZoomMode = ZoomMode.Disabled;
                    }
                }
                else
                {
                    VisualStateManager.GoToState(this, "FullScreenPortrait", true);             
                    
                    StackPanelRightAppBarText.Visibility = PinButton.Visibility = Visibility.Collapsed;
                    StackPanelRightAppBarImages.Visibility = PinButtonImage.Visibility = Visibility.Visible;

                    if (_detailsGridViewScrollViewer != null)
                    {
                        _detailsGridViewScrollViewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled;
                        _detailsGridViewScrollViewer.HorizontalScrollMode = ScrollMode.Disabled;
                        _detailsGridViewScrollViewer.HorizontalSnapPointsAlignment = SnapPointsAlignment.Center;
                        _detailsGridViewScrollViewer.HorizontalSnapPointsType = SnapPointsType.None;
                        _detailsGridViewScrollViewer.IsHorizontalRailEnabled = false;
                        _detailsGridViewScrollViewer.IsHorizontalScrollChainingEnabled = false;

                        _detailsGridViewScrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
                        _detailsGridViewScrollViewer.VerticalScrollMode = ScrollMode.Enabled;
                        _detailsGridViewScrollViewer.VerticalSnapPointsType = SnapPointsType.None;
                        _detailsGridViewScrollViewer.IsVerticalRailEnabled = true;
                        _detailsGridViewScrollViewer.IsVerticalScrollChainingEnabled = true;

                        _detailsGridViewScrollViewer.ZoomMode = ZoomMode.Disabled;
                    }
                }
            }
        }

        void VmPinContextChanged(object sender, EventArgs e)
        {
            var args = GetDataContextAsViewModel<IBasePageViewModel>().GetSecondaryTileArguments();
            if (args == null)
                return;
            ToggleAppBarButton(this.PinButton, this.PinButtonImage, !SecondaryTile.Exists(args.Id));
        }

        public override void TopAppBarEventButtonCommandHandler(String eventId)
        {
            var ev = GetDataContextAsViewModel<IBasePageViewModel>().SquareFootEvents.FirstOrDefault(x => x.Id == eventId);
            AppLocator.Current.Analytics.TrackAppBarEventInteraction(ev.Name, ev.SquareFootage, GetDataContextAsViewModel<IBasePageViewModel>().GetEventIndexById(ev.Id), "exhibit");

            Frame.Navigate(typeof(TimelinePage), eventId);
            TopAppBar.IsOpen = false;
            BottomAppBar.IsOpen = false;
        }

        public override void PopulateTopAppbar(IBasePageViewModel vm)
        {
            base.PopulateTopAppbar(vm);
            HomeButton.Command = HomeButtonClickCommand;
            AboutButton.Command = AboutButtonImage.Command = AboutButtonClickCommand;
            FacebookButton.Command = FacebookButtonImage.Command = FacebookButtonClickCommand;
            TwitterButton.Command = TwitterButtonImage.Command = TwitterButtonClickCommand;
            InstagramButton.Command = InstagramButtonImage.Command = InstagramButtonClickCommand;
        }

        #region MediaViewer

        private void LaunchVideoCommand_OnClick(object sender, RoutedEventArgs e)
        {
            if (!VideoPopup.IsOpen)
            {
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

        }

        #endregion

        #region Pinning

        private async void Pin_OnClick(object sender, RoutedEventArgs e)
        {
            BottomAppBar.IsSticky = true;

            var vm = GetDataContextAsViewModel<IBasePageViewModel>();
            var args = vm.GetSecondaryTileArguments();
            var analytics = AppLocator.Current.Analytics;

            if (SecondaryTile.Exists(args.Id))
            {
                //track Exhibit unpinning interaction
                if (analytics != null)
                    analytics.TrackUnPinExhibitInteraction(args.DisplayName);

                var secondaryTile = new SecondaryTile(args.Id);
                bool isUnpinned = await secondaryTile.RequestDeleteForSelectionAsync(GetElementRect((FrameworkElement)sender));
                ToggleAppBarButton(PinButton, PinButtonImage, isUnpinned);
            }
            else
            {
                var square150x150Logo = await RenderPinningLogo(150, 150);
                //track Exhibit pinning interaction
                if (analytics != null)
                    analytics.TrackPinExhibitInteraction(args.DisplayName);

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
                // secondaryTile.VisualElements.Square70x70Logo = await RenderPinningLogo(70, 70);
                // scaling down medium tile as mentioned above.

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
            var vm = GetDataContextAsViewModel<ExhibitDetailsPageViewModel>();
            RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap();

            if (width == 70)
            {
                RenderedGridSmall.Background = new SolidColorBrush(vm.Exhibit.ExhibitColor);
                await renderTargetBitmap.RenderAsync(RenderedGridSmall, 70, 70);
            }
            else if (width == 150)
            {
                RenderedGridSquare.Background = new SolidColorBrush(vm.Exhibit.ExhibitColor);
                await renderTargetBitmap.RenderAsync(RenderedGridSquare, 150, 150);
            }
            else if (width == 310)
            {
                RenderedGridWide.Background = new SolidColorBrush(vm.Exhibit.ExhibitColor);
                await renderTargetBitmap.RenderAsync(RenderedGridWide, 310, 150);
            }

            // create or get the named folder under LocalFolder
            var folder = await ApplicationData.Current.LocalFolder.CreateFolderAsync("SecondaryTiles", CreationCollisionOption.OpenIfExists);
            try
            {
                // create the file from the Uri                                                                                                
                var fileName = String.Format("exhibit_{0}_{1}x{2}.jpg", vm.Exhibit.Id, width, height);
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

        private void MediaListViewScrollViewerHorizontal_ViewChanging(object sender, ScrollViewerViewChangingEventArgs e)
        {
            var hsv = ((ScrollViewer)sender);
            hsv.HorizontalSnapPointsAlignment = hsv.HorizontalOffset * 2 > hsv.ScrollableWidth
                ? SnapPointsAlignment.Far
                : SnapPointsAlignment.Near;
        }

        private TaskCompletionSource<bool> _mediaListViewLoadedTaskCompletionSource; 
        private void MediaListView_OnLoaded(object sender, RoutedEventArgs e)
        {
            _detailsGridViewScrollViewer = VisualTreeUtilities.GetVisualChild<ScrollViewer>(MediaListView);
            if (_detailsGridViewScrollViewer != null)
            {
                _loaded = true;
                _detailsGridViewScrollViewer.ViewChanged += MediaListViewScrollViewer_ViewChanged;
                ProcessWindowSizeChangedEvent();
                _mediaListViewLoadedTaskCompletionSource.TrySetResult(true);
            }
        }

        private void MediaListView_OnUnloaded(object sender, RoutedEventArgs e)
        {
            if (_detailsGridViewScrollViewer != null)
            {                
                _detailsGridViewScrollViewer.ViewChanged -= MediaListViewScrollViewer_ViewChanged;                                
            }
        }

        void MediaListViewScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            if (e.IsIntermediate == false && GetDataContextAsViewModel<IBasePageViewModel>().LoadedEventsTaskCompletionSource.Task.IsCompleted)
            {
                SetIndicatorTextByOffset(((ScrollViewer)sender).HorizontalOffset);
            }
        }

        /// <summary>
        /// Examines HorizontalOffset of the page to determine the # of media item shown.
        /// Bound media items include [exhibit buffer][media list][next exhibit buffer]
        /// </summary>
        /// <param name="offset">HorizontalOffset</param>
        private void SetIndicatorTextByOffset(double offset)
        {
            var vm = GetDataContextAsViewModel<IExhibitDetailsPageViewModel>();
            if (vm.Exhibit == null || vm.Exhibit.MediaContent == null) return;
            var i = Math.Floor(offset / vm.FullScreenWidth);//zero base index
            var mediaCount = vm.Exhibit.MediaContent.Count - 2;
            if (i > mediaCount - 1)
            {                
                i = mediaCount - 1;//last item zero index
            }
            Index.Text = (i + 1).ToString();
            Count.Text = (mediaCount).ToString();
        }        
    }
}

