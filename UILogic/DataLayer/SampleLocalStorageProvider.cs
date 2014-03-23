using System;
using System.Threading.Tasks;
using Ratio.Showcase.Shared.DataLayer;

namespace Ratio.Showcase.UILogic.DataLayer
{
    public class SampleLocalStorageProvider : ILocalStorageProvider
    {
        public SampleLocalStorageProvider()
        {
        }

        public async Task<string> LoadFile(string fileName)
        {
            var folder = Windows.ApplicationModel.Package.Current.InstalledLocation;
            folder = await folder.GetFolderAsync("SampleData");            
            var jsonFile = await folder.GetFileAsync(fileName);
            var results = await Windows.Storage.FileIO.ReadTextAsync(jsonFile);
            return results;
        }

        public Task SaveFile(string fileName, byte[] data)
        {
            throw new NotImplementedException();
        }

        public string GetLocalFolder()
        {
            throw new NotImplementedException();
        }
    }
}
