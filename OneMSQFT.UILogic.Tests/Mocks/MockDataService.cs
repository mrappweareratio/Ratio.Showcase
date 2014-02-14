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
            if(GetEventsDelegate != null)
                return GetEventsDelegate();
            return Task.FromResult<IEnumerable<Event>>(null);
        }      

        public Task<ExhibitDetail> GetExhibitDetailByExhibitId(string exhibitId)
        {
            if (GetEventsDelegate != null)
                return GetExhibitDetailByExhibitIdDelegate(exhibitId);
            return Task.FromResult<ExhibitDetail>(null);
        }
    }
}
