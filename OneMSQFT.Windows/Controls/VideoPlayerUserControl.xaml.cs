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

        private void player_Loaded(object sender, RoutedEventArgs e)
        {            
            if (_selectedMediaContentSource != null)
            {
                player.Source = _selectedMediaContentSource.VideoSource;
                player.Play();
            }
            var app = AppLocator.Current;
            if (app != null && !app.KioskModeEnabled)
            {
                player.MediaEnded += player_MediaEnded;
            }
        }

        void player_MediaEnded(object sender, MediaPlayerActionEventArgs e)
        {
            if (MediaEndedCommand != null)
            {
                if (MediaEndedCommand.CanExecute())
                {
                    MediaEndedCommand.Execute();
                }
            }
        }

        public Microsoft.Practices.Prism.StoreApps.DelegateCommand MediaEndedCommand;

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
                vpuc.player.Source = vpuc._selectedMediaContentSource.VideoSource;
            }
        }

    }
}
