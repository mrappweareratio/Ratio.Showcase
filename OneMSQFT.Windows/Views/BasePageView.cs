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
using OneMSQFT.UILogic.ViewModels;

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

            ScrollViewer topScrollViewer = new ScrollViewer
            {
                Margin = new Thickness(0,0,0,-10),
                Padding = new Thickness(10, 0, 10, 10), // using 10 so scroll bars don't obscure content
                Content = TopAppBarContentStackPanel,
                Style = (Style)App.Current.Resources["HorizontalScrollViewerStyle"]
            };
            // adding top bar by default
            TopAppBar.Content = topScrollViewer;
        }

        void TopAppBarOpened(object sender, object e)
        {
        }

        public void PopulateTopAppbar(int count){
            for(var i =0; i<count; i++){
                var EIVM = new EventItemViewModel();
                EIVM.Event = new Event();
                EIVM.Event.Color = "00ff00";
                EIVM.Event.Description = "Description " + i;
                EIVM.Event.Id = i.ToString();
                EIVM.Event.Name = "Event Name " + i;
                EIVM.Event.IsInTheFuture = i < count / 2;
                var b = new Button();
                b.Style = (Style)App.Current.Resources["OMSQFTAppBarButtonStyle"];
                b.DataContext = EIVM;
                if ((EIVM.Event.IsInTheFuture == false) && (HomeButtonAddedToAppBar == false))
                {
                    var homeButton = new Button();
                    homeButton.Style = (Style)App.Current.Resources["OMSQFTAppBarHomeButtonStyle"];
                    TopAppBarContentStackPanel.Children.Add(homeButton);
                    HomeButtonAddedToAppBar = true;
                }
                TopAppBarContentStackPanel.Children.Add(b);
            }

        }
    }
}
