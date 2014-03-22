using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Ratio.Showcase.Win8.Controls
{
    public sealed partial class SplashAnimationUserControl : UserControl
    {
        public SplashAnimationUserControl()
        {
            this.InitializeComponent();
            this.Loaded += SplashAnimationLoaded;
            Mask.Completed += Mask_Completed;
            BuildRectangles.Completed += BuildRectanglesOnCompleted;
            RevealAndExit.Completed += RevealAndExitOnCompleted;
        }

        private void RevealAndExitOnCompleted(object sender, object o)
        {
            this.Visibility = Visibility.Collapsed;
        }

        private void BuildRectanglesOnCompleted(object sender, object o)
        {
            RevealAndExit.Begin();
        }

        private void SplashAnimationLoaded(object sender, RoutedEventArgs e)
        {
            Mask.Begin();
        }

        private void Mask_Completed(object sender, object e)
        {
            BuildRectangles.Begin();
        }
    }
}
