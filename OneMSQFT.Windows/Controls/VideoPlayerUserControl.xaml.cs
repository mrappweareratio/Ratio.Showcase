using Microsoft.PlayerFramework;
using OneMSQFT.UILogic.ViewModels;
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

namespace OneMSQFT.Windows.Controls
{
    public sealed partial class VideoPlayerUserControl : UserControl
    {
        public VideoPlayerUserControl()
        {
            this.InitializeComponent();
        }

        private void player_Loaded(object sender, RoutedEventArgs e)
        {
            if (_selectedEvent != null)
            {
                player.Source = _selectedEvent.EventHeroVideoUri;
                player.Play();
            }
        }

        private EventItemViewModel _selectedEvent;
        public EventItemViewModel SelectedEvent
        {
            get { return (EventItemViewModel)GetValue(SelectedEventProperty); }
            set { SetValue(SelectedEventProperty, value); }
        }

        public static readonly DependencyProperty SelectedEventProperty =
            DependencyProperty.Register("SelectedEvent", typeof(EventItemViewModel), typeof(VideoPlayerUserControl), new PropertyMetadata(null, SelectedEventPropertyChanged));

        private static void SelectedEventPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            VideoPlayerUserControl vpuc = d as VideoPlayerUserControl;
            if (vpuc == null) return;
            vpuc._selectedEvent = e.NewValue as EventItemViewModel;
            vpuc.player.Source = vpuc._selectedEvent.EventHeroVideoUri;
        }
        
    }
}
