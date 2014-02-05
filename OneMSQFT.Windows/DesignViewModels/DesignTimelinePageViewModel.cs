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
        private int _totalSquareFeet;
        public String TotalSquareFeet
        {
            get
            {
                return (String.Format(CultureInfo.InvariantCulture, "{0:# ### ###}", _totalSquareFeet)).Trim();
            }
        }
        public DelegateCommand<EventItemViewModel> EventHeroItemClickCommand { get; set; }
        public DesignTimelinePageViewModel()
        {
            SquareFootEvents = new ObservableCollection<EventItemViewModel>();

            const int fakeEventsCount = 10;
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
                SquareFootEvents.Add(eivm);
                _totalSquareFeet = _totalSquareFeet + eivm.SquareFootage;
            }

            EventHeroItemClickCommand = new DelegateCommand<EventItemViewModel>(EventHeroItemClickCommandHandler);
        }

        public ObservableCollection<EventItemViewModel> SquareFootEvents { get; set; }
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
        private double _fullScreenItemWidth;
        private double _fullScreenItemHeight;
        public double FullScreenItemWidth
        {
            get
            {
                return _fullScreenItemWidth;
            }
            set
            {
                if (value != null)
                {
                    SetProperty(ref _fullScreenItemWidth, value);
                }
            }
        }
        public double FullScreenItemHeight
        {
            get
            {
                return _fullScreenItemHeight;
            }
            set
            {
                if (value != null)
                {
                    SetProperty(ref _fullScreenItemHeight, value);
                }
            }
        }

        public void WindowSizeChanged(double width, double height)
        {
            ZoomedOutItemWidth = width / 6;
            ZoomedOutItemHeight = height / 4;
            FullScreenItemHeight = height;
            FullScreenItemWidth = width;
        }
        async public void EventHeroItemClickCommandHandler(EventItemViewModel item)
        {
            if (item == null) return;
            SelectedEvent = item;
        }
    }
}
