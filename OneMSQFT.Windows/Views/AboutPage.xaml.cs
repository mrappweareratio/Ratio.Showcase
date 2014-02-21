using System;
using Microsoft.Practices.Prism.StoreApps;
using OneMSQFT.UILogic.Interfaces.ViewModels;
using OneMSQFT.UILogic.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace OneMSQFT.Windows.Views
{
    public partial class AboutPage : BasePageView
    {
        public AboutPage()
        {
            this.InitializeComponent();
            InitAppBars();
            Loaded += AboutPage_Loaded;
            var vm = GetDataContextAsViewModel<IBasePageViewModel>();
            vm.PropertyChanged += AboutPage_PropertyChanged;
            var app = AppLocator.Current;
            if (app != null)
            {
                HomeButton.Visibility = app.KioskModeEnabled ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        void AboutPage_Loaded(object sender, RoutedEventArgs e)
        {
            GetDataContextAsViewModel<IBasePageViewModel>().WindowSizeChanged(Window.Current.Bounds.Width, Window.Current.Bounds.Height);
        }

        void AboutPage_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SquareFootEvents")
            {
                if (GetDataContextAsViewModel<IBasePageViewModel>().SquareFootEvents.Count > 0)
                {
                    this.PopulateTopAppbar(GetDataContextAsViewModel<IBasePageViewModel>());
                }
            }
        }

        public override void TopAppBarEventButtonCommandHandler(string eventId)
        {
            this.Frame.Navigate(typeof(TimelinePage), eventId);
            TopAppBar.IsOpen = false;
            BottomAppBar.IsOpen = false;
        }

        public override void PopulateTopAppbar(IBasePageViewModel vm)
        {
            base.PopulateTopAppbar(vm);
            this.HomeButton.Command = this.HomeButtonClickCommand;
            this.AboutButton.Command = this.AboutButtonClickCommand;
        }

    }
}
