﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using OneMSQFT.Common.Models;
using OneMSQFT.UILogic.DataLayer;
using OneMSQFT.UILogic.Utils;

namespace OneMSQFT.UILogic.Tests.Utils
{
    [TestClass]
    public class MediaContentSourceUtilsFixture
    {    
        [TestMethod]
        public void Parses_SortedImageAndVideoMediaList()
        {
            var mediaContentSources = new List<MediaContentSource>
            {
                new MediaContentSource(){SortOrder = 1, ContentSourceType = ContentSourceType.Image, Img = "http://url.com/imagesource"},
                new MediaContentSource(){SortOrder = 2, ContentSourceType = ContentSourceType.Video, Id = "5",Img = "http://url.com/imagesource", VideoUrlHd = "http://smf.blob.core.windows.net/samples/videos/bigbuck.mp4"}
            };
            var mediaContent = MediaContentSourceUtils.GetMediaContentSourceItemViewModels(mediaContentSources);
            Assert.IsNotNull(mediaContent);            
            Assert.IsTrue(mediaContent.Count() == 2, "Two Media Items");
            Assert.IsTrue(mediaContent.First().IsVideoButtonVisible == Visibility.Collapsed, "Video Hidden");
            Assert.IsTrue(mediaContent.Last().IsVideoButtonVisible == Visibility.Visible, "Video Visible");
            Assert.IsTrue(mediaContent.First().ImageSource.AbsoluteUri.Equals("http://url.com/imagesource"), "ImageSource Matches");
            Assert.IsTrue(mediaContent.Last().VideoSource.AbsoluteUri.Equals("http://smf.blob.core.windows.net/samples/videos/bigbuck.mp4"), "MediaSource Matches");
        }

        [TestMethod]
        public void Parses_MultipleImageAndVideoMediaContentList()
        {
            var mediaContentSources = new List<MediaContentSource>
            {
                new MediaContentSource(){ContentSourceType = ContentSourceType.Image, Img = "http://url.com/imagesource"},
                new MediaContentSource(){ContentSourceType = ContentSourceType.Video, Id = "5", Img = "http://url.com/imagesource", VideoUrlHd = "http://smf.blob.core.windows.net/samples/videos/bigbuck.mp4"},
                new MediaContentSource(){ContentSourceType = ContentSourceType.Image, Img = "http://url.com/imagesource"},
                new MediaContentSource(){ContentSourceType = ContentSourceType.Video, Id = "6",Img = "http://url.com/imagesource", VideoUrlHd = "http://smf.blob.core.windows.net/samples/videos/bigbuck.mp4"},
                new MediaContentSource(){ContentSourceType = ContentSourceType.Image, Img = "http://url.com/imagesource"},
                new MediaContentSource(){ContentSourceType = ContentSourceType.Video, Id = "blank",Img = "http://url.com/imagesource", VideoUrlHd = "http://smf.blob.core.windows.net/samples/videos/bigbuck.mp4"}

            };
            var mediaContent = MediaContentSourceUtils.GetMediaContentSourceItemViewModels(mediaContentSources).ToList();
            Assert.IsNotNull(mediaContent);
            Assert.IsTrue(mediaContent.Count == 6, "Media Item Count");
            Assert.IsTrue(mediaContent.Count(x => x.IsVideoButtonVisible == Visibility.Visible) == 3, "3 Visible Videos");
        }

        [TestMethod]
        public void Ignores_Bad_Videos()
        {
            var mediaContentSources = new List<MediaContentSource>
            {
                new MediaContentSource(){ContentSourceType = ContentSourceType.Image, Img = "http://url.com/imagesource"},
                new MediaContentSource(){ContentSourceType = ContentSourceType.Video, Id = "5", Img = "http://url.com/imagesource"},//bad video
                new MediaContentSource(){ContentSourceType = ContentSourceType.Image, Img = "http://url.com/imagesource"},
                new MediaContentSource(){ContentSourceType = ContentSourceType.Video, Id = "6",Img = "http://url.com/imagesource", VideoUrlHd = "http://smf.blob.core.windows.net/samples/videos/bigbuck.mp4"},                

            };
            var mediaContent = MediaContentSourceUtils.GetMediaContentSourceItemViewModels(mediaContentSources).ToList();
            Assert.IsNotNull(mediaContent);
            Assert.IsTrue(mediaContent.Count == 3, "Media Item Count");
            Assert.IsTrue(mediaContent.Count(x => x.ContentSourceType == ContentSourceType.Video) == 1, "1 Valid Video");
        }

        [TestMethod]
        public void Sorts_Media_Content()
        {
            var mediaContentSources = new List<MediaContentSource>
            {
                new MediaContentSource(){ContentSourceType = ContentSourceType.Image, Id = "8", Img = "http://url.com/imagesource", SortOrder = 20},                
                new MediaContentSource(){ContentSourceType = ContentSourceType.Image, Img = "http://url.com/imagesource", SortOrder = 10},
                new MediaContentSource(){ContentSourceType = ContentSourceType.Video, Id = "6",Img = "http://url.com/imagesource", VideoUrlHd = "http://smf.blob.core.windows.net/samples/videos/bigbuck.mp4", SortOrder = 0},                
            };
            var mediaContent = MediaContentSourceUtils.GetMediaContentSourceItemViewModels(mediaContentSources).ToList();
            Assert.IsNotNull(mediaContent);
            Assert.IsTrue(mediaContent.Count == 3, "Media Item Count");
            Assert.AreEqual(mediaContent.First().Id, "6", "First Sorted");
            Assert.AreEqual(mediaContent.Last().Id, "8", "Last Sorted");
        }

        public static void ValidateMediaContent(List<MediaContentSource> mediaContent)
        {
            foreach(var media in mediaContent)
                ValidateMediaContent(media);
        }

        public static void ValidateMediaContent(MediaContentSource mediaContent)
        {
            Assert.IsNotNull(mediaContent.ContentSourceType, "ContentSourceType IsNotNull");
            if (mediaContent.ContentSourceType == ContentSourceType.Image)
            {
                Assert.IsNotNull(mediaContent.Img, "Img IsNotNull");
            }
            else if (mediaContent.ContentSourceType == ContentSourceType.Video)
            {
                Assert.IsTrue(MediaContentSourceUtils.HasVideoUrl(mediaContent), "HasVideoUrl");
                Assert.IsNotNull(MediaContentSourceUtils.SelectVideoUrl(mediaContent), "SelectVideoUrl");
            }
        }
    }
}
