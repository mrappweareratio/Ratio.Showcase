using System;
using System.Collections.ObjectModel;
using OneMSQFT.Common.Models;
using OneMSQFT.UILogic.Interfaces.ViewModels;
using OneMSQFT.UILogic.ViewModels;
using Microsoft.Practices.Prism.StoreApps;
using System.Threading.Tasks;
using System.Globalization;

namespace OneMSQFT.Windows.DesignViewModels
{
    public class DesignExhibitDetailsPageViewModel : BasePageViewModel, IExhibitDetailsPageViewModel
    {
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
                    PhotoFilePath = "http://www.1msqft.com/assets/img/2.2/Sundance_hero_s.jpg",
                    SquareFootage = Convert.ToInt32(i.ToString() + i.ToString() + i.ToString() + i.ToString()),
                    EventHeroVideoPath = "http://smf.blob.core.windows.net/samples/videos/bigbuck.mp4"
                });
                SquareFootEvents.Add(eivm);
            }
        }

        private double _fullScreenWidth;
        private double _fullScreenHeight;
        public double FullScreenWidth
        {
            get
            {
                return _fullScreenWidth;
            }
            set
            {
                if (value != null)
                {
                    SetProperty(ref _fullScreenWidth, value);
                }
            }
        }
        public double FullScreenHeight
        {
            get
            {
                return _fullScreenHeight;
            }
            set
            {
                if (value != null)
                {
                    SetProperty(ref _fullScreenHeight, value);
                }
            }
        }

        private double _exhibitItemWidth;
        private double _exhibitItemHeight;
        public double ExhibitItemWidth
        {
            get
            {
                return _exhibitItemWidth;
            }
            set
            {
                if (value != null)
                {
                    SetProperty(ref _exhibitItemWidth, value);
                }
            }
        }
        public double ExhibitItemHeight
        {
            get
            {
                return _exhibitItemHeight;
            }
            set
            {
                if (value != null)
                {
                    SetProperty(ref _exhibitItemHeight, value);
                }
            }
        }

        public void WindowSizeChanged(double width, double height)
        {
            FullScreenHeight = height;
            FullScreenWidth = width;
        }
    }
}
