using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Prism.StoreApps.Interfaces;
using OneMSQFT.UILogic;
using OneMSQFT.UILogic.Interfaces;
// The Blank Application template is documented at http://go.microsoft.com/fwlink/?LinkId=234227
using Microsoft.Practices.Prism.StoreApps;
using OneMSQFT.UILogic.ViewModels;

namespace OneMSQFT.Windows
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : MvvmAppBase
    {
        private IOneMsqftApplication _application;

        public IEventAggregator EventAggregator { get; set; }

        protected override Task OnLaunchApplication(LaunchActivatedEventArgs args)
        {
            return _application.OnLaunchApplication(args);                        
        }

        protected override void OnInitialize(IActivatedEventArgs args)
        {
            _application = new OneMsqftApplication(NavigationService);

            ViewModelLocator.SetDefaultViewTypeToViewModelTypeResolver((viewType) =>
            {                
                var viewModelTypeName = string.Format(CultureInfo.InvariantCulture, "OneMSQFT.UILogic.ViewModels.{0}ViewModel, OneMSQFT.UILogic", viewType.Name);
                var viewModelType = Type.GetType(viewModelTypeName);
                return viewModelType;
            });
        }        
    }
}
