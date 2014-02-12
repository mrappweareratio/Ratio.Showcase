using System.Collections.Generic;
using Windows.UI.Xaml.Navigation;
using Microsoft.Practices.Prism.StoreApps;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using OneMSQFT.UILogic.Interfaces.ViewModels;

namespace OneMSQFT.UILogic.ViewModels
{
    public abstract class BasePageViewModel : ViewModel , IBasePageViewModel
    {
        private ObservableCollection<EventItemViewModel> _squareFootEvents;
        public ObservableCollection<EventItemViewModel> SquareFootEvents
        {
            get { return _squareFootEvents; }
            set { SetProperty(ref _squareFootEvents, value); }
        }

        public abstract void WindowSizeChanged(double width, double height);

    }
}
