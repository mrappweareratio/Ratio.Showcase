using System;
using Windows.UI.Xaml;
using Microsoft.Practices.Prism.StoreApps;
using Ratio.Showcase.Shared.Models;
using Ratio.Showcase.Shared.Services;
using Ratio.Showcase.UILogic.Utils;

namespace Ratio.Showcase.UILogic.ViewModels
{
    public class MediaContentSourceItemViewModel : BindableBase
    {
        public string Id { get; set; }

        public MediaContentSourceItemViewModel(MediaContentSource media)
        {
            if (media == null)
                return;
            Media = media;
            Id = media.Id;
            if (media.ContentSourceType == ContentSourceType.Image)
            {
                ContentSourceType = ContentSourceType.Image;
                IsVideoButtonVisible = Visibility.Collapsed;
                ImageSource = new Uri(media.Img, UriKind.RelativeOrAbsolute);
            }
            else
            {
                ContentSourceType = ContentSourceType.Video;
                IsVideoButtonVisible = Visibility.Visible;
                VideoSource = new Uri(MediaContentSourceUtils.GetDefaultVideoUrl(media), UriKind.RelativeOrAbsolute);
                ImageSource = new Uri(media.Img, UriKind.RelativeOrAbsolute);
            }
        }

        public MediaContentSource Media { get; private set; }

        public ContentSourceType ContentSourceType { get; private set; }

        public Visibility IsVideoButtonVisible { get; private set; }

        public Uri ImageSource { get; private set; }

        public Uri VideoSource { get; private set; }

        public Uri GetVideoSourceByInternetConnection(IInternetConnection internetConnection)
        {
            VideoSource = MediaContentSourceUtils.GetVideoUrl(Media, internetConnection);
            return VideoSource;
        }
    }
}
