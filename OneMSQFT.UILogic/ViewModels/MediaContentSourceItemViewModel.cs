using Windows.UI.Xaml;
using Microsoft.Practices.Prism.StoreApps;
using OneMSQFT.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneMSQFT.UILogic.ViewModels
{
    public class MediaContentSourceItemViewModel : ItemBaseViewModel
    {
        public override string Id { get; set; }

        public MediaContentSourceItemViewModel(MediaContentSource image)
        {
            if (image.ContentSourceType == ContentSourceType.Video)
                throw new ArgumentOutOfRangeException("MediaContentSourceItemViewModel Constructor for an Image");
            IsVideoButtonVisible = Visibility.Collapsed;
            ImageSource = new Uri(image.Source, UriKind.RelativeOrAbsolute);
        }

        public MediaContentSourceItemViewModel(MediaContentSource video, MediaContentSource image)
        {
            if(!String.Equals(image.ParentId, video.Id))
                throw new ArgumentOutOfRangeException("video Id does not Match Image Parent Id");
            IsVideoButtonVisible = Visibility.Visible;
            ImageSource = new Uri(image.Source, UriKind.RelativeOrAbsolute);
            MediaSource = new Uri(video.Source, UriKind.RelativeOrAbsolute);
        }

        public Visibility IsVideoButtonVisible { get; set; }        

        public Uri ImageSource { get; set; }        

        public Uri MediaSource { get; set; }        
    }
}
