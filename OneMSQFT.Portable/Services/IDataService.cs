using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using OneMSQFT.Common.Models;

namespace OneMSQFT.Common.Services
{
    public interface IDataService
    {
        Task<IEnumerable<Event>> GetEvents();
        Task<IEnumerable<Curator>> GetCuratorsByEvent(int eventId);
        Task<IEnumerable<Exhibit>> GetExhibitsByEvent(int eventId);
        Task<IEnumerable<Exhibit>> GetExhibitsByCurator(int curatorId);
        Task<IEnumerable<Exhibit>> SearchExhibits(string[] tags);
    }
}