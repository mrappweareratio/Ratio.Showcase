using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;
using Microsoft.Practices.Prism.StoreApps;
using Ratio.Showcase.Shared.Models;
using Ratio.Showcase.UILogic.Interfaces.ViewModels;
using Ratio.Showcase.UILogic.Utils;
using Ratio.Showcase.UILogic.ViewModels;

namespace Ratio.Showcase.Win8.DesignViewModels
{
    public class DesignExhibitDetailsPageViewModel : BasePageViewModel, IExhibitDetailsPageViewModel
    {
        public DelegateCommand<MediaContentSourceItemViewModel> LaunchVideoCommand { get; set; }
        public DelegateCommand SetStartupCommand { get; private set; }
        public DelegateCommand ClearStartupCommand { get; private set; }

        public override void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            if (!(navigationParameter is String))
                throw new ArgumentOutOfRangeException("No Exhibit Id String");

            var id = navigationParameter as String;
            var exhibit = new ExhibitItemViewModel(new Exhibit()
            {
                Id = id,
                Name = "Exhibit Name " + id,
                Description = "Exhibit Description Name " + id,
                MediaContent = new List<MediaContentSource>
                    {
                        new MediaContentSource
                        {
                            ContentSourceType = ContentSourceType.Image,
                            Img = "http://www.1msqft.com/assets/img/cultivators/sundance/laBlogo/1.jpg"
                        },
                        new MediaContentSource
                        {
                            ContentSourceType = ContentSourceType.Video,
                            VideoUrlHd = "http://www.1msqft.com/assets/img/cultivators/sundance/kenMiller/2.jpg"
                        }

                    },
                Exhibitor = "Artist Name",
                SquareFootage = Convert.ToInt32(id) * 1234 + 123,
                Color = "0" + id + "C" + id + "F" + id
            }, null);
            Exhibit = exhibit;

            base.OnNavigatedTo(navigationParameter, navigationMode, viewModelState);
        }

        public DesignExhibitDetailsPageViewModel()
        {
            SquareFootEvents = new ObservableCollection<EventItemViewModel>();
            const int fakeEventsCount = 10;
            for (var i = 1; i < fakeEventsCount + 1; i++)
            {
                var eivm = new EventItemViewModel(new Event()
                {
                    Color = (i * 2).ToString() + i.ToString() + (i * 2).ToString() + i.ToString() + i.ToString() + i.ToString(),
                    Description = "Description Description " + i,
                    Id = i.ToString(),
                    Name = "Event Name " + i,
                    DateStart = DateTime.Now.Add(TimeSpan.FromDays(i * (i > fakeEventsCount / 2 ? 20 : -20) + 1)),
                    MediaContent = new List<MediaContentSource>
                    {
                        new MediaContentSource
                        {
                            ContentSourceType = ContentSourceType.Image,
                            Img = "http://www.1msqft.com/assets/img/2.2/Sundance_hero_s.jpg"
                        },
                        new MediaContentSource
                        {
                            ContentSourceType = ContentSourceType.Video,
                            VideoUrlHd = "http://smf.blob.core.windows.net/samples/videos/bigbuck.mp4"
                        }

                    },
                    SquareFootage = Convert.ToInt32(i.ToString() + i.ToString() + i.ToString() + i.ToString()),
                });
                SquareFootEvents.Add(eivm);
            }

            LaunchVideoCommand = new DelegateCommand<MediaContentSourceItemViewModel>(LaunchVideoCommandHandler);
            SetStartupCommand = new DelegateCommand(() => { });
            ClearStartupCommand = new DelegateCommand(() => { });
            ClearStartupVisibility = Visibility.Collapsed;
            SetStartupVisibility = Visibility.Collapsed;
        }

        private void PopulateExhibitMediaCollection()
        {
            var mediaContentSources = new List<MediaContentSource>();

            mediaContentSources.Add(new MediaContentSource()
            {
                ContentSourceType = ContentSourceType.Video,
                Id = "Video0",
                VideoUrlHd = "http://smf.blob.core.windows.net/samples/videos/bigbuck.mp4",
            });

            mediaContentSources.Add(new MediaContentSource()
            {
                ContentSourceType = ContentSourceType.Image,
                Img = "http://www.1msqft.com/assets/img/cultivators/sundance/kenMiller/1.jpg"
            });

            mediaContentSources.Add(new MediaContentSource()
            {
                ContentSourceType = ContentSourceType.Video,
                Id = "a78dd60b6d00d4ea3cb24c04f8123fc5",
                VideoUrlHd = "http://player.vimeo.com/external/85202186.hd.mp4?s=a78dd60b6d00d4ea3cb24c04f8123fc5",
            });

            mediaContentSources.Add(new MediaContentSource()
            {
                ContentSourceType = ContentSourceType.Image,
                Img = "http://www.1msqft.com/assets/img/cultivators/sundance/kenMiller/1.jpg"
            });

            mediaContentSources.Add(new MediaContentSource()
          {
              ContentSourceType = ContentSourceType.Video,
              Id = "678c1a9e12fef16bc4db912bd6c69def",
              VideoUrlHd = "http://player.vimeo.com/external/85202186.sd.mp4?s=678c1a9e12fef16bc4db912bd6c69def"
          });
            mediaContentSources.Add(new MediaContentSource()
            {
                ContentSourceType = ContentSourceType.Image,
                Img = "http://www.1msqft.com/assets/img/cultivators/sundance/kenMiller/1.jpg"
            });

            mediaContentSources.Add(new MediaContentSource()
          {
              ContentSourceType = ContentSourceType.Video,
              Id = "135089443cf7c01d8762cc206a7cc5e7",
              VideoUrlHd = "http://player.vimeo.com/external/85202186.m3u8?p=high,standard,mobile&s=135089443cf7c01d8762cc206a7cc5e7"
          });

            mediaContentSources.Add(new MediaContentSource()
            {
                ContentSourceType = ContentSourceType.Image,
                Img = "http://www.1msqft.com/assets/img/cultivators/sundance/kenMiller/1.jpg"
            });         

            //Source = "http://player.vimeo.com/external/85202186.sd.mp4?s=678c1a9e12fef16bc4db912bd6c69def",                    
            //Source = "http://player.vimeo.com/external/85202186.mobile.mp4?s=0a50d2c088856672f467f909fc1a2c8c",                            
            //Source = "http://player.vimeo.com/external/85202186.m3u8?p=high,standard,mobile&s=135089443cf7c01d8762cc206a7cc5e7",
            MediaContentCollection = new ObservableCollection<MediaContentSourceItemViewModel>(MediaContentSourceUtils.GetMediaContentSourceItemViewModels(mediaContentSources));
        }

        public void LaunchVideoCommandHandler(MediaContentSourceItemViewModel item)
        {
            if (item == null) return;
            SelectedMediaContentSource = item;
        }

        public ExhibitItemViewModel NextExhibit { get; private set; }
        public DelegateCommand<string> NextExhibitCommand { get; private set; }

        #region ContentProperties

        private MediaContentSourceItemViewModel _selectedMediaContentSource;
        public MediaContentSourceItemViewModel SelectedMediaContentSource
        {
            get
            {
                return _selectedMediaContentSource;
            }
            set
            {
                if (value != null)
                {
                    SetProperty(ref _selectedMediaContentSource, value);
                }
            }
        }

        public Color EventColor
        {
            get
            {
                return Exhibit.ExhibitColor;
            }
        }

        private ExhibitItemViewModel _exhibit;

        public ExhibitItemViewModel Exhibit
        {
            get
            {
                return _exhibit;
            }
            set
            {
                if (value != null)
                {
                    SetProperty(ref _exhibit, value);
                    PopulateExhibitMediaCollection();
                }
            }
        }

        public Uri ThumbnailImageUri
        {
            get
            {
                return Exhibit.ThumbnailImageUri;
            }
        }

        public ObservableCollection<MediaContentSourceItemViewModel> MediaContentCollection { get; set; }

        #endregion

        #region ResizingProperties

        public double NineSixteenthsOfWidth
        {
            get
            {
                return (FullScreenWidth / 16) * 9;
            }
        }
        public double OneThirdPanelWidth
        {
            get { return FullScreenWidth * .325; } // approx one third (per comp)
        }

        public double PortraitHeaderFooterHeight
        {
            get { return FullScreenHeight * .275; } // approx one third (per comp)
        }

        public override void WindowSizeChanged(double width, double height)
        {
            base.WindowSizeChanged(width, height);
            OnPropertyChanged("NineSixteenthsOfWidth");
            OnPropertyChanged("OneThirdPanelWidth");
            OnPropertyChanged("PortraitHeaderFooterHeight");
        }

        #endregion       

        public Visibility ClearStartupVisibility { get; set; }
        public Visibility SetStartupVisibility { get; set; }
    }
}
