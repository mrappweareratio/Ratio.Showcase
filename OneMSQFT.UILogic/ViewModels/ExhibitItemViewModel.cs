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
        private IExhibit<ICurator> Exhibit { get; set; }

        public Uri HeroPhotoFilePath { get; set; }
        public Color ExhibitColor { get; set; }
        public ObservableCollection<MediaContentSourceItemViewModel> MediaContent { get; set; }
        public Visibility MediaContentVisibility { get; set; }        
        public override string Id { get; set; }
        public CuratorItemViewModel Curator { get; set; }
        public ObservableCollection<LinkItemViewModel> Links { get; set; }
        public Uri RsvpUrl { get; set; }
        public DateTime? DateStart;
        public DateTime? DateEnd;

        public Visibility RsvpVisibility
        {
            get {
                if (ExhibitItemUtils.IsRsvpValid(this.RsvpUrl) && !ExhibitItemUtils.IsRsvpExpired(this.DateStart))
                {
                    return Visibility.Visible;
                }
                else
                {
                    return Visibility.Collapsed;
                }
            }
        }

        public bool RsvpEnabled
        {
            get
            {
                return ExhibitItemUtils.IsRsvpValid(this.RsvpUrl) && !ExhibitItemUtils.IsRsvpExpired(this.DateStart);
            }
        }


        public ExhibitItemViewModel(IExhibit<ICurator> exhibitModel)
        {
            if (exhibitModel == null)
                return;
            Exhibit = exhibitModel;
            Name = exhibitModel.Name;
            Id = exhibitModel.Id;
            Description = exhibitModel.Description;
            SquareFootage = exhibitModel.SquareFootage;
            HeroPhotoFilePath = new Uri(exhibitModel.ThumbImage, UriKind.RelativeOrAbsolute);
            LoadMediaContent(exhibitModel.MediaContent);
            LoadLinks(exhibitModel.Links);
            ExhibitColor = ColorUtils.GetExhibitColor(exhibitModel);
            Curator = new CuratorItemViewModel(exhibitModel.Curator);
            DateStart = exhibitModel.DateStart;
            DateEnd = exhibitModel.DateEnd;


            if (!string.IsNullOrEmpty(exhibitModel.RsvpUrl))
            {
                RsvpUrl = new Uri(exhibitModel.RsvpUrl, UriKind.RelativeOrAbsolute);
            }
        }

        private void LoadMediaContent(IEnumerable<MediaContentSource> mediaContent)
        {
            var mediaContentViewModels = MediaContentSourceUtils.GetMediaContentSourceItemViewModels(mediaContent).ToList();
            MediaContent = new ObservableCollection<MediaContentSourceItemViewModel>(mediaContentViewModels);
            MediaContentVisibility = MediaContent.Any() ? Visibility.Visible : Visibility.Collapsed;                                    
        }

        private void LoadLinks(IEnumerable<Link> links)
        {
            if (links == null)
                return;
            Links = new ObservableCollection<LinkItemViewModel>();
            foreach (var link in links)
            {
                Links.Add(new LinkItemViewModel(link));
            }
        }
    }
}
