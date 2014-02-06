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
    public class DesignTimelinePageViewModel : BasePageViewModel, ITimelinePageViewModel
    {
        public DelegateCommand<EventItemViewModel> EventHeroItemClickCommand { get; set; }
        public DesignTimelinePageViewModel()
        {
            SquareFootEvents = new ObservableCollection<EventItemViewModel>();

            // construct Fake Data
            const int fakeEventsCount = 10;
            var fakeExhibits = new ObservableCollection<ExhibitItemViewModel>();
             
            for (var i = 1; i < fakeEventsCount+1; i++)
            {
                var eivm = new EventItemViewModel(new Event()
                {
                    Color = (i*2).ToString()+i.ToString()+(i*2).ToString()+i.ToString()+i.ToString()+i.ToString(),
                    Description = "Description Description " + i,
                    Id = i.ToString(),
                    Name = "Event Name " + i,
                    DateStart = DateTime.Now.Add(TimeSpan.FromDays(i * (i > fakeEventsCount / 2 ? 20 : -20) + 1)),
                    PhotoFilePath = "http://www.1msqft.com/assets/img/2.2/Sundance_hero_s.jpg",
                    SquareFootage = Convert.ToInt32(i.ToString() + i.ToString() + i.ToString() + i.ToString()),
                    EventHeroVideoPath ="http://smf.blob.core.windows.net/samples/videos/bigbuck.mp4"
                });

                var fakeExhibitsCount = Math.Round((decimal) new Random().Next(1, 10));
                for (var j = 0; j < fakeExhibitsCount; j++)
                {
                    var exhibit = new ExhibitItemViewModel(new Exhibit()
                    {
                        Id = j.ToString(),
                        Name = "Exhibit Name " + j,
                        Description = "Exhibit Description Name " + j,
                        PhotoFilePath = "http://www.1msqft.com/assets/img/cultivators/sundance/laBlogo/1.jpg",
                        SquareFootage = j * 1234 + 123
                    });
                    eivm.Exhibits.Add(exhibit);
                }         
                SquareFootEvents.Add(eivm);
                _totalSquareFeet = _totalSquareFeet + eivm.SquareFootage;
            }

            EventHeroItemClickCommand = new DelegateCommand<EventItemViewModel>(EventHeroItemClickCommandHandler);
        }

        public ObservableCollection<EventItemViewModel> TimeLineItems
        {
            get
            {
                var c = new ObservableCollection<EventItemViewModel>();
                c.Add(new EventItemViewModel(new Event() { 
                    Name = "Featured", 
                    Color="FFFFFF",
                    SquareFootage = _totalSquareFeet
                    }));
                foreach (var e in SquareFootEvents)
                {
                    c.Add(e);
                }
                return c as ObservableCollection<EventItemViewModel>;
            }
        }
        public ObservableCollection<EventItemViewModel> TimeLineMenuItems
        {
            get
            {
                var c = new ObservableCollection<EventItemViewModel>();
                foreach (var e in TimeLineItems)
                {
                    c.Add(e);
                    c.Add(new EventItemViewModel(new Event() { Name = "Spacer", Color = "FFFFFF" }));
                    c.Add(new EventItemViewModel(new Event() { Name = "Spacer", Color = "FFFFFF" }));
                }
                c.Add(new EventItemViewModel(new Event() { Name = "Spacer", Color = "FFFFFF" }));
                c.Add(new EventItemViewModel(new Event() { Name = "Spacer", Color = "FFFFFF" }));
                c.Add(new EventItemViewModel(new Event() { Name = "Spacer", Color = "FFFFFF" }));
                return c as ObservableCollection<EventItemViewModel>;
        } }

        private EventItemViewModel _selectedEvent;
        public EventItemViewModel SelectedEvent
        {
            get
            {
                return _selectedEvent;
            }
            set
            {
                if (value != null)
                {
                    SetProperty(ref _selectedEvent, value);
                }
            }
        }

        private int _totalSquareFeet;
        public String TotalSquareFeet
        {
            get
            {
                return (String.Format(CultureInfo.InvariantCulture, "{0:# ### ###}", _totalSquareFeet)).Trim();
            }
        }
        private double _zoomedOutItemWidth;
        private double _zoomedOutItemHeight;
        public double ZoomedOutItemWidth
        {
            get
            {
                return _zoomedOutItemWidth;
            }
            set
            {
                if (value != null)
                {
                    SetProperty(ref _zoomedOutItemWidth, value);
                }
            }
        }
        public double ZoomedOutItemHeight
        {
            get
            {
                return _zoomedOutItemHeight;
            }
            set
            {
                if (value != null)
                {
                    SetProperty(ref _zoomedOutItemHeight, value);
                }
            }
        }
        private double _eventItemWidth;
        private double _eventItemHeight;
        public double EventItemWidth
        {
            get
            {
                return _eventItemWidth;
            }
            set
            {
                if (value != null)
                {
                    SetProperty(ref _eventItemWidth, value);
                }
            }
        }
        public double EventItemHeight
        {
            get
            {
                return _eventItemHeight;
            }
            set
            {
                if (value != null)
                {
                    SetProperty(ref _eventItemHeight, value);
                }
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
                SetProperty(ref _exhibitItemWidth, value);
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
                SetProperty(ref _exhibitItemHeight, value);
            }
        }

        public void WindowSizeChanged(double width, double height)
        {
            FullScreenHeight = height;
            FullScreenWidth = width;
            ZoomedOutItemWidth = width / 6;
            ZoomedOutItemHeight = height / 4;
            EventItemHeight = height;
            EventItemWidth = width * .9;
            ExhibitItemWidth = (EventItemWidth / 3) - 1;
            ExhibitItemHeight = (EventItemHeight / 4) - 1;
        }
        async public void EventHeroItemClickCommandHandler(EventItemViewModel item)
        {
            if (item == null) return;
            SelectedEvent = item;
        }
    }
}
