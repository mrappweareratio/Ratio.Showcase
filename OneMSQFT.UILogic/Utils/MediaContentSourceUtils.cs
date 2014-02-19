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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mediaContentSources"></param>
        /// <returns></returns>
        public static IEnumerable<MediaContentSourceItemViewModel> GetMediaContentSourceItemViewModels(
            IEnumerable<MediaContentSource> mediaContentSources)
        {
            var vms = new List<MediaContentSourceItemViewModel>();
            var contentSources = mediaContentSources as MediaContentSource[] ?? mediaContentSources.ToArray();
            var sortedContent = contentSources.OrderBy(x => x.SortOrder);

            foreach (var mediaContent in sortedContent)
            {
                var mediaVm = new MediaContentSourceItemViewModel(mediaContent);
                vms.Add(mediaVm);
            }

            return vms;
        }
    }
}
