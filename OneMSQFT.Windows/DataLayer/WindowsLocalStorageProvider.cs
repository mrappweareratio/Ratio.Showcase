using System;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using OneMSQFT.Common.DataLayer;

namespace OneMSQFT.WindowsStore.DataLayer
{
    class WindowsLocalStorageProvider : ILocalStorageProvider
    {
        public async Task<string> LoadFile(string fileName)
        {
            var storageFolder = ApplicationData.Current.LocalFolder;
            var file = await storageFolder.CreateFileAsync(fileName, CreationCollisionOption.OpenIfExists);

            string fileContents = string.Empty;
            if (file != null)
            {
                fileContents = await FileIO.ReadTextAsync(file);
            }

            return fileContents;
        }

        public async Task SaveFile(string fileName, byte[] data)
        {
            var storageFolder = ApplicationData.Current.LocalFolder;
            var file = await storageFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);

            using (var stream = await file.OpenAsync(FileAccessMode.ReadWrite))
            using (var writer = new DataWriter(stream))
            {
                writer.WriteBytes(data);
                await writer.StoreAsync();
            }
        }

        public string GetLocalFolder()
        {
            return ApplicationData.Current.LocalFolder.Path;
        }
    }
}
