using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.StartScreen;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media.Imaging;
using Microsoft.Practices.Prism.StoreApps;
using OneMSQFT.Common.Models;
using OneMSQFT.UILogic.Interfaces.ViewModels;
using OneMSQFT.UILogic.Utils;
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
            vm.PinContextChanged += vm_PinContextChanged;
            var app = AppLocator.Current;
            if (app != null)
            {
                StartupButtonStackPanel.Visibility = app.KioskModeEnabled ? Visibility.Visible : Visibility.Collapsed;
                PinButton.Visibility = app.KioskModeEnabled ? Visibility.Collapsed : Visibility.Visible;
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

            if (SecondaryTile.Exists(args.Id))
            {
                var secondaryTile = new SecondaryTile(args.Id);
                bool isUnpinned = await secondaryTile.RequestDeleteForSelectionAsync(GetElementRect((FrameworkElement)sender));
                ToggleAppBarButton(PinButton, isUnpinned);
            }
            else
            {
                var secondaryTile = new SecondaryTile(args.Id,
                                                      args.DisplayName,
                                                      args.ArgumentsName,
                                                      await RenderBitmaps(150, 150),
                                                      TileSize.Square150x150)
                {
                    ForegroundText = ForegroundText.Dark,
                    SmallLogo = await RenderBitmaps(70, 70),
                    WideLogo = await RenderBitmaps(310, 150),
                    BackgroundColor = ColorUtils.GetColorFromARGBString(args.BackgroundColor, Colors.White)
                };

                bool isPinned = await secondaryTile.RequestCreateForSelectionAsync(GetElementRect((FrameworkElement)sender));
                ToggleAppBarButton(PinButton, !isPinned);
            }
            BottomAppBar.IsSticky = false;
        }

        private async Task<Uri> RenderBitmaps(uint width, uint height)
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
                var file = await folder.CreateFileAsync(fileName, CreationCollisionOption.OpenIfExists);
                if ((await file.GetBasicPropertiesAsync()).Size > 0 || await RenderTargetBitmapToStorageFile(file, renderTargetBitmap, width, height))
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

        void MediaListViewScrollViewerHorizontal_ViewChanging(object sender, ScrollViewerViewChangingEventArgs e)
        {
            var hsv = ((ScrollViewer)sender);
            hsv.HorizontalSnapPointsAlignment = hsv.HorizontalOffset * 2 > hsv.ScrollableWidth ? SnapPointsAlignment.Far : SnapPointsAlignment.Near;
        }

        void MediaListViewScrollViewerVertical_ViewChanging(object sender, ScrollViewerViewChangingEventArgs e)
        {
            var vsv = ((ScrollViewer)sender);
            vsv.VerticalSnapPointsAlignment = vsv.VerticalOffset * 2 > vsv.ScrollableHeight+1 ? SnapPointsAlignment.Far : SnapPointsAlignment.Near;
        }
    }
}
