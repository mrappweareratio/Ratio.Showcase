using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Microsoft.Practices.Prism.StoreApps;
using OneMSQFT.Common.Models;
using OneMSQFT.UILogic.Interfaces.ViewModels;
using OneMSQFT.UILogic.Utils;
using OneMSQFT.UILogic.ViewModels;
using OneMSQFT.Common.Services;

namespace OneMSQFT.Windows.DesignViewModels
{
    public class DesignExhibitDetailsPageViewModel : BasePageViewModel, IExhibitDetailsPageViewModel
    {
        public DelegateCommand<MediaContentSourceItemViewModel> LaunchVideoCommand { get; set; }

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
                            Source = "http://www.1msqft.com/assets/img/cultivators/sundance/laBlogo/1.jpg"
                        },
                        new MediaContentSource
                        {
                            ContentSourceType = ContentSourceType.Video,
                            Source = "http://www.1msqft.com/assets/img/cultivators/sundance/kenMiller/2.jpg"
                        }

                    },
                Exhibitor = "Artist Name",
                SquareFootage = Convert.ToInt32(id) * 1234 + 123,
                Color = "0" + id + "C" + id + "F" + id
            });
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
                            Source = "http://www.1msqft.com/assets/img/2.2/Sundance_hero_s.jpg"
                        },
                        new MediaContentSource
                        {
                            ContentSourceType = ContentSourceType.Video,
                            Source = "http://smf.blob.core.windows.net/samples/videos/bigbuck.mp4"
                        }

                    },
                    SquareFootage = Convert.ToInt32(i.ToString() + i.ToString() + i.ToString() + i.ToString()),
                });
                SquareFootEvents.Add(eivm);
            }

            LaunchVideoCommand = new DelegateCommand<MediaContentSourceItemViewModel>(LaunchVideoCommandHandler);
        }

        private void PopulateExhibitMediaCollection()
        {
            var mediaContentSources = new List<MediaContentSource>();

            mediaContentSources.Add(new MediaContentSource()
            {
                ContentSourceType = ContentSourceType.Video,
                Id = "Video0",
                Source = "http://smf.blob.core.windows.net/samples/videos/bigbuck.mp4",
            });

            mediaContentSources.Add(new MediaContentSource()
            {
                ContentSourceType = ContentSourceType.Image,
                ParentId = "Video0",
                Source = "http://www.1msqft.com/assets/img/cultivators/sundance/kenMiller/1.jpg"
            });

            mediaContentSources.Add(new MediaContentSource()
            {
                ContentSourceType = ContentSourceType.Video,
                Id = "a78dd60b6d00d4ea3cb24c04f8123fc5",
                Source = "http://player.vimeo.com/external/85202186.hd.mp4?s=a78dd60b6d00d4ea3cb24c04f8123fc5",
            });

            mediaContentSources.Add(new MediaContentSource()
            {
                ContentSourceType = ContentSourceType.Image,
                ParentId = "a78dd60b6d00d4ea3cb24c04f8123fc5",
                Source = "http://www.1msqft.com/assets/img/cultivators/sundance/kenMiller/1.jpg"
            });

            mediaContentSources.Add(new MediaContentSource()
          {
              ContentSourceType = ContentSourceType.Video,
              Id = "678c1a9e12fef16bc4db912bd6c69def",
              Source = "http://player.vimeo.com/external/85202186.sd.mp4?s=678c1a9e12fef16bc4db912bd6c69def"
          });
            mediaContentSources.Add(new MediaContentSource()
            {
                ContentSourceType = ContentSourceType.Image,
                ParentId = "678c1a9e12fef16bc4db912bd6c69def",
                Source = "http://www.1msqft.com/assets/img/cultivators/sundance/kenMiller/1.jpg"
            });

            mediaContentSources.Add(new MediaContentSource()
          {
              ContentSourceType = ContentSourceType.Video,
              Id = "135089443cf7c01d8762cc206a7cc5e7",
              Source = "http://player.vimeo.com/external/85202186.m3u8?p=high,standard,mobile&s=135089443cf7c01d8762cc206a7cc5e7"
          });

            mediaContentSources.Add(new MediaContentSource()
            {
                ContentSourceType = ContentSourceType.Image,
                ParentId = "135089443cf7c01d8762cc206a7cc5e7",
                Source = "http://www.1msqft.com/assets/img/cultivators/sundance/kenMiller/1.jpg"
            });         

            //Source = "http://player.vimeo.com/external/85202186.sd.mp4?s=678c1a9e12fef16bc4db912bd6c69def",                    
            //Source = "http://player.vimeo.com/external/85202186.mobile.mp4?s=0a50d2c088856672f467f909fc1a2c8c",                            
            //Source = "http://player.vimeo.com/external/85202186.m3u8?p=high,standard,mobile&s=135089443cf7c01d8762cc206a7cc5e7",
            MediaContentCollection = new ObservableCollection<MediaContentSourceItemViewModel>(MediaContentSourceUtils.GetMediaContentSourceItemViewModels(mediaContentSources));
        }

        async public void LaunchVideoCommandHandler(MediaContentSourceItemViewModel item)
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

        public SolidColorBrush EventColor
        {
            get
            {
                return Exhibit.ColorBrush;
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

        public String Panel0Title
        {
            get
            {
                return Exhibit.SquareFootage + " square feet at " + Exhibit.Name;
            }
        }

        public String Panel0Description
        {
            get
            {
                return Exhibit.Description;
            }
        }

        public Uri HeroPhotoFilePath
        {
            get
            {
                return Exhibit.HeroPhotoFilePath;
            }
        }

        public String Panel1LongDescription
        {
            get
            {
                return Exhibit.Description;
            }
        }

        public ObservableCollection<MediaContentSourceItemViewModel> MediaContentCollection { get; set; }

        #endregion

        #region ResizingProperties

        public double FullScreenWidth
        {
            get
            {
                return Window.Current.Bounds.Width;
            }
        }
        public double FullScreenHeight
        {
            get
            {
                return Window.Current.Bounds.Height;
            }
        }

        public double ExhibitItemWidth
        {
            get
            {
                return Window.Current.Bounds.Width * .9;
            }
        }

        public double ExhibitItemHeight
        {
            get
            {
                return FullScreenHeight;
            }
        }

        public double OneThirdPanelWidth
        {
            get { return ExhibitItemWidth / 3; }
        }

        public void WindowSizeChanged(double width, double height)
        {
            OnPropertyChanged("FullScreenHeight");
            OnPropertyChanged("FullScreenWidth");
            OnPropertyChanged("ExhibitItemHeight");
            OnPropertyChanged("ExhibitItemWidth");
        }

        #endregion
    }
}
