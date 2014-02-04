using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OneMSQFT.Common.DataLayer
{
    public interface ILocalStorageProvider
    {
        string LoadFile(string fileName);
        void SaveFile(string fileName, byte[] data);
        string GetLocalFolder();
    }
}
