﻿using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.ApplicationSettings;
using Windows.UI.StartScreen;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using OneMSQFT.UILogic.Interfaces.ViewModels;
using OneMSQFT.UILogic.Utils;
using OneMSQFT.UILogic.ViewModels;
using Windows.UI.Core;

namespace OneMSQFT.WindowsStore.Views
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
            scrollerTimer.Interval = new TimeSpan(0, 0, 0, 0, 500);
            scrollerTimer.Tick += scrollerTimer_Tick;
            vm.PinContextChanged += VmOnPinContextChanged;

            var app = AppLocator.Current;
            if (app != null)
            {
                StartupButtonStackPanel.Visibility = app.KioskModeEnabled ? Visibility.Visible : Visibility.Collapsed;
                PinButton.Visibility = app.KioskModeEnabled ? Visibility.Collapsed : Visibility.Visible;
            }
        }

        private void VmOnPinContextChanged(object sender, EventArgs eventArgs)
        {
            var args = GetDataContextAsViewModel<IBasePageViewModel>().GetSecondaryTileArguments();
            if (args == null)
                return;
            ToggleAppBarButton(!SecondaryTile.Exists(args.Id));
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
            var vm = GetDataContextAsViewModel<ITimelinePageViewModel>();
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
                if (vm.TimeLineItems.Count>0)
                    ScrollToEventById(vm.TimeLineItems[1].Id);
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

        public override void TopAppBarEventButtonCommandHandler(String eventId)
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
                _timelineGridViewScrollViewer.ChangeView(0, 0, 1);
            }

            var e = vm.SquareFootEvents.FirstOrDefault(x => x.Id == eventId);
            if (e == null)
                return;
            vm.SelectedEvent = e as EventItemViewModel;
            var itemIndex = vm.SquareFootEvents.IndexOf(e) + 1; // +1 for buffer items

            AppBarIsAutoScrolling = true;
            ShowTimelineMasks(false);
            _timelineGridViewScrollViewer.ChangeView(((itemIndex * vm.EventItemWidth) - vm.MaskItemWidth - vm.BufferItemWidth), 0, 1);
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
                VideoPlayerUserControl.SelectedMediaContentSource = ((MediaContentSourceItemViewModel)FlipViewer.SelectedItem);
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

        async private void Pin_OnClick(object sender, RoutedEventArgs e)
        {
            this.BottomAppBar.IsSticky = true;

            var vm = GetDataContextAsViewModel<IBasePageViewModel>();
            var args = vm.GetSecondaryTileArguments();

            if (SecondaryTile.Exists(args.Id))
            {
                SecondaryTile secondaryTile = new SecondaryTile(args.Id);
                bool isUnpinned = await secondaryTile.RequestDeleteForSelectionAsync(GetElementRect((FrameworkElement)sender));

                ToggleAppBarButton(isUnpinned);
            }
            else
            {
                var secondaryTileImages = await vm.GetSecondaryTileImages();

                SecondaryTile secondaryTile = new SecondaryTile(args.Id,
                                                                args.DisplayName,
                                                                args.ArgumentsName,
                                                                await RenderBitmaps(150, 150),
                                                                TileSize.Square150x150);

                secondaryTile.ForegroundText = ForegroundText.Dark;
                secondaryTile.SmallLogo = await RenderBitmaps(70, 70);
                secondaryTile.WideLogo = await RenderBitmaps(310, 150);
                secondaryTile.BackgroundColor = ColorUtils.GetColorFromARGBString(args.BackgroundColor, Colors.White);

                bool isPinned = await secondaryTile.RequestCreateForSelectionAsync(GetElementRect((FrameworkElement)sender));

                ToggleAppBarButton(!isPinned);
            }
            this.BottomAppBar.IsSticky = false;

        }

        public static Rect GetElementRect(FrameworkElement element)
        {
            GeneralTransform buttonTransform = element.TransformToVisual(null);
            Point point = buttonTransform.TransformPoint(new Point());
            return new Rect(point, new Size(element.ActualWidth, element.ActualHeight));
        }

        private void ToggleAppBarButton(bool showPinButton)
        {
            if (this.PinButton != null)
            {
                PinButton.Style = (showPinButton) ? ((Style)App.Current.Resources["OMSQFTTraditionalPinAppBarButtonStyle"]) : ((Style)App.Current.Resources["OMSQFTTraditionalUnPinAppBarButtonStyle"]);
            }
        }

        private async Task<Uri> RenderBitmaps(uint width, uint height)
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
                var file = await folder.CreateFileAsync(fileName, CreationCollisionOption.GenerateUniqueName);
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

        public static async Task<bool> RenderTargetBitmapToStorageFile(StorageFile storageFile, RenderTargetBitmap bitmap, uint width, uint height)
        {
            try
            {
                Guid encoderId = Windows.Graphics.Imaging.BitmapEncoder.JpegEncoderId;                

                using (var stream = await storageFile.OpenAsync(FileAccessMode.ReadWrite))
                {
                    stream.Size = 0;

                    var encoder = await Windows.Graphics.Imaging.BitmapEncoder.CreateAsync(
                        encoderId,
                        stream
                        );

                    var pixels = await bitmap.GetPixelsAsync();
                    encoder.SetPixelData(
                        Windows.Graphics.Imaging.BitmapPixelFormat.Bgra8,
                        Windows.Graphics.Imaging.BitmapAlphaMode.Premultiplied,
                        width, // pixel width
                        height, // pixel height
                        72, // horizontal DPI
                        72, // vertical DPI
                        pixels.ToArray()
                    );

                    try
                    {
                        await encoder.FlushAsync();
                        return true;
                    }
                    catch (Exception err)
                    {
                        // There was an error encoding.
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                Debug.WriteLine("RenderTargetBitmapToStorageFile Exception: " + ex.ToString());
#endif
            }
            return false;
        }




        public static string GetStorageFileNameFromUri(Uri uri)
        {
            var segments = uri.AbsoluteUri.Split('?');
            var shortUri = segments[0];
            shortUri = WebUtility.UrlDecode(shortUri);
            shortUri = String.Join("", shortUri.Split(new string[] { "http://", "https://" }, StringSplitOptions.RemoveEmptyEntries));
            shortUri = String.Join("-", shortUri.Split(new string[] { ":", "/" }, StringSplitOptions.RemoveEmptyEntries));
            return shortUri;
        }

    }
}
