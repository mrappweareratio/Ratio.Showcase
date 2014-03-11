using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;
using Microsoft.Practices.Prism.StoreApps.Interfaces;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using OneMSQFT.Common.Models;
using OneMSQFT.Common.Services;
using OneMSQFT.UILogic.Analytics;
using OneMSQFT.UILogic.DataLayer;
using OneMSQFT.UILogic.Interfaces.ViewModels;
using OneMSQFT.UILogic.Services;
using OneMSQFT.UILogic.Tests.Mocks;
using OneMSQFT.UILogic.Utils;
using OneMSQFT.UILogic.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneMSQFT.UILogic.Tests.ViewModels
{
    [TestClass]
    public class ExhibitDetailsPageViewModelFixture
    {
        public IAsyncAction ExecuteOnUIThread(DispatchedHandler action)
        {
            return Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, action);
        }

        [TestMethod]
        public void ViewModel_Implements_Interface()
        {
            var vm = new ExhibitDetailsPageViewModel(new MockDataService(), new MockAlertMessageService(), new MockNavigationService(), new MockConfigurationService(), new AnalyticsService()) as IExhibitDetailsPageViewModel;
            Assert.IsNotNull(vm);
        }

        [TestMethod]
        public void ViewModel_NavigatededTo_Calls_DataService_GetExhibitDetailByExhibitId()
        {
            var autoResetEvent = new AutoResetEvent(false);
            bool called = false;
            var mockDataService = new MockDataService
            {
                GetEventsDelegate = () => Task.FromResult<IEnumerable<Event>>(new List<Event>()),
                GetExhibitDetailByExhibitIdDelegate = (id) =>
                {
                    called = true;
                    return Task.FromResult<ExhibitDetail>(new ExhibitDetail()
                    {
                        Exhibit = MockModelGenerator.NewExhibit(id, "Exhibit")
                    });
                }
            };
            var vm = new ExhibitDetailsPageViewModel(mockDataService, new MockAlertMessageService(), new MockNavigationService(), new MockConfigurationService(), new AnalyticsService());
            ExecuteOnUIThread(() => vm.OnNavigatedTo("0", NavigationMode.New, null));
            autoResetEvent.WaitOne(200);
            Assert.IsTrue(called);
            Assert.IsNotNull(vm.Exhibit);
        }

        [TestMethod]
        public async Task ViewModel_NavigatededTo_Has_MediaContentCollection()
        {
            var autoResetEvent = new AutoResetEvent(false);
            bool called = false;
            var exhibit = MockModelGenerator.NewExhibit("0", "Exhibit");
            exhibit.MediaContent = DemoDataRepository.GetMediaCollection(1);
            var mockDataService = new MockDataService
            {
                GetEventsDelegate = () => Task.FromResult<IEnumerable<Event>>(new List<Event>()),
                GetExhibitDetailByExhibitIdDelegate = (id) =>
                {
                    called = true;
                    return Task.FromResult<ExhibitDetail>(new ExhibitDetail()
                    {
                        Exhibit = exhibit
                    });
                }
            };
            var vm = new ExhibitDetailsPageViewModel(mockDataService, new MockAlertMessageService(), new MockNavigationService(), new MockConfigurationService(), new AnalyticsService());
            ExecuteOnUIThread(() => vm.OnNavigatedTo("0", NavigationMode.New, null));
            autoResetEvent.WaitOne(200);
            Assert.IsTrue(called);
            Assert.IsNotNull(vm.Exhibit);
            Assert.IsNotNull(vm.Exhibit.MediaContent);
        }

        [TestMethod]
        public async Task ViewModel_NavigatededTo_Has_HeroPhotoPath()
        {
            var autoResetEvent = new AutoResetEvent(false);
            var mockDataService = new MockDataService
            {
                GetEventsDelegate = () => Task.FromResult<IEnumerable<Event>>(new List<Event>()),
                GetExhibitDetailByExhibitIdDelegate = (id) =>
                {
                    return Task.FromResult<ExhibitDetail>(new ExhibitDetail()
                    {
                        Exhibit = MockModelGenerator.NewExhibit(id, "Exhibit")
                    });
                }
            };
            var vm = new ExhibitDetailsPageViewModel(mockDataService, new MockAlertMessageService(), new MockNavigationService(), new MockConfigurationService(), new AnalyticsService());
            ExecuteOnUIThread(() => vm.OnNavigatedTo("0", NavigationMode.New, null));
            autoResetEvent.WaitOne(200);
            Assert.IsNotNull(vm.Exhibit);

        }

        [TestMethod]
        public void GetExhibitDetailByExhibitId_NextExhibitCommand_Enabled()
        {
            var autoResetEvent = new AutoResetEvent(false);
            bool called = false;
            var mockDataService = new MockDataService
            {
                GetEventsDelegate = () => Task.FromResult<IEnumerable<Event>>(new List<Event>()),
                GetExhibitDetailByExhibitIdDelegate = (id) =>
                {
                    called = true;
                    return Task.FromResult<ExhibitDetail>(new ExhibitDetail()
                    {
                        Exhibit = MockModelGenerator.NewExhibit(id, "Exhibit"),
                        NextExhibit = MockModelGenerator.NewExhibit("1", "Next Exhibit")

                    });
                }
            };
            var vm = new ExhibitDetailsPageViewModel(mockDataService, new MockAlertMessageService(), new MockNavigationService(), new MockConfigurationService(), new AnalyticsService());
            ExecuteOnUIThread(() => vm.OnNavigatedTo("0", NavigationMode.New, null));
            autoResetEvent.WaitOne(200);
            Assert.IsTrue(called);
            Assert.IsNotNull(vm.Exhibit);
            Assert.IsTrue(vm.NextExhibitCommand.CanExecute("1"));
        }


        [TestMethod]
        public async Task Set_Startup_Exhibit_Flow()
        {
            var autoReset = new AutoResetEvent(false);
            var data = new MockDataService()
            {
                GetEventsDelegate = () => Task.FromResult<IEnumerable<Event>>(new List<Event>()),                
                GetExhibitDetailByExhibitIdDelegate = s =>
                {
                    return Task<ExhibitDetail>.FromResult(new ExhibitDetail()
                    {
                        Exhibit = MockModelGenerator.NewExhibit("0", "Exhibit"),
                        NextExhibit = MockModelGenerator.NewExhibit("1", "Exhibit")
                    });
                }
            };
            var configuration = new ConfigurationService();
            configuration.ClearStartupItem();
            var vm = new ExhibitDetailsPageViewModel(data, new MockAlertMessageService(),
                new MockNavigationService(), configuration, new AnalyticsService());
            ExecuteOnUIThread(() => vm.OnNavigatedTo("0", NavigationMode.New, null));
            autoReset.WaitOne(500);
            autoReset.Reset();
            Assert.IsTrue(vm.SetStartupCommand.CanExecute(), "SetStartupCommand CanExecute");
            await vm.SetStartupCommand.Execute();            
            Assert.IsTrue(vm.ClearStartupCommand.CanExecute(), "ClearStartupCommand CanExecute IsTrue");
            Assert.IsTrue(vm.ClearStartupVisibility == Visibility.Visible, "ClearStartupVisibility Visible");
            Assert.IsTrue(vm.SetStartupVisibility == Visibility.Collapsed, "SetStartupVisibility Collapsed");
            Assert.IsTrue(configuration.StartupItemType == StartupItemType.Exhibit, "StartupItemType Exhibit");
            Assert.AreEqual(configuration.StartupItemId, "0", "StartupItemType Id");
            data.GetExhibitDetailByExhibitIdDelegate = s =>
            {
                return Task<ExhibitDetail>.FromResult(new ExhibitDetail()
                {
                    Exhibit = MockModelGenerator.NewExhibit("1", "Exhibit"),
                    NextExhibit = MockModelGenerator.NewExhibit("2", "Exhibit")
                });
            };
            ExecuteOnUIThread(() => vm.OnNavigatedTo("0", NavigationMode.New, null));
            autoReset.WaitOne(500);
            autoReset.Reset();
            Assert.IsFalse(vm.ClearStartupCommand.CanExecute(), "ClearStartupCommand CanExecute IsFalse");
            Assert.IsTrue(vm.SetStartupCommand.CanExecute(), "SetStartupCommand CanExecute IsTrue");                        
            await vm.SetStartupCommand.Execute();
            Assert.IsTrue(vm.ClearStartupCommand.CanExecute(), "ClearStartupCommand CanExecute IsTrue");
            Assert.IsTrue(configuration.StartupItemType == StartupItemType.Exhibit, "StartupItemType Exhibit");
            Assert.AreEqual(configuration.StartupItemId, "1", "StartupItemType Id");

        }

        #region Pinning       

        [TestMethod]
        public void Pinning_PinContextChanged_TileId()
        {
            var autoResetEvent = new AutoResetEvent(false);
            bool changed = false;
            var data = new MockDataService()
            {
                GetEventsDelegate = () => Task.FromResult<IEnumerable<Event>>(new List<Event>()),
                GetExhibitDetailByExhibitIdDelegate = s =>
                {
                    return Task<ExhibitDetail>.FromResult(new ExhibitDetail()
                    {
                        Exhibit = MockModelGenerator.NewExhibit("0", "Exhibit"),
                        NextExhibit = MockModelGenerator.NewExhibit("1", "Exhibit")
                    });
                }
            };
            INavigationService navigation = new MockNavigationService();
            IConfigurationService configuration = new MockConfigurationService();
            var vm = new ExhibitDetailsPageViewModel(data, new MockAlertMessageService(), navigation, configuration, new AnalyticsService());
            vm.PinContextChanged += (sender, args) =>
            {
                changed = true;
            };
            ExecuteOnUIThread(() => vm.OnNavigatedTo("0", NavigationMode.New, null));
            autoResetEvent.WaitOne(500);
            Assert.IsTrue(changed, "PinContextChanged");
            var secondaryTileArgs = vm.GetSecondaryTileArguments();
            Assert.AreEqual(secondaryTileArgs.Id, PinningUtils.GetSecondaryTileIdByExhibitId("0"));
            Assert.AreEqual(secondaryTileArgs.ArgumentsName, PinningUtils.GetSecondaryTileIdByExhibitId("0"));
            //todo insert tests against color
        }

        [TestMethod]
        public void Pinning_PinContextChanged_TileId_Names()
        {
            var autoResetEvent = new AutoResetEvent(false);
            bool changed = false;
            var data = new MockDataService()
            {
                GetEventsDelegate = () => Task.FromResult<IEnumerable<Event>>(new List<Event>()),
                GetExhibitDetailByExhibitIdDelegate = s =>
                {
                    return Task<ExhibitDetail>.FromResult(new ExhibitDetail()
                    {
                        Exhibit = MockModelGenerator.NewExhibit("0", "Exhibit"),
                        NextExhibit = MockModelGenerator.NewExhibit("1", "Exhibit")
                    });
                }
            };
            INavigationService navigation = new MockNavigationService();
            IConfigurationService configuration = new MockConfigurationService();
            var vm = new ExhibitDetailsPageViewModel(data, new MockAlertMessageService(), navigation, configuration, new AnalyticsService());
            vm.PinContextChanged += (sender, args) =>
            {
                changed = true;
            };
            ExecuteOnUIThread(() => vm.OnNavigatedTo("0", NavigationMode.New, null));
            autoResetEvent.WaitOne(500);
            Assert.IsTrue(changed, "PinContextChanged");
            var secondaryTileArgs = vm.GetSecondaryTileArguments();
            Assert.AreEqual(secondaryTileArgs.Id, PinningUtils.GetSecondaryTileIdByExhibitId("0"));
            Assert.AreEqual(secondaryTileArgs.ArgumentsName, PinningUtils.GetSecondaryTileIdByExhibitId("0"));
        }
        #endregion

        #region Resizing

        [TestMethod]
        public void IsHorizontal_Updates_On_Window_Size_Changed()
        {
            var autoResetEvent = new AutoResetEvent(false);
            bool changed = false;

            var vm = new ExhibitDetailsPageViewModel(new MockDataService(), new MockAlertMessageService(),
                new MockNavigationService(), new MockConfigurationService(), new MockAnalyticsService());
            vm.PropertyChanged += (sender, args) =>
            {
                changed = true;
            };
            ExecuteOnUIThread(() => vm.WindowSizeChanged(1000, 500));
            autoResetEvent.WaitOne(500);
            Assert.IsTrue(changed, "PropertyChanged");
            Assert.IsTrue(vm.IsHorizontal);
            ExecuteOnUIThread(() => vm.WindowSizeChanged(500, 1000));
            autoResetEvent.WaitOne(500);
            Assert.IsTrue(changed, "PropertyChanged");
            Assert.IsFalse(vm.IsHorizontal);
            ExecuteOnUIThread(() => vm.WindowSizeChanged(1000, 500));
            autoResetEvent.WaitOne(500);
            Assert.IsTrue(changed, "PropertyChanged");
            Assert.IsTrue(vm.IsHorizontal);
        }

        [TestMethod]
        public void Base_Properties_Update_On_Window_Size_Change()
        {
            var autoResetEvent = new AutoResetEvent(false);
            bool changed = false;
            var changedProperties = new List<string>();

            var vm = new ExhibitDetailsPageViewModel(new MockDataService(), new MockAlertMessageService(),
                new MockNavigationService(), new MockConfigurationService(), new MockAnalyticsService());
            vm.PropertyChanged += (sender, args) =>
            {
                changed = true;
                changedProperties.Add(args.PropertyName);
            };
            ExecuteOnUIThread(() => vm.WindowSizeChanged(1000, 500));
            autoResetEvent.WaitOne(500);
            Assert.IsTrue(changed, "PropertyChanged");
            Assert.IsTrue(changedProperties.Contains("FullScreenWidth"));
            Assert.IsTrue(changedProperties.Contains("FullScreenHeight"));
            Assert.IsTrue(changedProperties.Contains("IsHorizontal"));
            Assert.IsTrue(changedProperties.Contains("LargeFlexyFontSize"));
            Assert.IsTrue(changedProperties.Contains("MediumLargeFlexyFontSize"));
            Assert.IsTrue(changedProperties.Contains("MediumLargeFlexyTightLeading"));
            Assert.IsTrue(changedProperties.Contains("MediumFlexyFontSize"));
            Assert.IsTrue(changedProperties.Contains("MediumSmallFlexyFontSize"));
            Assert.IsTrue(changedProperties.Contains("SmallFlexyFontSize"));
        }

        [TestMethod]
        public void Local_Properties_Update_On_Window_Size_Changed()
        {
            var autoResetEvent = new AutoResetEvent(false);
            bool changed = false;
            var changedProperties = new List<string>();
            var vm = new ExhibitDetailsPageViewModel(new MockDataService(), new MockAlertMessageService(),
                new MockNavigationService(), new MockConfigurationService(), new MockAnalyticsService());
            vm.PropertyChanged += (sender, args) =>
            {
                changed = true;
                changedProperties.Add(args.PropertyName);
            };
            ExecuteOnUIThread(() => vm.WindowSizeChanged(1000, 500));
            autoResetEvent.WaitOne(500);
            Assert.IsTrue(changed, "PropertyChanged");
            Assert.IsTrue(changedProperties.Contains("NineSixteenthsOfWidth"));
            Assert.IsTrue(changedProperties.Contains("OneThirdPanelWidth"));
            Assert.IsTrue(changedProperties.Contains("PortraitHeaderFooterHeight"));
        }


        #endregion
    }
}
