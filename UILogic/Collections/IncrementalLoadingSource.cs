using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ratio.Showcase.UILogic.Collections
{
    public class IncrementalLoadingSource<T> : IIncrementalLoadingSource<T>
    {        
        private readonly IEnumerable<T> _enumerable;

        public IncrementalLoadingSource(IEnumerable<T> enumerable)
        {
            _enumerable = enumerable;
        }

        public async Task<IEnumerable<T>> GetPagedItems(int pageIndex, int pageSize)
        {
            return await Task.Run(() =>
            {
                var result = _enumerable.Skip(pageIndex * pageSize).Take(pageSize);
                return result;
            });
        }
    }
}