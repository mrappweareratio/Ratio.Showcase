using System.ComponentModel;
using Microsoft.Practices.Prism.PubSubEvents;
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
    public abstract class BasePageView : VisualStateAwarePage
    {
        public DelegateCommand<String> TopAppBarEventButtonCommand { get; set; }
        public DelegateCommand HomeButtonClickCommand { get; set; }
        public DelegateCommand AboutButtonClickCommand { get; set; }
        public DelegateCommand FilterButtonClickCommand { get; set; }
        public DelegateCommand<Button> AdminButtonClickCommand { get; set; }

        protected StackPanel TopAppBarContentStackPanel;
        protected Boolean HomeButtonAddedToAppBar = false;

        public void InitAppBars()
        {
            var childTransitions = new TransitionCollection { new EntranceThemeTransition() };

            TopAppBar = new AppBar()
            {
                Padding = new Thickness(0),
                Height = 192.0,
                MinHeight = 192.0,
                MaxHeight = 192.0,
                Margin = new Thickness(0, -2, 0, 0),
                Background = new SolidColorBrush(Colors.White),
                BorderBrush = new SolidColorBrush(Colors.Transparent),
                BorderThickness = new Thickness(0),
            };
            TopAppBarContentStackPanel = new StackPanel()
            {
                Margin = new Thickness(0, 0, 0, 0),
                Orientation = Orientation.Horizontal,
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left,
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
            TopAppBar.Opened += TopAppBar_Opened;
            BottomAppBar.Opened += BottomAppBar_Opened;
            TopAppBar.Closed += TopAppBar_Closed;
            BottomAppBar.Closed += BottomAppBar_Closed;

            TopAppBarEventButtonCommand = new DelegateCommand<String>(TopAppBarEventButtonCommandHandler);
            HomeButtonClickCommand = new DelegateCommand(HomeButtonClickCommandHandler);
            AboutButtonClickCommand = new DelegateCommand(AboutButtonClickCommandHandler);
            FilterButtonClickCommand = new DelegateCommand(FilterButtonClickCommandHandler);
            AdminButtonClickCommand = new DelegateCommand<Button>(AdminButtonClickCommandHandler);
        }

        void BottomAppBar_Closed(object sender, object e)
        {
            BottomAppBar.Visibility = Visibility.Collapsed;
        }

        void TopAppBar_Closed(object sender, object e)
        {
            BottomAppBar.Visibility = Visibility.Collapsed;
        }

        void BottomAppBar_Opened(object sender, object e)
        {
            BottomAppBar.Visibility = Visibility.Visible;
        }

        void TopAppBar_Opened(object sender, object e)
        {
            BottomAppBar.Visibility = Visibility.Visible;
        }

        public abstract void TopAppBarEventButtonCommandHandler(String eventId);      

        public void HomeButtonClickCommandHandler()
        {
            Frame.Navigate(typeof(TimelinePage));
            TopAppBar.IsOpen = false;
            BottomAppBar.IsOpen = false;
        }

        public void AboutButtonClickCommandHandler()
        {
            Frame.Navigate(typeof(AboutPage));
            TopAppBar.IsOpen = false;
            BottomAppBar.IsOpen = false;
        }

        public virtual void FilterButtonClickCommandHandler()
        {
            // overridden in local page
        }

        public virtual void AdminButtonClickCommandHandler(Button sender)
        {
            BottomAppBar.IsOpen = false;
            TopAppBar.IsOpen = false;
        }

        public virtual void PopulateTopAppbar(IBasePageViewModel vm)
        {
            var homeButton = new Button();
            homeButton.Style = (Style)App.Current.Resources["OMSQFTAppBarHomeButtonStyle"];
            homeButton.Command = null;
            homeButton.IsHitTestVisible=false;
            homeButton.CommandParameter = null;
            TopAppBarContentStackPanel.Children.Clear();            
            foreach (var e in vm.SquareFootEvents)
            {
                var b = new Button();
                b.Margin = new Thickness(0);
                b.Padding = new Thickness(0);
                b.Command = TopAppBarEventButtonCommand;
                b.CommandParameter = e.Id;
                b.Style = (Style)App.Current.Resources["OMSQFTAppBarButtonStyle"];
                b.DataContext = e;
                TopAppBarContentStackPanel.Children.Add(b);
            }
            var homeIndex = 0;
            var ev = vm.SquareFootEvents.FirstOrDefault(x => x.IsInThePast.HasValue && x.IsInThePast.Value);
            if (ev != null)
                homeIndex = vm.SquareFootEvents.IndexOf(ev);
            else
            {
                ev = vm.SquareFootEvents.LastOrDefault(x => x.IsInTheFuture.HasValue && x.IsInTheFuture.Value);
                if (ev != null)
                {
                    homeIndex = vm.SquareFootEvents.IndexOf(ev) + 1;
                    if (homeIndex == vm.SquareFootEvents.Count)
                    {
                        homeIndex = 0;
                    }
                }
            }            
            TopAppBarContentStackPanel.Children.Insert(homeIndex, homeButton);
        }

        public T GetDataContextAsViewModel<T>() where T : INotifyPropertyChanged
        {
            return (T)DataContext;
        }
    }
}
