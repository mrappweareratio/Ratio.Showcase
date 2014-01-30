using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Microsoft.Practices.Prism.StoreApps.Interfaces;
using OneMSQFT.UILogic.Interfaces;
using OneMSQFT.UILogic.Navigation;

namespace OneMSQFT.UILogic
{
    public class OneMsqftApplication : IOneMsqftApplication
    {
        public INavigationService NavigationService { get; set; }

        public OneMsqftApplication(INavigationService navigationService)
        {
            NavigationService = navigationService;
        }

        public Task OnLaunchApplication(ILaunchActivatedEventArgs args)
        {
            NavigationService.Navigate(ViewLocator.Pages.Timeline, null);
            return Task.FromResult<object>(null);
        }
    }
}