using Windows.UI.Xaml;
using Microsoft.Practices.Prism.StoreApps;
using OneMSQFT.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OneMSQFT.UILogic.Utils;

namespace OneMSQFT.UILogic.ViewModels
{
    public class MediaContentSourceItemViewModel : ItemBaseViewModel
    {
        public override string Id { get; set; }

        public MediaContentSourceItemViewModel(MediaContentSource media)
        {
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
                VideoSource = new Uri(MediaContentSourceUtils.SelectVideoUrl(media), UriKind.RelativeOrAbsolute);
                ImageSource = new Uri(media.Img, UriKind.RelativeOrAbsolute);
            }
        }

        public ContentSourceType ContentSourceType { get; set; }

        public Visibility IsVideoButtonVisible { get; set; }        

        public Uri ImageSource { get; set; }        

        public Uri VideoSource { get; set; }        
    }
}
