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


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter != null && e.Parameter is ExhibitItemViewModel)
            {
                IExhibitDetailsPageViewModel vm = this.DataContext as IExhibitDetailsPageViewModel;
                if (vm != null)
                {
                    vm.Exhibit = e.Parameter as ExhibitItemViewModel;
                }
            }
        }

        void ExhibitDetailsPage_Loaded(object sender, RoutedEventArgs e)
        {
            PopulateTopAppbar(((BasePageViewModel)this.DataContext));
            IExhibitDetailsPageViewModel vm = this.DataContext as IExhibitDetailsPageViewModel;
            if (vm != null)
            {
                vm.WindowSizeChanged(Window.Current.Bounds.Width, Window.Current.Bounds.Height);
            }
        }

        public override async void TopAppBarEventButtonCommandHandler(EventItemViewModel item)
        {
            if (item == null) return;
            this.Frame.Navigate(typeof(TimelinePage), item);
            TopAppBar.IsOpen = false;
            BottomAppBar.IsOpen = false;
        }

        public override void PopulateTopAppbar(BasePageViewModel vm)
        {
            base.PopulateTopAppbar(vm);
            this.AboutButton.Command = this.AboutButtonClickCommand;
            this.HomeButton.Command = this.HomeButtonClickCommand;
            this.AdminButton.Command = this.AdminButtonClickCommand;
        }

    }
}
