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
            var sortedContent = contentSources.OrderBy(x => x.SortOrder);
            return sortedContent.Select(mediaContent => new MediaContentSourceItemViewModel(mediaContent)).ToList();
        }

        public static string SelectVideoUrl(MediaContentSource media)
        {
            return media.VideoUrlHd;
        }
    }
}
