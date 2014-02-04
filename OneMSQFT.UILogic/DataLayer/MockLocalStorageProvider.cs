using System;
using System.Threading.Tasks;
using OneMSQFT.Common.DataLayer;

namespace OneMSQFT.UILogic.DataLayer
{
    public class MockLocalStorageProvider : ILocalStorageProvider
    {
        public MockLocalStorageProvider()
        {
        }

        public async Task<string> LoadFile(string fileName)
        {
            var folder = Windows.ApplicationModel.Package.Current.InstalledLocation;
            folder = await folder.GetFolderAsync("DataLayer");
            var jsonFile = await folder.GetFileAsync(fileName);

            var results = await Windows.Storage.FileIO.ReadTextAsync(jsonFile);
            return results;
        }

        public async Task SaveFile(string fileName, byte[] data)
        {
            throw new NotImplementedException();
        }

        public string GetLocalFolder()
        {
            throw new NotImplementedException();
        }
    }
}
