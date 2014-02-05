using System;
using System.Threading.Tasks;
using OneMSQFT.Common.DataLayer;
using OneMSQFT.Common.Models;

namespace OneMSQFT.UILogic.Tests.Mocks
{
    public class MockDataRepository : IDataRepository
    {
        public Func<Task<TimelineResult>> LoadAllDataDelegate { get; set; }

        public Task<TimelineResult> LoadAllData()
        {
            return LoadAllDataDelegate();
        }        
    }
}