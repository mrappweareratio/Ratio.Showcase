using Windows.UI.Xaml.Navigation;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using OneMSQFT.Common.Models;
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
        [TestMethod]
        public void ViewModel_Implements_Interface()
        {
            var vm = new ExhibitDetailsPageViewModel(new MockDataService(), new MockAlertMessageService()) as IExhibitDetailsPageViewModel;
            Assert.IsNotNull(vm);
        }

        [TestMethod]
        public async Task ViewModel_NavigatededTo_Calls_DataService_GetExhibitDetailByExhibitId()
        {
            bool called = false;
            var mockDataService = new MockDataService
            {
                GetExhibitDetailByExhibitIdDelegate = (id) =>
                {
                    called = true;
                    return Task.FromResult<ExhibitDetail>(new ExhibitDetail()
                    {
                        Exhibit = new Exhibit() { Id = id}
                    });                    
                }
            };
            var vm = new ExhibitDetailsPageViewModel(mockDataService, new MockAlertMessageService());
            vm.OnNavigatedTo("0", NavigationMode.New, null);
            await vm.LoadingTaskCompletionSource.Task;
            Assert.IsTrue(called);
            Assert.IsNotNull(vm.Exhibit);
        }

        [TestMethod]
        public async Task ViewModel_NavigatededTo_Has_PhotoCollection_With_PhotoFilePath()
        {
            bool called = false;
            var mockDataService = new MockDataService
            {
                GetExhibitDetailByExhibitIdDelegate = (id) =>
                {
                    called = true;
                    return Task.FromResult<ExhibitDetail>(new ExhibitDetail()
                    {
                        Exhibit = new Exhibit() { Id = id}
                    });
                }
            };
            var vm = new ExhibitDetailsPageViewModel(mockDataService, new MockAlertMessageService());
            vm.OnNavigatedTo("0", NavigationMode.New, null);
            await vm.LoadingTaskCompletionSource.Task;
            Assert.IsTrue(called);
            Assert.IsNotNull(vm.Exhibit);
            Assert.IsTrue(vm.MediaContentCollection.Any(x => x.ImageSource.AbsoluteUri.Contains("http://")));
        }

        [TestMethod]
        public async Task ViewModel_NavigatededTo_Has_HeroPhotoPath()
        {
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
            var vm = new ExhibitDetailsPageViewModel(mockDataService, new MockAlertMessageService());
            vm.OnNavigatedTo("2", NavigationMode.New, null);
            await vm.LoadingTaskCompletionSource.Task;
            Assert.IsNotNull(vm.Exhibit);
            
        }

        [TestMethod]
        public async Task ViewModel_NavigatedTo_Has_TextProperties()
        {
            // Actual text properties are still unknown at this point 
        }
    }
}
