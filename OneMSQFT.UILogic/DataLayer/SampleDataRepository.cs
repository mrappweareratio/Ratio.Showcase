using System;
using System.Threading.Tasks;
using OneMSQFT.Common;
using OneMSQFT.Common.DataLayer;
using OneMSQFT.Common.Models;

namespace OneMSQFT.UILogic.DataLayer
{
    /// <summary>
    /// IDataRepository dependent on Package.Current.InstalledLocation/SampleData/*.json
    /// </summary>
    public class SampleDataRepository : IDataRepository
    {
        private const string JsonFileName = "sundance.json";

        public SampleDataRepository()
        {
        }

        public async Task<TimelineResult> LoadAllData()
        {
            var folder = Windows.ApplicationModel.Package.Current.InstalledLocation;
            folder = await folder.GetFolderAsync("SampleData");
            var jsonFile = await folder.GetFileAsync(JsonFileName);
            var jsonData = await Windows.Storage.FileIO.ReadTextAsync(jsonFile);            
            var dataFromJson = JsonHelper.DeserializeObject<TimelineResult>(jsonData);
            return dataFromJson;
        }
    }
}
