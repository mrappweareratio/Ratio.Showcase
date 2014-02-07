using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OneMSQFT.Common.DataLayer;
using OneMSQFT.Common.Models;
using OneMSQFT.Common.Services;

namespace OneMSQFT.UILogic.Tests.Mocks
{
    public class MockDataService : IDataService
    {
        public Func<Task<IEnumerable<Event>>> GetEventsDelegate { get; set; }

        public Func<String, Task<ExhibitDetail>> GetExhibitDetailByExhibitIdDelegate { get; set; }

        public MockDataService()
        {
        }

        public Task<IEnumerable<Event>> GetEvents()
        {
            return GetEventsDelegate();
        }

        public Task<IEnumerable<Curator>> GetCuratorsByEvent(int eventId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Exhibit>> GetExhibitsByEvent(int eventId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Exhibit>> GetExhibitsByCurator(int curatorId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Exhibit>> SearchExhibits(string[] tags)
        {
            throw new NotImplementedException();
        }

        public Task<ExhibitDetail> GetExhibitDetailByExhibitId(string id)
        {
            return GetExhibitDetailByExhibitIdDelegate(id);
        }
    }
}
