using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using OneMSQFT.Common.Models;
using OneMSQFT.UILogic.Utils;

namespace OneMSQFT.UILogic.Tests.Utils
{
    [TestClass]
    public class MediaContentSourceUtilsFixture
    {
        [TestMethod]
        public void Parses_Video_Thumbnail_Group()
        {
            var mediaContentSources = new List<MediaContentSource>
            {
                new MediaContentSource(){ContentSourceType = ContentSourceType.Image, ParentId = "5", Source = "http://url.com/imagesource"},
                new MediaContentSource(){ContentSourceType = ContentSourceType.Video, Id = "5", Source = "http://smf.blob.core.windows.net/samples/videos/bigbuck.mp4"}
            };
            var mediaContent = MediaContentSourceUtils.GetMediaContentSourceItemViewModels(mediaContentSources);
            Assert.IsNotNull(mediaContent);            
            Assert.IsTrue(mediaContent.Count() == 1, "One Media Item");
            Assert.IsTrue(mediaContent.First().IsVideoButtonVisible == Visibility.Visible, "Video Visible");
            Assert.IsTrue(mediaContent.First().ImageSource.AbsoluteUri.Equals("http://url.com/imagesource"), "ImageSource Matches");
            Assert.IsTrue(mediaContent.First().MediaSource.AbsoluteUri.Equals("http://smf.blob.core.windows.net/samples/videos/bigbuck.mp4"), "MediaSource Matches");
        }

        [TestMethod]
        public void Parses_Video_Thumbnail_Groups()
        {
            var mediaContentSources = new List<MediaContentSource>
            {
                new MediaContentSource(){ContentSourceType = ContentSourceType.Image, ParentId = "5", Source = "http://url.com/imagesource"},
                new MediaContentSource(){ContentSourceType = ContentSourceType.Video, Id = "5", Source = "http://smf.blob.core.windows.net/samples/videos/bigbuck.mp4"},
                new MediaContentSource(){ContentSourceType = ContentSourceType.Image, ParentId = "6", Source = "http://url.com/imagesource"},
                new MediaContentSource(){ContentSourceType = ContentSourceType.Video, Id = "6", Source = "http://smf.blob.core.windows.net/samples/videos/bigbuck.mp4"},
                new MediaContentSource(){ContentSourceType = ContentSourceType.Image, ParentId = "single image", Source = "http://url.com/imagesource"},
                new MediaContentSource(){ContentSourceType = ContentSourceType.Video, Id = "blank", Source = "http://smf.blob.core.windows.net/samples/videos/bigbuck.mp4"}

            };
            var mediaContent = MediaContentSourceUtils.GetMediaContentSourceItemViewModels(mediaContentSources).ToList();
            Assert.IsNotNull(mediaContent);
            Assert.IsTrue(mediaContent.Count == 3, "Media Item Count");
            Assert.IsTrue(mediaContent.Count(x => x.IsVideoButtonVisible == Visibility.Visible) == 2, "2 Visible Videos");
        }
    }
}
