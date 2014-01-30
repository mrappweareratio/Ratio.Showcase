using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using OneMSQFT.UILogic.Navigation;
using OneMSQFT.UILogic.Tests.Mocks;

namespace OneMSQFT.UILogic.Tests
{
    [TestClass]
    public class OneMsqftApplicationFixture
    {
        [TestMethod]
        public void Application_Launches_To_Timeline_Page()
        {
            string page = null;
            var navigationService = new MockNavigationService()
                {
                    NavigateDelegate = (a, b) =>
                    {
                        page = a;
                        return true;
                    }
                };
            var app = new OneMsqftApplication(navigationService);
            app.OnLaunchApplication(new MockLaunchActivatedEventArgs());
            Assert.AreEqual(page, ViewLocator.Pages.Timeline);
        }
    }
}
