using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;

namespace OneMSQFT.UILogic.Interfaces
{
    public interface IOneMsqftApplication
    {
        Task OnLaunchApplication(ILaunchActivatedEventArgs args);
        void OnInitialize(IActivatedEventArgs args);
    }
}