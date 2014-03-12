using System.Collections.Generic;
using System.Threading.Tasks;

namespace OneMSQFT.UILogic.Collections
{
    public interface IIncrementalLoadingSource<T>
    {
        Task<IEnumerable<T>> GetPagedItems(int pageIndex, int pageSize);
    }
}