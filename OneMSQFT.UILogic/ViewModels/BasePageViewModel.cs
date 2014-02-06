using System.Collections.Generic;
using Windows.UI.Xaml.Navigation;
using Microsoft.Practices.Prism.StoreApps;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;

namespace OneMSQFT.UILogic.ViewModels
{
    public class BasePageViewModel : ViewModel
    {
        public ObservableCollection<EventItemViewModel> SquareFootEvents { get; set; }        
    }
}
