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
using OneMSQFT.UILogic.ViewModels;

namespace OneMSQFT.Windows.Views
{
    public sealed partial class TimelinePage : BasePageView
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
            PopulateTopAppbar(((TimelinePageViewModel)this.DataContext));
        }

        private void itemsGridView_Loaded(object sender, RoutedEventArgs e)
        {
            _timelineGridViewScrollViewer = VisualTreeUtilities.GetVisualChild<ScrollViewer>(itemsGridView);
            _timelineGridViewScrollViewer.ViewChanged += _timelineGridViewScrollViewer_ViewChanged;
        }

        void _timelineGridViewScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            this.StoryboardSeeker.Resume();
            int offSet = Convert.ToInt32(_timelineGridViewScrollViewer.HorizontalOffset);
            this.StoryboardSeeker.Seek(new TimeSpan(0, 0, offSet));
            this.StoryboardSeeker.Pause();
        }

    }
}
