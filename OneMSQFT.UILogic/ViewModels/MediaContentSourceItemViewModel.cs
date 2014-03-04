using Windows.UI.Xaml;
using Microsoft.Practices.Prism.StoreApps;
using OneMSQFT.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OneMSQFT.Common.Services;
using OneMSQFT.UILogic.Utils;

namespace OneMSQFT.UILogic.ViewModels
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
            return MediaContentSourceUtils.GetVideoUrl(Media, internetConnection);            
        }
    }
}
