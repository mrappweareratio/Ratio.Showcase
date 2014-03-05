using System.Windows.Input;
using Microsoft.PlayerFramework;
using OneMSQFT.UILogic.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Microsoft.Practices.Prism.StoreApps;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace OneMSQFT.WindowsStore.Controls
{
    public sealed partial class VideoPlayerUserControl : UserControl
    {
        public VideoPlayerUserControl()
        {
            this.InitializeComponent();
        }

        private void PlayerLoaded(object sender, RoutedEventArgs e)
        {
            var app = AppLocator.Current;            
            player.MediaEnded += PlayerMediaEnded;
            if (_selectedMediaContentSource == null) 
                return;
            player.Source = app == null ? _selectedMediaContentSource.VideoSource : _selectedMediaContentSource.GetVideoSourceByInternetConnection(app.InternetConnection);
            //auto play
            player.Play();
        }

        void PlayerMediaEnded(object sender, MediaPlayerActionEventArgs e)
        {
            if (MediaEndedCommand == null) return;
            if (MediaEndedCommand.CanExecute(null))
            {
                MediaEndedCommand.Execute(null);
            }
        }

        public ICommand MediaEndedCommand;

        private MediaContentSourceItemViewModel _selectedMediaContentSource;
        public MediaContentSourceItemViewModel SelectedMediaContentSource
        {
            get { return (MediaContentSourceItemViewModel)GetValue(SelectedMediaContentSourceProperty); }
            set { SetValue(SelectedMediaContentSourceProperty, value); }
        }

        public static readonly DependencyProperty SelectedMediaContentSourceProperty =
            DependencyProperty.Register("SelectedMediaContentSource", typeof(MediaContentSourceItemViewModel), typeof(VideoPlayerUserControl), new PropertyMetadata(null, SelectedMediaContentSourcePropertyChanged));

        // MediaContentSourceItemViewModel changed from Exhibits Page
        private static void SelectedMediaContentSourcePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var vpuc = d as VideoPlayerUserControl;
            if (vpuc == null) return;
            vpuc._selectedMediaContentSource = e.NewValue as MediaContentSourceItemViewModel;
            if (vpuc._selectedMediaContentSource != null)
            {
                var app = AppLocator.Current;
                vpuc.player.Source = app == null ? vpuc._selectedMediaContentSource.VideoSource : vpuc._selectedMediaContentSource.GetVideoSourceByInternetConnection(app.InternetConnection);
            }
        }

    }
}
