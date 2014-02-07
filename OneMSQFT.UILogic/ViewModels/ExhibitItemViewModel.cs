using Windows.UI;
using Windows.UI.Xaml.Media;
using OneMSQFT.Common.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace OneMSQFT.UILogic.ViewModels
{
    public class ExhibitItemViewModel : ItemBaseViewModel
    {
        public override string Id { get; set; }

        public ExhibitItemViewModel(IExhibit exhibitModel)
        {
            Exhibit = exhibitModel;
            Name = exhibitModel.Name;
            Id = exhibitModel.Id;
            Description = exhibitModel.Description;
            SquareFootage = exhibitModel.SquareFootage;
            LoadImages(exhibitModel.MediaContent);
        }

        private void LoadImages(IEnumerable<MediaContentSource> mediaContent)
        {
            if (mediaContent == null)
            {
                HeroPhotoFilePath = new Uri("ms-appx:///Assets/BG_AllWhite.png", UriKind.RelativeOrAbsolute);
                return;
            }
            var images = mediaContent.Where(x => x.ContentSourceType == ContentSourceType.Image);
            var firstImage = images.FirstOrDefault(x => x.ContentSourceType == ContentSourceType.Image);
            HeroPhotoFilePath = firstImage == null ? new Uri("ms-appx:///Assets/BG_AllWhite.png", UriKind.RelativeOrAbsolute) : new Uri(firstImage.Source, UriKind.Absolute);            
        }


        private IExhibit Exhibit { get; set; }

        public Uri HeroPhotoFilePath { get; set; }              

        public SolidColorBrush EventColor
        {
            get
            {
                string color = "FF" + Exhibit.Color;
                var c = Color.FromArgb(
                Convert.ToByte(color.Substring(0, 2), 16),
                Convert.ToByte(color.Substring(2, 2), 16),
                Convert.ToByte(color.Substring(4, 2), 16),
                Convert.ToByte(color.Substring(6, 2), 16));

                return new SolidColorBrush(c);
            }
        }


    }
}
