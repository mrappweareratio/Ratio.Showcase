using Microsoft.Practices.Prism.StoreApps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using OneMSQFT.Common.Models;
using Windows.UI.Xaml.Media;
using Windows.UI;
using OneMSQFT.UILogic.Interfaces.ViewModels;
using OneMSQFT.UILogic.ViewModels;
using Windows.UI.Core;
using OneMSQFT.Windows.DesignViewModels;

namespace OneMSQFT.Windows.Views
{
    public class BasePageView : VisualStateAwarePage
    {
        public BasePageView()
        {
        }

        public DelegateCommand<String> TopAppBarEventButtonCommand { get; set; }
        public DelegateCommand HomeButtonClickCommand { get; set; }
        public DelegateCommand AboutButtonClickCommand { get; set; }
        public DelegateCommand FilterButtonClickCommand { get; set; }
        public DelegateCommand<AppBarButton> AdminButtonClickCommand { get; set; }
        public DelegateCommand AdminSubmitButtonClickCommand { get; set; }

        protected StackPanel TopAppBarContentStackPanel;
        protected Boolean HomeButtonAddedToAppBar = false;
        
        public void InitAppBars()
        {
            var childTransitions = new TransitionCollection { new EntranceThemeTransition() };

            TopAppBar = new AppBar()
            {
                Padding = new Thickness(0),
                Height = 200.0,
                MinHeight = 200.0,
                MaxHeight = 200.0
            };
            TopAppBarContentStackPanel = new StackPanel()
            {
                Margin = new Thickness(0, 0, 0, 0),
                Orientation = Orientation.Horizontal,
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Center,
                ChildrenTransitions = childTransitions
            };
            var topScrollViewer = new ScrollViewer
            {
                Margin = new Thickness(0),
                Padding = new Thickness(0), 
                Content = TopAppBarContentStackPanel,
                Style = (Style)App.Current.Resources["HorizontalScrollViewerStyle"]
            };
            TopAppBar.Content = topScrollViewer;

            TopAppBarEventButtonCommand = new DelegateCommand<String>(TopAppBarEventButtonCommandHandler);
            HomeButtonClickCommand = new DelegateCommand(HomeButtonClickCommandHandler);
            AboutButtonClickCommand = new DelegateCommand(AboutButtonClickCommandHandler);
            FilterButtonClickCommand = new DelegateCommand(FilterButtonClickCommandHandler);
            AdminButtonClickCommand = new DelegateCommand<AppBarButton>(AdminButtonClickCommandHandler);
            AdminSubmitButtonClickCommand = new DelegateCommand(AdminSubmitButtonClickCommandHandler);

        }

        public virtual void TopAppBarEventButtonCommandHandler(String eventId)
        {            
        }

        async public void HomeButtonClickCommandHandler()
        {
            this.Frame.Navigate(typeof(TimelinePage));
            TopAppBar.IsOpen = false;
            BottomAppBar.IsOpen = false;
        }

        async public void AboutButtonClickCommandHandler()
        {
            this.Frame.Navigate(typeof(AboutPage));
            TopAppBar.IsOpen = false;
            BottomAppBar.IsOpen = false;
        }
        async public virtual void FilterButtonClickCommandHandler()
        {
            // overridden in local page
        }
        async public virtual void AdminButtonClickCommandHandler(AppBarButton sender)
        {
            this.TopAppBar.IsOpen = false;
        }

        async public virtual void AdminSubmitButtonClickCommandHandler()
        {
            this.BottomAppBar.IsOpen = false;
            this.TopAppBar.IsOpen = false;
        }

        public virtual void PopulateTopAppbar(BasePageViewModel vm)
        {
            var homeButton = new Button();
            homeButton.Style = (Style)App.Current.Resources["OMSQFTAppBarHomeButtonStyle"];
            homeButton.Command = TopAppBarEventButtonCommand;
            homeButton.CommandParameter = null;
            TopAppBarContentStackPanel.Children.Add(homeButton);
            foreach (var e in vm.SquareFootEvents)
            {
                var b = new Button();
                b.Command = TopAppBarEventButtonCommand;
                b.CommandParameter = e.Id;
                b.Style = (Style)App.Current.Resources["OMSQFTAppBarButtonStyle"];
                b.DataContext = e;
                TopAppBarContentStackPanel.Children.Add(b);
            }
        }

    }
}
