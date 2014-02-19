using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OneMSQFT.Common.Models;
using OneMSQFT.UILogic.ViewModels;

namespace OneMSQFT.UILogic.Utils
{
    public class MediaContentSourceUtils
    {
        public static IEnumerable<MediaContentSourceItemViewModel> GetMediaContentSourceItemViewModels(
            IEnumerable<MediaContentSource> mediaContentSources)
        {
            var contentSources = mediaContentSources as MediaContentSource[] ?? mediaContentSources.ToArray();
            contentSources = contentSources.Where(ValidateMediaContent).OrderBy(x => x.SortOrder).ToArray();
            return contentSources.Select(mediaContent => new MediaContentSourceItemViewModel(mediaContent)).ToList();
        }

        public static string SelectVideoUrl(MediaContentSource media)
        {
            return media.VideoUrlHd;
        }

        public static bool HasVideoUrl(MediaContentSource media)
        {
            return !String.IsNullOrEmpty(media.VideoUrlHd) || !String.IsNullOrEmpty(media.VideoUrlSd) || !String.IsNullOrEmpty(media.VideoUrlMobile);
        }

        public static bool ValidateMediaContent(MediaContentSource mediaContent)
        {
            if (mediaContent.ContentSourceType == ContentSourceType.Image)
            {
                return !String.IsNullOrEmpty(mediaContent.Img);
            }
            return HasVideoUrl(mediaContent);
        }
    }
}
