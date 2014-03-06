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
using OneMSQFT.Common.Services;
using OneMSQFT.UILogic.Utils;
using Strings = OneMSQFT.Common.Strings;

namespace OneMSQFT.UILogic.ViewModels
{
    public class ExhibitItemViewModel : ItemBaseViewModel, IHasMediaContentViewModel, IDatedItemViewModel
    {
        public Exhibit ExhibitModel { get; private set; }
        private readonly IAnalyticsService _analyticsService;

        public Uri ThumbnailImageUri { get; set; }
        public Color ExhibitColor { get; set; }
        public ObservableCollection<MediaContentSourceItemViewModel> MediaContent { get; set; }
        public Visibility MediaContentVisibility { get; set; }
        public override string Id { get; set; }
        public CuratorItemViewModel Curator { get; set; }
        public ObservableCollection<LinkItemViewModel> Links { get; set; }
        public Uri RsvpUrl { get; set; }
        public DelegateCommand<LinkItemViewModel> ExitLinkCommand { get; private set; }
        public DelegateCommand RsvpLinkCommand { get; private set; }

        public Visibility RsvpVisibility
        {
            get { return RsvpEnabled ? Visibility.Visible : Visibility.Collapsed; }
        }

        public bool RsvpEnabled
        {
            get { return ExhibitItemUtils.IsRsvpValid(this.RsvpUrl) && !IsInThePast.GetValueOrDefault(false); }
        }

        public bool? IsInTheFuture
        {
            get
            {
                return DateUtils.IsInTheFuture(ExhibitModel);
            }
        }

        public bool? IsInThePast
        {
            get
            {
                return DateUtils.IsInThePast(ExhibitModel);

            }
        }


        public ExhibitItemViewModel(Exhibit exhibitModelModel, IAnalyticsService analyticsService)
        {
            if (exhibitModelModel == null)
                return;
            _analyticsService = analyticsService;
            ExhibitModel = exhibitModelModel;
            Name = exhibitModelModel.Name;
            Id = exhibitModelModel.Id;
            Description = StringUtils.BuildDescription(exhibitModelModel.Description);
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
            Uri rsvpUri;
            if (Uri.TryCreate(exhibitModelModel.RsvpUrl, UriKind.Absolute, out rsvpUri))
            {
                RsvpUrl = rsvpUri;
            }
            ExitLinkCommand = new DelegateCommand<LinkItemViewModel>(ExitLinkNavigate);
            RsvpLinkCommand = new DelegateCommand(RsvpNavigate, RsvpLinkCommandCanExecuteMethod);
        }

        private bool RsvpLinkCommandCanExecuteMethod()
        {
            return RsvpEnabled;
        }

        async private void RsvpNavigate()
        {
            if (RsvpUrl == null)
                return;

            //Track link interaction
            if (_analyticsService != null)
            {
                _analyticsService.TrackLinkInteractionInExhibitView(this.Name, this.Id, RsvpUrl.ToString());
            }

            await Launcher.LaunchUriAsync(RsvpUrl, new LauncherOptions { DesiredRemainingView = ViewSizePreference.UseHalf });
        }

        async private void ExitLinkNavigate(LinkItemViewModel linkItemViewModel)
        {
            //Track link interaction
            if (_analyticsService != null)
            {
                _analyticsService.TrackLinkInteractionInExhibitView(this.Name, this.Id, linkItemViewModel.Title);
            }

            Uri uri;
            if (!Uri.TryCreate(linkItemViewModel.Url, UriKind.Absolute, out uri))
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
                return SquareFootageStringPlain + " " + Strings.SquareFeetAt + " " + Name;
            }
        }
    }
}
