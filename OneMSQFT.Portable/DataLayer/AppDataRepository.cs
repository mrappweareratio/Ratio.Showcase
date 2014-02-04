using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OneMSQFT.Common.Models;
using OneMSQFT.Windows

namespace OneMSQFT.Common.DataLayer
{
    class AppDataRepository: IDataRepository
    {
        private LocalStorageManager _localStorage;
        private bool _useMockData;
        private string jsonFileName = "timeline.json";
        private Timeline _timeline;

        public AppDataRepository(ILocalStorageProvider localStorage, bool useMockData)
        {
            _localStorage = new LocalStorageManager(localStorage);
            _useMockData = useMockData;
        }
 
        public Timeline LoadAllData()
        {
            string jsonData;
            if (_useMockData)
            {
                jsonData = _localStorage.LoadFile(jsonFileName);
            }
            else
            {
                //TODO: Get data from service
            }

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
