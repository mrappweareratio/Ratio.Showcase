using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OneMSQFT.Common.Models;

namespace OneMSQFT.Common.DataLayer
{
    public interface IDataRepository
    {
        Task Timeline LoadAllData();
        Task SaveAllData();
        Task<ObservableCollection<Event>> GetEvents();
        Task<ObservableCollection<Curator>> GetCuratorsByEvent(int eventId);
        Task<ObservableCollection<Exhibit>> GetExhibitsByEvent(int eventId);
        Task<ObservableCollection<Exhibit>> GetExhibitsByCurator(int curatorId);
        Task<ObservableCollection<Exhibit>> SearchExhibits(string[] tags);
        //TODO: Search Events and curators?
    }
}
