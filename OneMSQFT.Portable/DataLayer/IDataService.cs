using System.Collections.ObjectModel;
using System.Threading.Tasks;
using OneMSQFT.Common.Models;

namespace OneMSQFT.Common.DataLayer
{
    public interface IDataService
    {
        Task<ObservableCollection<Event>> GetEvents();
        Task<ObservableCollection<Curator>> GetCuratorsByEvent(int eventId);
        Task<ObservableCollection<Exhibit>> GetExhibitsByEvent(int eventId);
        Task<ObservableCollection<Exhibit>> GetExhibitsByCurator(int curatorId);
        Task<ObservableCollection<Exhibit>> SearchExhibits(string[] tags);
    }
}