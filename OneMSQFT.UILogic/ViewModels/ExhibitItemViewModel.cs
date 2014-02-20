using Windows.System;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Media;
using Microsoft.Practices.Prism.StoreApps;
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
        public Visibility RsvpVisibility { get; set; }
        public bool RsvpEnabled { get; set; }
        public DelegateCommand<string> ExitLinkCommand { get; private set; }
        public DelegateCommand<string> RsvpLinkCommand { get; private set; }


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

            //TODO: Confirm Rsvp visibility / enable logic
            if (string.IsNullOrEmpty(exhibitModel.RsvpUrl))
            {
                RsvpEnabled = false;
                RsvpVisibility = Visibility.Collapsed;
            }
            else
            {
                RsvpUrl = new Uri(exhibitModel.RsvpUrl, UriKind.RelativeOrAbsolute);
                RsvpEnabled = true;
                RsvpVisibility = Visibility.Visible;
            }
            ExitLinkCommand = new DelegateCommand<string>(ExitLinkNavigate, CanExitLinkNavigate);
            RsvpLinkCommand = new DelegateCommand<string>(RsvpNavigate, CanRsvpNavigate);
        }

        private bool CanRsvpNavigate(string s)
        {
            //TODO: Confirm Rsvp can navigate
            return true;
        }

        async private void RsvpNavigate(string s)
        {
            var la = new LauncherOptions { DesiredRemainingView = ViewSizePreference.UseHalf };
            if (s != null) await Launcher.LaunchUriAsync(new Uri(s), la);
        }

        private bool CanExitLinkNavigate(string s)
        {
            //TODO: Confirm ExitLink can navigate
            return true;
        }

        async private void ExitLinkNavigate(string s)
        {
            var la = new LauncherOptions { DesiredRemainingView = ViewSizePreference.UseHalf };
            if (s != null) await Launcher.LaunchUriAsync(new Uri(s), la);
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
