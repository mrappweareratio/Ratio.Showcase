using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ratio.Showcase.Shared.Models;
using Ratio.Showcase.Shared.Services;

namespace Ratio.Showcase.UILogic.Tests.Mocks
{
    public class MockDataService : IDataService
    {
        public Func<Task<IEnumerable<Event>>> GetEventsDelegate { get; set; }

        public Func<String, Task<ExhibitDetail>> GetExhibitDetailByExhibitIdDelegate { get; set; }

        public Func<String, Task<Event>> GetEventByIdDelegate { get; set; }

        public MockDataService()
        {
        }

        public Task<IEnumerable<Event>> GetEvents(CancellationToken token)
        {
            if(GetEventsDelegate != null)
                return GetEventsDelegate();
            return Task.FromResult<IEnumerable<Event>>(new List<Event>());
        }

        public Task<ExhibitDetail> GetExhibitDetailByExhibitId(string exhibitId, CancellationToken token)
        {
            if (GetExhibitDetailByExhibitIdDelegate != null)
                return GetExhibitDetailByExhibitIdDelegate(exhibitId);
            return Task.FromResult<ExhibitDetail>(null);
        }

        public Task<Event> GetEventById(string eventId, CancellationToken token)
        {
            if (GetEventByIdDelegate != null)
                return GetEventByIdDelegate(eventId);
            return Task.FromResult<Event>(null);
        }

        public void ClearDataCacheInStorage()
        {
        }
    }
}
