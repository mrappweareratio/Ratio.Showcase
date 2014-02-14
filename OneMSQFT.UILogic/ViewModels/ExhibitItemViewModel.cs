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
using OneMSQFT.UILogic.Utils;

namespace OneMSQFT.UILogic.ViewModels
{
    public class ExhibitItemViewModel : ItemBaseViewModel, IHasMediaContentViewModel
    {
        public override string Id { get; set; }

        public ExhibitItemViewModel(IExhibit exhibitModel)
        {
            if (exhibitModel == null)
                return;
            Exhibit = exhibitModel;
            Name = exhibitModel.Name;
            Id = exhibitModel.Id;
            Description = exhibitModel.Description;
            SquareFootage = exhibitModel.SquareFootage;
            LoadMediaContent(exhibitModel.MediaContent);
            ColorBrush = ColorUtils.GetExhibitColor(exhibitModel);
        }

        private void LoadMediaContent(IEnumerable<MediaContentSource> mediaContent)
        {
            var mediaContentViewModels = MediaContentSourceUtils.GetMediaContentSourceItemViewModels(mediaContent).ToList();
            MediaContent = new ObservableCollection<MediaContentSourceItemViewModel>(mediaContentViewModels);
            MediaContentVisibility = MediaContent.Any() ? Visibility.Visible : Visibility.Collapsed;
            
            var firstImage = mediaContentViewModels.FirstOrDefault(x => x.ContentSourceType == ContentSourceType.Image);
            if (firstImage == null)
                return;
            //todo remove HeroFilePath
            HeroPhotoFilePath = firstImage.ImageSource;
        }


        private IExhibit Exhibit { get; set; }

        public Uri HeroPhotoFilePath { get; set; }

        public SolidColorBrush ColorBrush { get; set; }

        public ObservableCollection<MediaContentSourceItemViewModel> MediaContent { get; set; }

        public Visibility MediaContentVisibility { get; set; }
    }
}
