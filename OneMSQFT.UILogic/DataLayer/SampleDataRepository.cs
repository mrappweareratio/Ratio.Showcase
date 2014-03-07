using System;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage;
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
        private const string JsonFileName = "site_data.json";        

        public async Task<SiteData> GetSiteData(CancellationToken token)
        {
            var folder = Windows.ApplicationModel.Package.Current.InstalledLocation;           
            folder = await folder.GetFolderAsync("SampleData");
            var jsonFile = await folder.GetFileAsync(JsonFileName);
            var jsonData = await Windows.Storage.FileIO.ReadTextAsync(jsonFile);            
            var dataFromJson = JsonHelper.DeserializeObject<SiteData>(jsonData);
            return dataFromJson;
        }
    }
}
