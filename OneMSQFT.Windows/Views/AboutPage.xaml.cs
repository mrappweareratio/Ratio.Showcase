using OneMSQFT.UILogic.Interfaces.ViewModels;
using Windows.UI.Xaml;

namespace OneMSQFT.WindowsStore.Views
{
    public partial class AboutPage : BasePageView
    {
        public AboutPage()
        {
            this.InitializeComponent();
            InitAppBars();
            var vm = GetDataContextAsViewModel<IBasePageViewModel>();            
            vm.PropertyChanged += AboutPage_PropertyChanged;
            ProcessWindowSizeChangedEvent();
            var app = AppLocator.Current;
            if (app != null)
            {
                HomeButton.Visibility = app.KioskModeEnabled ? Visibility.Visible : Visibility.Collapsed;
            }
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

            if (e.PropertyName == "IsHorizontal")
            {
                if (GetDataContextAsViewModel<IBasePageViewModel>().IsHorizontal)
                {
                    VisualStateManager.GoToState(this, "FullScreenLandscape", true);
                }
                else
                {
                    VisualStateManager.GoToState(this, "FullScreenPortrait", true);
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
            HomeButton.Command = HomeButtonClickCommand;
            TwitterButton.Command = TwitterButtonClickCommand;
            InstagramButton.Command = InstagramButtonClickCommand;
        }        
    }
}
