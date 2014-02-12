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
        }

        void AboutPage_Loaded(object sender, RoutedEventArgs e)
        {
            ((AboutPageViewModel)this.DataContext).PropertyChanged += AboutPage_PropertyChanged;
            var vm = this.DataContext as IAboutPageViewModel;
            if (vm != null)
            {
                vm.WindowSizeChanged(Window.Current.Bounds.Width, Window.Current.Bounds.Height);
            }
        }

        void AboutPage_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SquareFootEvents")
            {
                if (((AboutPageViewModel)this.DataContext).SquareFootEvents.Count > 0)
                {
                    this.PopulateTopAppbar(((BasePageViewModel)this.DataContext));
                }
            }
        }

        public override async void TopAppBarEventButtonCommandHandler(string eventId)
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

    }
}
