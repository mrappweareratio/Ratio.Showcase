using System.Collections.Generic;
using Windows.UI.Xaml.Navigation;
using Microsoft.Practices.Prism.StoreApps;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;

namespace OneMSQFT.UILogic.ViewModels
{
    public class BasePageViewModel : ViewModel
    {
        private ObservableCollection<EventItemViewModel> _squareFootEvents;

        public ObservableCollection<EventItemViewModel> SquareFootEvents
        {
            get { return _squareFootEvents; }
            set { SetProperty(ref _squareFootEvents, value); }
        }
    }
}
