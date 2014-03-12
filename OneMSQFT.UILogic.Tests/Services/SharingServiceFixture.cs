using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Devices.HumanInterfaceDevice;
using Windows.Storage.Streams;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using OneMSQFT.Common.Models;
using OneMSQFT.UILogic.Services;
using OneMSQFT.UILogic.Tests.Mocks;

namespace OneMSQFT.UILogic.Tests.Services
{
    [TestClass]
    public class SharingServiceFixture
    {
        private SharingService sharingService;

        [TestInitialize]
        public void Init()
        {
            sharingService = new SharingService(new MockApiConfiguration(null, "http://1msqft.com", "http://vimeo.com"));   
        }

        [TestMethod]
        public void Event_Share_Url()
        {            
            Uri uri = null;
            Assert.IsTrue(sharingService.TryGetEventShareUri(MockModelGenerator.NewEvent("1", "event with a #@$%^ &*() name and  $#%^()"), out uri));
            Assert.AreEqual(String.Format("http://1msqft.com/event/{0}/{1}", "event-with-a-name-and", "1"), uri.AbsoluteUri);
        }

        [TestMethod]
        public void Event_Share_Url_Null()
        {
            Uri uri = null;
            Assert.IsFalse(sharingService.TryGetEventShareUri(MockModelGenerator.NewEvent("1", null), out uri));
        }

        [TestMethod]
        public void Video_Share_Url()
        {
            Uri uri = null;
            Assert.IsTrue(sharingService.TryGetVideoShareUri(new MediaContentSource(){VideoId = "12345678"}, out uri ));
            Assert.AreEqual(String.Format("http://vimeo.com/12345678"), uri.AbsoluteUri);
        }

        [TestMethod]
        public void Video_Share_Url_Null()
        {
            Uri uri = null;
            Assert.IsFalse(sharingService.TryGetVideoShareUri(null, out uri));
            Assert.IsFalse(sharingService.TryGetVideoShareUri(new MediaContentSource(), out uri));     
        }

        [TestMethod]
        public async Task Event_Share_Urls()
        {
            var ev = MockModelGenerator.NewEvent("0", "name");
            Uri uri;
            Assert.IsFalse(sharingService.TryGetSharingThumbnailUri(ev, out uri), "No Uri without MediaContent");
            ev.MediaContent = new List<MediaContentSource>
            {
                new MediaContentSource() {Img = "http://placehold.it/350x150"}
            };
            Assert.IsTrue(sharingService.TryGetSharingThumbnailUri(ev, out uri), "Good Uri with MediaContent");
            var reference = RandomAccessStreamReference.CreateFromUri(uri);
            Assert.IsNotNull(reference, "reference");
            using (var stream = await reference.OpenReadAsync())
            {
                Assert.IsTrue(stream.CanRead, "CanRead");
                Assert.IsTrue(stream.Size > 0, "Has Size");
            }

            ev.MediaContent = new List<MediaContentSource>
            {
                new MediaContentSource() {Img = "http://1msqft-stage.azurewebsites.net/images/events/1/content/1.jpg"}
            };

            Assert.IsTrue(sharingService.TryGetSharingThumbnailUri(ev, out uri), "Good Uri from api");
            reference = RandomAccessStreamReference.CreateFromUri(uri);
            Assert.IsNotNull(reference, "reference");
            using (var stream = await reference.OpenReadAsync())
            {
                Assert.IsTrue(stream.CanRead, "CanRead");
                Assert.IsTrue(stream.Size > 0, "Has Size");
            }
        }
    }
}