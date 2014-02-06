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
            var photo = "http://placekitten.com/100/100";
            var mockDataService = new MockDataService
            {
                GetExhibitDetailByExhibitIdDelegate = (id) =>
                {
                    called = true;
                    return Task.FromResult<ExhibitDetail>(new ExhibitDetail()
                    {
                        Exhibit = new Exhibit() { Id = id, PhotoFilePath = photo}
                    });
                }
            };
            var vm = new ExhibitDetailsPageViewModel(mockDataService, new MockAlertMessageService());
            vm.OnNavigatedTo("0", NavigationMode.New, null);
            await vm.LoadingTaskCompletionSource.Task;
            Assert.IsTrue(called);
            Assert.IsNotNull(vm.Exhibit);
            Assert.IsTrue(vm.PhotoCollection.Any(x => x.AbsoluteUri.Contains(photo)));
        }

        [TestMethod]
        public async Task ViewModel_NavigatededTo_Has_HeroPhotoPath()
        {
            bool called = false;
            var photo = "http://placekitten.com/100/100";
            var mockDataService = new MockDataService
            {
                GetExhibitDetailByExhibitIdDelegate = (id) =>
                {
                    called = true;
                    return Task.FromResult<ExhibitDetail>(new ExhibitDetail()
                    {
                        Exhibit = new Exhibit() { Id = id, PhotoFilePath = photo }
                    });
                }
            };
            var vm = new ExhibitDetailsPageViewModel(mockDataService, new MockAlertMessageService());
            vm.OnNavigatedTo("1", NavigationMode.New, null);
            await vm.LoadingTaskCompletionSource.Task;
            Assert.IsTrue(called);
            Assert.IsNotNull(vm.Exhibit);
            Assert.IsNotNull(vm.HeroPhotoPath);
            Assert.IsTrue(vm.HeroPhotoPath.AbsoluteUri == photo);
            
        }
    }
}
