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

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace OneMSQFT.WindowsStore.Controls
{
    public sealed partial class SplashAnimationUserControl : UserControl
    {
        public SplashAnimationUserControl()
        {
            this.InitializeComponent();
            Mask.Completed += Mask_Completed;
        }

        private void Mask_Completed(object sender, object e)
        {
            BuildRectangles.Begin();
        }

        private void Start(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Mask.Begin();
        }

        private void Reverse(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Mask.AutoReverse = true;
            Mask.Begin();
        }
    }
}
