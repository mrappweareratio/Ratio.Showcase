using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using OneMSQFT.Common.Models;

namespace OneMSQFT.Common.Services
{
    public interface IDataService
    {
        Task<IEnumerable<Event>> GetEvents(CancellationToken token);      
        Task<ExhibitDetail> GetExhibitDetailByExhibitId(string exhibitId, CancellationToken token);
        Task<Event> GetEventById(string eventId, CancellationToken token);
        void ClearDataCacheInStorage();
    }
}