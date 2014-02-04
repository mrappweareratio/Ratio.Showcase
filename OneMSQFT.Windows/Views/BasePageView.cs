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

        protected StackPanel TopAppBarContentStackPanel;
        protected Boolean HomeButtonAddedToAppBar = false;
        
        public void InitAppBar()
        {
            var childTransitions = new TransitionCollection { new EntranceThemeTransition() };

            TopAppBar = new AppBar()
            {
                Padding = new Thickness(0),
                Height = 200.0,
                MinHeight = 200.0,
                MaxHeight = 200.0
            };
            TopAppBar.Opened += TopAppBarOpened;

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
                Margin = new Thickness(0,0,0,-10),
                Padding = new Thickness(10, 0, 10, 10), // using 10 so scroll bars don't obscure content
                Content = TopAppBarContentStackPanel,
                Style = (Style)App.Current.Resources["HorizontalScrollViewerStyle"]
            };
            TopAppBar.Content = topScrollViewer;
        }

        void TopAppBarOpened(object sender, object e)
        {
        }

        public void PopulateTopAppbar(ITimelinePageViewModel vm)
        {
            foreach (var e in vm.SquareFootPastEvents)
            {
                var b = new Button();
                b.Style = (Style)App.Current.Resources["OMSQFTAppBarButtonStyle"];
                b.DataContext = e;
                TopAppBarContentStackPanel.Children.Add(b);
            }
            var homeButton = new Button();
            homeButton.Style = (Style)App.Current.Resources["OMSQFTAppBarHomeButtonStyle"];
            TopAppBarContentStackPanel.Children.Add(homeButton);
            foreach (var e in vm.SquareFootFutureEvents)
            {
                var b = new Button();
                b.Style = (Style)App.Current.Resources["OMSQFTAppBarButtonStyle"];
                b.DataContext = e;
                TopAppBarContentStackPanel.Children.Add(b);
            }
        }
    }
}
