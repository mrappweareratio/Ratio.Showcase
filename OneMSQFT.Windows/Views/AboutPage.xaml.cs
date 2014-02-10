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
            PopulateTopAppbar(((BasePageViewModel)this.DataContext));
            var vm = this.DataContext as IAboutPageViewModel;
            if (vm != null)
            {
                vm.WindowSizeChanged(Window.Current.Bounds.Width, Window.Current.Bounds.Height);
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
            this.AdminButton.Command = this.AdminButtonClickCommand;
        }

    }
}
