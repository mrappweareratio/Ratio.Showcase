using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using OneMSQFT.Common.DataLayer;

namespace OneMSQFT.Windows.Win8Libs
{
    class LocalStorageWin8 : ILocalStorageProvider
    {
        public async Task<string> LoadFile(string fileName)
        {
            var storageFolder = ApplicationData.Current.LocalFolder;
            var file = await storageFolder.GetFileAsync(fileName);
            using (var stream = await file.OpenAsync(FileAccessMode.Read))
            using (var inputStream = stream.GetInputStreamAt(0))
            using (var reader = new DataReader(inputStream))
            {
                var data = new byte[stream.Size];
                await reader.LoadAsync((uint)data.Length);
                reader.ReadBytes(data);
                return data;
            }
        }

        public async void SaveFile(string fileName, byte[] data)
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
