using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.StartScreen;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media.Imaging;
using OneMSQFT.Common;
using OneMSQFT.Common.Models;
using OneMSQFT.Common.Services;
using OneMSQFT.UILogic.Interfaces.ViewModels;
using OneMSQFT.UILogic.Utils;
using OneMSQFT.UILogic.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

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
            vm.PinContextChanged += vm_PinContextChanged;
            var app = AppLocator.Current;
            if (app != null)
            {
                StartupButtonStackPanel.Visibility = app.KioskModeEnabled ? Visibility.Visible : Visibility.Collapsed;
                PinButton.Visibility = app.KioskModeEnabled ? Visibility.Collapsed : Visibility.Visible;
                if (!app.KioskModeEnabled)
                {
                    _sharing = AppLocator.Current.SharingService;
                    var dataTransferManager = DataTransferManager.GetForCurrentView();
                    dataTransferManager.DataRequested += DataTransferManagerOnDataRequested;
                }
            }
        }

        #region Sharing

        private ISharingService _sharing;
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
            }
            else
            { 
                if(String.IsNullOrEmpty(vm.Exhibit.ExhibitModel.EventId))
                {
                    args.Request.FailWithDisplayText(Strings.SharingFailedDisplayText);
                    return;
                }
                var deferral = args.Request.GetDeferral();                
                var evt = await AppLocator.Current.DataService.GetEventById(vm.Exhibit.ExhibitModel.EventId);
                if (evt == null)
                {
                    args.Request.FailWithDisplayText(Strings.SharingFailedDisplayText);
                    deferral.Complete();
                    return;
                }
                if (!_sharing.TryGetExhibitShareUri(evt, vm.Exhibit.ExhibitModel, out uri))
                {
                    args.Request.FailWithDisplayText(Strings.SharingFailedDisplayText);
                    return;
                }
                args.Request.Data.Properties.Title = vm.Exhibit.Name;
                args.Request.Data.Properties.Description = vm.Exhibit.Description;
                args.Request.Data.Properties.ContentSourceWebLink = uri;
                args.Request.Data.SetWebLink(uri);                
                deferral.Complete();
            }
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

        void ExhibitDetailsPage_Loaded(object sender, RoutedEventArgs e)
        {
            ProcessWindowSizeChangedEvent();
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

            if (e.PropertyName == "IsHorizontal")
            {
                if (GetDataContextAsViewModel<ExhibitDetailsPageViewModel>().IsHorizontal)
                {
                    VisualStateManager.GoToState(this, "FullScreenLandscape", true);
                }
                else
                {
                    VisualStateManager.GoToState(this, "FullScreenPortrait", true);
                }
            }
        }

        void vm_PinContextChanged(object sender, EventArgs e)
        {
            var args = GetDataContextAsViewModel<IBasePageViewModel>().GetSecondaryTileArguments();
            if (args == null)
                return;
            ToggleAppBarButton(this.PinButton, !SecondaryTile.Exists(args.Id));
        }

        async public override void TopAppBarEventButtonCommandHandler(String eventId)
        {
            //track appbar interaction
            Event ev = await AppLocator.Current.DataService.GetEventById(eventId);
            AppLocator.Current.Analytics.TrackAppBarInteractionInExhibitView(ev.Name, ev.SquareFootage);

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

        #region Resizing


        protected override void WindowSizeChanged(object sender, WindowSizeChangedEventArgs e)
        {
            ProcessWindowSizeChangedEvent();
            base.WindowSizeChanged(sender, e);
        }

        private void ProcessWindowSizeChangedEvent()
        {
            GetDataContextAsViewModel<ExhibitDetailsPageViewModel>().WindowSizeChanged(Window.Current.Bounds.Width, Window.Current.Bounds.Height);
        }

        #endregion

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
                ToggleAppBarButton(PinButton, isUnpinned);
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
                secondaryTile.VisualElements.Square70x70Logo = await RenderPinningLogo(70, 70);

                // Only support of the small and medium tile sizes is mandatory.
                // To have the larger tile sizes available the assets must be provided.                
                secondaryTile.VisualElements.Wide310x150Logo = await RenderPinningLogo(310, 150);

                // The display of the secondary tile name can be controlled for each tile size.
                // The default is false.                
                secondaryTile.VisualElements.ShowNameOnSquare150x150Logo = false;
                secondaryTile.VisualElements.ShowNameOnWide310x150Logo = false;         

                bool isPinned = await secondaryTile.RequestCreateForSelectionAsync(GetElementRect((FrameworkElement)sender));
                ToggleAppBarButton(PinButton, !isPinned);
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
            var hsv = ((ScrollViewer) sender);
            hsv.HorizontalSnapPointsAlignment = hsv.HorizontalOffset*2 > hsv.ScrollableWidth
                ? SnapPointsAlignment.Far
                : SnapPointsAlignment.Near;
        }
    }
}

