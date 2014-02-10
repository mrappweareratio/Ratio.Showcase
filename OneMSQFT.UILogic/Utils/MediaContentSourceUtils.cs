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
            var videoGroups = contentSources
                .GroupBy(x => x.ContentSourceType == ContentSourceType.Image ? x.ParentId : x.Id)
                .Select(x => new
                {
                    VideoId = x.Key, 
                    Image = x.FirstOrDefault(item => item.ContentSourceType == ContentSourceType.Image), 
                    Video = x.FirstOrDefault(item => item.ContentSourceType == ContentSourceType.Video)
                })
                .ToList();            
                        
            foreach (var mediaContentSource in contentSources)
            {
                if (mediaContentSource.ContentSourceType == ContentSourceType.Image)
                {
                    if (String.IsNullOrEmpty(mediaContentSource.ParentId))
                    {                        
                        vms.Add(new MediaContentSourceItemViewModel(mediaContentSource));
                        continue;
                    }
                    var videoGroup = videoGroups.FirstOrDefault(x => x.VideoId == mediaContentSource.ParentId);
                    if (videoGroup == null || videoGroup.Image == null || videoGroup.Video == null)
                    {                        
                        vms.Add(new MediaContentSourceItemViewModel(mediaContentSource));
                        continue;
                    }
                    vms.Add(new MediaContentSourceItemViewModel(videoGroup.Video, videoGroup.Image));
                    videoGroups.Remove(videoGroup);
                }
                else
                {
                    var videoGroup = videoGroups.FirstOrDefault(x => x.VideoId == mediaContentSource.Id);
                    if (videoGroup == null || videoGroup.Image == null || videoGroup.Video == null)
                    {                                 
                        continue;
                    }
                    vms.Add(new MediaContentSourceItemViewModel(videoGroup.Video, videoGroup.Image));
                    videoGroups.Remove(videoGroup);
                }
            }
            return vms;
        }
    }
}
