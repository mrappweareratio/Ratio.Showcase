using Windows.System;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Media;
using Microsoft.Practices.Prism.StoreApps;
using Microsoft.VisualBasic;
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
using Strings = OneMSQFT.Common.Strings;

namespace OneMSQFT.UILogic.ViewModels
{
    public class ExhibitItemViewModel : ItemBaseViewModel, IHasMediaContentViewModel
    {
        public IExhibit<ICurator> ExhibitModel { get; private set; }

        public Uri ThumbnailImageUri { get; set; }
        public Color ExhibitColor { get; set; }
        public ObservableCollection<MediaContentSourceItemViewModel> MediaContent { get; set; }
        public Visibility MediaContentVisibility { get; set; }
        public override string Id { get; set; }
        public CuratorItemViewModel Curator { get; set; }
        public ObservableCollection<LinkItemViewModel> Links { get; set; }
        public Uri RsvpUrl { get; set; }
        public DateTime? DateStart;
        public DateTime? DateEnd;
        public DelegateCommand<string> ExitLinkCommand { get; private set; }
        public DelegateCommand<string> RsvpLinkCommand { get; private set; }

        public Visibility RsvpVisibility
        {
            get { return RsvpEnabled ? Visibility.Visible : Visibility.Collapsed; }
        }

        public bool RsvpEnabled
        {
            get
            {
                return ExhibitItemUtils.IsRsvpValid(this.RsvpUrl) && !ExhibitItemUtils.IsRsvpExpired(this.DateStart);
            }
        }


        public ExhibitItemViewModel(IExhibit<ICurator> exhibitModelModel)
        {
            if (exhibitModelModel == null)
                return;
            ExhibitModel = exhibitModelModel;
            Name = exhibitModelModel.Name;
            Id = exhibitModelModel.Id;
            Description = exhibitModelModel.Description;
            SquareFootage = exhibitModelModel.SquareFootage;
            Uri thumbnailImageUri;
            if (Uri.TryCreate(exhibitModelModel.ThumbImage, UriKind.RelativeOrAbsolute, out thumbnailImageUri))
            {
                ThumbnailImageUri = thumbnailImageUri;
            }
            LoadMediaContent(exhibitModelModel.MediaContent);
            LoadLinks(exhibitModelModel.Links);
            ExhibitColor = ColorUtils.GetExhibitColor(exhibitModelModel);
            Curator = new CuratorItemViewModel(exhibitModelModel.Curator);
            DateStart = exhibitModelModel.DateStart;
            DateEnd = exhibitModelModel.DateEnd;
            Uri rsvpUri;
            if (Uri.TryCreate(exhibitModelModel.RsvpUrl, UriKind.Absolute, out rsvpUri))
            {
                RsvpUrl = rsvpUri;
            }
            ExitLinkCommand = new DelegateCommand<string>(ExitLinkNavigate);
            RsvpLinkCommand = new DelegateCommand<string>(RsvpNavigate);
        }

        async private void RsvpNavigate(string s)
        {
            Uri uri;
            if (!Uri.TryCreate(s, UriKind.Absolute, out uri))
            {
                uri = ExhibitItemUtils.GetRsvpFallbackUri();
            }
            await Launcher.LaunchUriAsync(uri, new LauncherOptions { DesiredRemainingView = ViewSizePreference.UseHalf });
        }

        async private void ExitLinkNavigate(string s)
        {
            Uri uri;
            if (!Uri.TryCreate(s, UriKind.Absolute, out uri))
            {
                uri = ExhibitItemUtils.GetExitLinkFallbackUri();
            }
            await Launcher.LaunchUriAsync(uri, new LauncherOptions { DesiredRemainingView = ViewSizePreference.UseHalf });
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

        public String TileBlurb
        {
            get
            {
                return SquareFootageStringPlain + " " + Strings.SquareFeet + " " + Name;
            }
        }

        public String SnapBlurb
        {
            get
            {
                return "  " + Strings.SquareFeet + " at " + Name;
            }
        }
    }
}
