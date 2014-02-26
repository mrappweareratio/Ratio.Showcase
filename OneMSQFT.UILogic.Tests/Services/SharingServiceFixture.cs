using System;
using Windows.Devices.HumanInterfaceDevice;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using OneMSQFT.Common.Models;
using OneMSQFT.UILogic.Services;
using OneMSQFT.UILogic.Tests.Mocks;

namespace OneMSQFT.UILogic.Tests.Services
{
    [TestClass]
    public class SharingServiceFixture
    {
        [TestMethod]
        public void Event_Share_Url()
        {
            var sharingService = new SharingService(new MockApiConfiguration(null, "http://1msqft.com", "http://vimeo.com"));
            Uri uri = null;
            Assert.IsTrue(sharingService.TryGetEventShareUri(MockModelGenerator.NewEvent("1", "event with a #@$%^ &*() name and  $#%^()"), out uri));
            Assert.AreEqual(String.Format("http://1msqft.com/{0}/{1}", "event-with-a-name-and", "1"), uri.AbsoluteUri);
        }

        [TestMethod]
        public void Event_Share_Url_Null()
        {
            var sharingService = new SharingService(new MockApiConfiguration(null, "http://1msqft.com", "http://vimeo.com"));
            Uri uri = null;
            Assert.IsFalse(sharingService.TryGetEventShareUri(MockModelGenerator.NewEvent("1", null), out uri));
        }

        [TestMethod]
        public void Video_Share_Url()
        {
            var sharingService = new SharingService(new MockApiConfiguration(null, "http://1msqft.com", "http://vimeo.com"));
            Uri uri = null;
            Assert.IsTrue(sharingService.TryGetVideoShareUri(new MediaContentSource(){VideoId = "12345678"}, out uri ));
            Assert.AreEqual(String.Format("http://vimeo.com/12345678"), uri.AbsoluteUri);
        }

        [TestMethod]
        public void Video_Share_Url_Null()
        {
            var sharingService = new SharingService(new MockApiConfiguration(null, "http://1msqft.com", "http://vimeo.com"));
            Uri uri = null;
            Assert.IsFalse(sharingService.TryGetVideoShareUri(null, out uri));            
        }
    }
}