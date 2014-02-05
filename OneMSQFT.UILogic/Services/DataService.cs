using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OneMSQFT.Common.DataLayer;
using OneMSQFT.Common.Models;
using OneMSQFT.Common.Services;

namespace OneMSQFT.UILogic.Services
{
    public class DataService : IDataService
    {
        private readonly IDataRepository _repository;

        public DataService(IDataRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Event>> GetEvents()
        {
            var result = await _repository.LoadAllData();
            return result.Events;
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
    }
}
