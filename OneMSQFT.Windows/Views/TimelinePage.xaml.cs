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

namespace OneMSQFT.Windows.Views
{
    public sealed partial class TimelinePage : BasePageView
    {
        public TimelinePage()
        {
            this.InitializeComponent();
           // this.StoryboardSeeker.Begin();
            InitAppBar();
            PopulateTopAppbar(8);
        }

        private void scrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            //this.StoryboardSeeker.Resume();
            //int offSet = Convert.ToInt32(scrollViewer.HorizontalOffset);
            //this.StoryboardSeeker.Seek(new TimeSpan(0, 0, offSet));
            //this.StoryboardSeeker.Pause();
            //tb.Text = offSet.ToString();
        }
    }
}
