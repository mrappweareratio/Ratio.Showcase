﻿using System;
using Microsoft.Practices.Prism.StoreApps;
using OneMSQFT.Common.Models;
using OneMSQFT.UILogic.Interfaces.ViewModels;
using OneMSQFT.UILogic.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace OneMSQFT.Windows.Views
{
    public partial class ExhibitDetailsPage : BasePageView
    {
        public ExhibitDetailsPage()
        {
            this.InitializeComponent();
            InitAppBars();
            Loaded += ExhibitDetailsPage_Loaded;
        }       

        void ExhibitDetailsPage_Loaded(object sender, RoutedEventArgs e)
        {
            ((ExhibitDetailsPageViewModel)this.DataContext).PropertyChanged +=ExhibitDetailsPage_PropertyChanged;
            var vm = this.DataContext as IExhibitDetailsPageViewModel;
            if (vm != null)
            {
                vm.WindowSizeChanged(Window.Current.Bounds.Width, Window.Current.Bounds.Height);
            }
        }

        void ExhibitDetailsPage_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SquareFootEvents")
            {
                if (((ExhibitDetailsPageViewModel)this.DataContext).SquareFootEvents.Count > 0)
                {
                    this.PopulateTopAppbar(((BasePageViewModel)this.DataContext));
                }
            }
        }

        public override async void TopAppBarEventButtonCommandHandler(String eventId)
        {
            this.Frame.Navigate(typeof(TimelinePage), eventId);
            TopAppBar.IsOpen = false;
            BottomAppBar.IsOpen = false;
        }

        public override void PopulateTopAppbar(BasePageViewModel vm)
        {
            base.PopulateTopAppbar(vm);
            this.HomeButton.Command = this.HomeButtonClickCommand;
            this.AboutButton.Command = this.AboutButtonClickCommand;
        }

        private void LaunchVideoCommand_OnClick(object sender, RoutedEventArgs e)
        {
            if (!VideoPopup.IsOpen)
            {
                ExhibitDetailsPanels.Opacity = 0;
                VideoPopup.IsOpen = true;
            }
        }

        private void CloseVideo_OnClick(object sender, RoutedEventArgs e)
        {
            if (VideoPopup.IsOpen)
            {
                VideoPopup.IsOpen = false;
            }
        }

        private void VideoPopup_Closed(object sender, object e)
        {
            ExhibitDetailsPanels.Opacity = 1;
        }
    }
}
