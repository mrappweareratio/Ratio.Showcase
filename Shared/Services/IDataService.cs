using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ratio.Showcase.Shared.Models;

namespace Ratio.Showcase.Shared.Services
{
    public interface IDataService
    {
        Task<IEnumerable<Event>> GetEvents(CancellationToken token);      
        Task<ExhibitDetail> GetExhibitDetailByExhibitId(string exhibitId, CancellationToken token);
        Task<Event> GetEventById(string eventId, CancellationToken token);
        void ClearDataCacheInStorage();
    }
}