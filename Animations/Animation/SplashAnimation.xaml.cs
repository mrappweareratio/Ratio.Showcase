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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Animation
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            this.Loaded += SplashAnimationLoaded;
            Mask.Completed += MaskCompleted;
            BuildRectangles.Completed += BuildRectanglesOnCompleted;
        }

        private void BuildRectanglesOnCompleted(object sender, object o)
        {
            this.Visibility = Visibility.Collapsed;
        }

        private void SplashAnimationLoaded(object sender, RoutedEventArgs e)
        {
            Mask.Begin();
        }

        private void MaskCompleted(object sender, object e)
        {
            BuildRectangles.Begin();
        }
    }
}
