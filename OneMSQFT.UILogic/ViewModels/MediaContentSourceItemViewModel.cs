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

        public MediaContentSourceItemViewModel(MediaContentSource mediaContentSource)
        {
            MediaContentSource = mediaContentSource;
        }

        private MediaContentSource MediaContentSource { get; set; }

        public Visibility IsVideoButtonVisible 
        {
            get
            {
                if (MediaContentSource.ContentSourceType == ContentSourceType.Video)
                {
                    return Visibility.Visible;
                }
                return Visibility.Collapsed;
            }
        }

        public Uri ImageSource
        {
            get
            {
                if (MediaContentSource.ContentSourceType == ContentSourceType.Image)
                {
                    return new Uri(MediaContentSource.Source, UriKind.Absolute);
                }
                return new Uri(MediaContentSource.ThumbnailSource, UriKind.Absolute);
            }
        }

        public Uri MediaSource
        {
            get
            {
                return new Uri(MediaContentSource.Source, UriKind.Absolute);
            }
        }

    }
}
