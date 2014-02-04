using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OneMSQFT.Common.Models;

namespace OneMSQFT.Common.DataLayer
{
    public interface IDataRepository
    {
        Task<TimelineResult> LoadAllData();
        Task SaveAllData();
    }
}
