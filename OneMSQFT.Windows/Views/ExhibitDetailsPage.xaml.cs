using Microsoft.Practices.Prism.StoreApps;
using OneMSQFT.UILogic.Interfaces.ViewModels;
using OneMSQFT.UILogic.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace OneMSQFT.Windows.Views
{
    public sealed partial class ExhibitDetailsPage
    {
        public ExhibitDetailsPage()
        {
            this.InitializeComponent();
            InitAppBar();
            Loaded += ExhibitDetailsPage_Loaded;
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

        public override async void OMSQFTAppBarButtonCommandHandler(EventItemViewModel item)
        {
            if (item == null) return;
            this.Frame.Navigate(typeof(TimelinePage), item);
            TopAppBar.IsOpen = false;
        }
    }
}
