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

        public Task<ExhibitDetail> GetExhibitDetailByExhibitId(string exhibitId)
        {
            return GetExhibitDetailByExhibitIdDelegate(exhibitId);
        }
    }
}
