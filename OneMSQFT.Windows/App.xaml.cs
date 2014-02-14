using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ApplicationSettings;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Prism.StoreApps.Interfaces;
using Microsoft.Practices.Unity;
using OneMSQFT.Common.DataLayer;
using OneMSQFT.Common.Services;
using OneMSQFT.UILogic;
using OneMSQFT.UILogic.DataLayer;
using OneMSQFT.UILogic.Interfaces;
using Microsoft.Practices.Prism.StoreApps;
using OneMSQFT.UILogic.Services;
using OneMSQFT.UILogic.ViewModels;
using OneMSQFT.Windows.DataLayer;

namespace OneMSQFT.Windows
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : MvvmAppBase
    {
        public App()
        {
            this.UnhandledException += App_UnhandledException;
            this.ExtendedSplashScreenFactory = (splashscreen) => new ExtendedSplashScreen(splashscreen);
        }

        void App_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (Debugger.IsAttached)
            {
                Debugger.Break();
            }
        }
        // Create the singleton container that will be used for type resolution in the app
        private readonly IUnityContainer _container = new UnityContainer();

        private IOneMsqftApplication _application;

        protected override Task OnLaunchApplication(LaunchActivatedEventArgs args)
        {
            return _application.OnLaunchApplication(args);
        }

        protected override void OnInitialize(IActivatedEventArgs args)
        {            
            //register repositories
            //_container.RegisterType<IDataRepository, SampleDataRepository>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IDataRepository, DemoDataRepository>(new ContainerControlledLifetimeManager());            

            //register services
            _container.RegisterInstance<INavigationService>(NavigationService);
            _container.RegisterType<IInternetConnection, InternetConnectionService>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IDataCacheService, DataCacheService>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IDataService, DataService>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IAlertMessageService, AlertMessageService>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IConfigurationService, ConfigurationService>(new ContainerControlledLifetimeManager());

            //create the application
            _application = new OneMsqftApplication(NavigationService, _container.Resolve<IDataService>());

            //register the application
            _container.RegisterInstance<IOneMsqftApplication>(_application);

            _application.OnInitialize(args);

            ViewModelLocator.SetDefaultViewTypeToViewModelTypeResolver((viewType) =>
            {
#if DESIGN
                var viewModelTypeName = string.Format(CultureInfo.InvariantCulture, "OneMSQFT.Windows.DesignViewModels.Design{0}ViewModel, OneMSQFT.Windows", viewType.Name);
#else
                var viewModelTypeName = string.Format(CultureInfo.InvariantCulture, "OneMSQFT.UILogic.ViewModels.{0}ViewModel, OneMSQFT.UILogic", viewType.Name);
#endif
                var viewModelType = Type.GetType(viewModelTypeName);
                return viewModelType;
            });         
        }

        protected override object Resolve(Type type)
        {
            return _container.Resolve(type);
        }

        protected override IList<SettingsCommand> GetSettingsCommands()
        {
            return _application.GetSettingsCommands();
        }
    }
}
