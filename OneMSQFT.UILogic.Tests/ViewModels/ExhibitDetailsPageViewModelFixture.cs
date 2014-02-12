using System.Threading;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Xaml.Navigation;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using OneMSQFT.Common.Models;
using OneMSQFT.UILogic.DataLayer;
using OneMSQFT.UILogic.Interfaces.ViewModels;
using OneMSQFT.UILogic.Tests.Mocks;
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
            var vm = new ExhibitDetailsPageViewModel(new MockDataService(), new MockAlertMessageService(), new MockNavigationService()) as IExhibitDetailsPageViewModel;
            Assert.IsNotNull(vm);
        }

        [TestMethod]
        public void ViewModel_NavigatededTo_Calls_DataService_GetExhibitDetailByExhibitId()
        {
            var autoResetEvent = new AutoResetEvent(false);
            bool called = false;
            var mockDataService = new MockDataService
            {
                GetExhibitDetailByExhibitIdDelegate = (id) =>
                {
                    called = true;
                    return Task.FromResult<ExhibitDetail>(new ExhibitDetail()
                    {
                        Exhibit = MockModelGenerator.NewExhibit(id, "Exhibit")
                    });                    
                }
            };
            var vm = new ExhibitDetailsPageViewModel(mockDataService, new MockAlertMessageService(), new MockNavigationService());
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
                GetExhibitDetailByExhibitIdDelegate = (id) =>
                {
                    called = true;
                    return Task.FromResult<ExhibitDetail>(new ExhibitDetail()
                    {
                        Exhibit = exhibit
                    });
                }
            };
            var vm = new ExhibitDetailsPageViewModel(mockDataService, new MockAlertMessageService(), new MockNavigationService());
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
                GetExhibitDetailByExhibitIdDelegate = (id) =>
                {
                    return Task.FromResult<ExhibitDetail>(new ExhibitDetail()
                    {
                        Exhibit = new Exhibit()
                        {
                            Id = id,
                            Description = "Description Lorem ipsum dolor sit amet, consectetur adipiscing elit. Phasellus in viverra neque. Ut dictum, massa ut sodales consectetur, mi eros consequat enim, quis pretium mauris justo non est. Duis sit amet est nulla. Mauris vehicula. ",
                            Exhibitor = "Artist Name",
                        }
                    });
                }
            };
            var vm = new ExhibitDetailsPageViewModel(mockDataService, new MockAlertMessageService(), new MockNavigationService());
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
            var vm = new ExhibitDetailsPageViewModel(mockDataService, new MockAlertMessageService(), new MockNavigationService());
            ExecuteOnUIThread(() => vm.OnNavigatedTo("0", NavigationMode.New, null));
            autoResetEvent.WaitOne(200);
            Assert.IsTrue(called);
            Assert.IsNotNull(vm.Exhibit);
            Assert.IsTrue(vm.NextExhibitCommand.CanExecute("1"));
        }
    }
}
