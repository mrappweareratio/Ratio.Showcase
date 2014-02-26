﻿using Windows.ApplicationModel.DataTransfer;
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
                player.MediaEnded += (o, args) => DataTransferManager.ShowShareUI();
            }
        }               

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
