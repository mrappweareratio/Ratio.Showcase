using System;
using System.Collections.Generic;
using System.Linq;
using Ratio.Showcase.Shared.Models;
using Ratio.Showcase.Shared.Services;
using Ratio.Showcase.UILogic.ViewModels;

namespace Ratio.Showcase.UILogic.Utils
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

        public static string GetDefaultVideoUrl(MediaContentSource media)
        {
            return GetVideoUrlsByNetworkCost(media)[NetworkCost.Normal];
        }

        public static bool HasVideoUrl(MediaContentSource media)
        {
            return !String.IsNullOrEmpty(media.VideoUrlHd) || !String.IsNullOrEmpty(media.VideoUrlSd) || !String.IsNullOrEmpty(media.VideoUrlMobile);
        }

        public static bool ValidateMediaContent(MediaContentSource mediaContent)
        {
            if (mediaContent == null)
                return false;
            if (mediaContent.ContentSourceType == ContentSourceType.Image)
            {
                return !String.IsNullOrEmpty(mediaContent.Img);
            }
            return HasVideoUrl(mediaContent);
        }

        public static Uri GetVideoUrl(MediaContentSource media, IInternetConnection internetConnection)
        {
            Uri uri;
            var url = GetVideoUrlsByNetworkCost(media)[internetConnection.CostGuidance.Cost];
            return Uri.TryCreate(url, UriKind.RelativeOrAbsolute, out uri) ? uri : null;
        }

        public static Dictionary<NetworkCost, string> GetVideoUrlsByNetworkCost(MediaContentSource media)
        {
            var retVal = new Dictionary<NetworkCost, string>();
            retVal.Add(NetworkCost.Normal,
                String.IsNullOrEmpty(media.VideoUrlHd)
                ? String.IsNullOrWhiteSpace(media.VideoUrlSd)
                    ? String.IsNullOrWhiteSpace(media.VideoUrlMobile)
                        ? null
                        : media.VideoUrlMobile//last
                    : media.VideoUrlSd//2nd
                : media.VideoUrlHd);//1st
            retVal.Add(NetworkCost.Conservative,
                String.IsNullOrEmpty(media.VideoUrlSd)
                ? String.IsNullOrWhiteSpace(media.VideoUrlMobile)
                    ? String.IsNullOrWhiteSpace(media.VideoUrlHd)
                        ? null
                        : media.VideoUrlHd //last
                    : media.VideoUrlMobile//2nd
                : media.VideoUrlSd);//1st
            retVal.Add(NetworkCost.OptIn,
                String.IsNullOrEmpty(media.VideoUrlMobile)
                ? String.IsNullOrWhiteSpace(media.VideoUrlSd)
                    ? String.IsNullOrWhiteSpace(media.VideoUrlHd)
                        ? null
                        : media.VideoUrlHd
                    : media.VideoUrlSd
                : media.VideoUrlMobile);
            return retVal;
        }
    }
}
