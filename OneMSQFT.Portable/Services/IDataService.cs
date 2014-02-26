using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using OneMSQFT.Common.Models;

namespace OneMSQFT.Common.Services
{
    public interface IDataService
    {
        Task<IEnumerable<Event>> GetEvents();      
        Task<ExhibitDetail> GetExhibitDetailByExhibitId(string exhibitId);
        Task<Event> GetEventById(string eventId);
    }
}