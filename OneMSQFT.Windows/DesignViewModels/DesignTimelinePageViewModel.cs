using System;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using OneMSQFT.Common.Models;
using OneMSQFT.UILogic.Interfaces.ViewModels;
using OneMSQFT.UILogic.ViewModels;
using Microsoft.Practices.Prism.StoreApps;
using System.Threading.Tasks;
using System.Globalization;

namespace OneMSQFT.WindowsStore.DesignViewModels
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
                    SquareFootage = Convert.ToInt32(i.ToString() + i.ToString() + i.ToString() + i.ToString())
                });

                var fakeExhibitsCount = Math.Round((decimal) new Random().Next(1, 10));
                for (var j = 0; j < fakeExhibitsCount; j++)
                {
                    var exhibit = new ExhibitItemViewModel(new Exhibit()
                    {
                        Id = j.ToString(),
                        Name = "Exhibit Name " + j,
                        Description = "Exhibit Description Name " + j,
                        SquareFootage = j * 123 + 123
                    }, null);
                    eivm.Exhibits.Add(exhibit);
                }         
                SquareFootEvents.Add(eivm);
                _totalSquareFeet = _totalSquareFeet + eivm.SquareFootage;
            }

            EventHeroItemClickCommand = new DelegateCommand<EventItemViewModel>(EventHeroItemClickCommandHandler);
            SetStartupCommand = new DelegateCommand(() => { });
            ClearStartupCommand = new DelegateCommand(() => { });
            ClearStartupVisibility = Visibility.Collapsed;
            SetStartupVisibility = Visibility.Collapsed;
        }

        public ObservableCollection<EventItemViewModel> TimeLineItems
        {
            get
            {
                var c = new ObservableCollection<EventItemViewModel>();
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
                }
                c.Insert(2, new EventItemViewModel(new Event() { Name = "Coming Soon", Color = "DDDDDD" }));
                c.Insert(3, new EventItemViewModel(new Event() { Name = "Coming Soon", Color = "DDDDDD" }));
                c.Insert(8, new EventItemViewModel(new Event() { Name = "Coming Soon", Color = "DDDDDD" }));
                c.Insert(10, new EventItemViewModel(new Event() { Name = "Coming Soon", Color = "DDDDDD" }));
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
                return (String.Format(CultureInfo.InvariantCulture, "{0:# ### ###}", _totalSquareFeet)).Trim() + " sqft";
            }
        }

        #region ResizingProperties

        public double ZoomedOutGridHeight
        {
            get
            {
                return FullScreenHeight;
            }
        }

        public double ZoomedOutItemWidth
        {
            get
            {
                return (FullScreenWidth-44) / 4; // 42 is GridView padding
            }
        }
        public double ZoomedOutItemHeight
        {
            get
            {
                return ((ZoomedOutGridHeight-165) / 3)-18;
            }
        }

        public double EventItemWidth
        {
            get
            {
                return FullScreenWidth *.9;
            }
        }
        public double EventItemHeight
        {
            get
            {
                return FullScreenHeight;
            }
        }

        public double FullScreenWidth { get; set; }

        public double FullScreenHeight { get; set; }

        public double ExhibitItemWidth
        {
            get
            {
                return (EventItemWidth / 3) - 1;
            }
        }
        public double ExhibitItemHeight
        {
            get
            {
                return (EventItemHeight / 4) - 1;
            }
        }
        public override void WindowSizeChanged(double width, double height)
        {
            FullScreenHeight = height;
            FullScreenWidth = width;
            OnPropertyChanged("ZoomedOutItemWidth");
            OnPropertyChanged("ZoomedOutItemHeight");
            OnPropertyChanged("EventItemHeight");
            OnPropertyChanged("EventItemWidth");
            OnPropertyChanged("ExhibitItemWidth");
            OnPropertyChanged("ExhibitItemHeight");
            OnPropertyChanged("ZoomedOutGridHeight");
        }

        #endregion

        public void EventHeroItemClickCommandHandler(EventItemViewModel item)
        {
            if (item == null) return;
            SelectedEvent = item;
        }

        public double BufferItemWidth
        {
            get
            {
                return EventItemWidth / 2;
            }
        }

        public double MaskItemWidth
        {
            get
            {
                return ((FullScreenWidth - EventItemWidth) / 2) + 1;
            }
        }

        public DelegateCommand SetStartupCommand { get; private set; }
        public DelegateCommand ClearStartupCommand { get; private set; }     
        public Visibility ClearStartupVisibility { get; set; }
        public Visibility SetStartupVisibility { get; set; }
    }
}
