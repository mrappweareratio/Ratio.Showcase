using System.Collections.Generic;
using Windows.UI.Xaml.Navigation;
using Microsoft.Practices.Prism.StoreApps;
using System.Collections.ObjectModel;

namespace OneMSQFT.UILogic.ViewModels
{
    public class BasePageViewModel : ViewModel
    {
        public ObservableCollection<EventItemViewModel> SquareFootEvents { get; set; }
        public override void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(navigationParameter, navigationMode, viewModelState);
        }
    }
}
