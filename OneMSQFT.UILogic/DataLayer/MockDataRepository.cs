using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Newtonsoft.Json;
using OneMSQFT.Common.DataLayer;
using OneMSQFT.Common.Models;

namespace OneMSQFT.UILogic.DataLayer
{
    public class MockDataRepository : IDataRepository
    {
        private LocalStorageManager _localStorage;
        private string jsonFileName = "sundance.json";

        public MockDataRepository()
        {
            _localStorage = new LocalStorageManager(new MockLocalStorageProvider());
        }

        public async Task<TimelineResult> LoadAllData()
        {
            string jsonData;
            jsonData = await _localStorage.LoadFile(jsonFileName);

            //Convert to model data
            var dataFromJSON = JsonConvert.DeserializeObject<TimelineResult>(jsonData);
            //Task<TimelineResult> timelineData = new TimelineResult(dataFromJSON);

            //return timelineData;
            return dataFromJSON;
        }

        public Task SaveAllData()
        {
            throw new NotImplementedException();
        }

        public Task<ObservableCollection<Event>> GetEvents()
        {
            throw new NotImplementedException();
        }

        public Task<ObservableCollection<Curator>> GetCuratorsByEvent(int eventId)
        {
            throw new NotImplementedException();
        }

        public Task<ObservableCollection<Exhibit>> GetExhibitsByEvent(int eventId)
        {
            throw new NotImplementedException();
        }

        public Task<ObservableCollection<Exhibit>> GetExhibitsByCurator(int curatorId)
        {
            throw new NotImplementedException();
        }

        public Task<ObservableCollection<Exhibit>> SearchExhibits(string[] tags)
        {
            throw new NotImplementedException();
        }
    }
}
