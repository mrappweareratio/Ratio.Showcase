using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml.Data;
using Ratio.Showcase.Shared.Services;

namespace Ratio.Showcase.UILogic.Collections
{
    public class IncrementalLoadingCollection<TItem> : ObservableCollection<TItem>, ISupportIncrementalLoading     
    {
        private readonly IIncrementalLoadingSource<TItem> _source;
        private readonly IDispatcherService _dispatcher;
        private readonly int _itemsPerPage;
        private bool _hasMoreItems;
        private int _currentPage;

        public IncrementalLoadingCollection(IIncrementalLoadingSource<TItem> source, IDispatcherService dispatcher, int itemsPerPage = 20)
        {
            _dispatcher = dispatcher;
            _itemsPerPage = itemsPerPage;
            _source = source;
            _hasMoreItems = true;
        }

        public IncrementalLoadingCollection(IEnumerable<TItem> collection, IDispatcherService dispatcher, int itemsPerPage = 20)
        {
            _dispatcher = dispatcher;
            _itemsPerPage = itemsPerPage;
            _source = new IncrementalLoadingSource<TItem>(collection);
            _hasMoreItems = true;
        }
        
        public bool HasMoreItems
        {
            get { return _hasMoreItems; }
        }

        public IAsyncOperation<LoadMoreItemsResult> LoadMoreItemsAsync(uint count)
        {
            return Task.Run<LoadMoreItemsResult>(
                async () =>
                {
                    uint resultCount = 0;
                    var result = await _source.GetPagedItems(_currentPage++, _itemsPerPage);
                    var items = result as IList<TItem> ?? result.ToList();
                    
                    if (result == null || !items.Any())
                    {
                        _hasMoreItems = false;
                    }
                    else
                    {
                        resultCount = (uint)items.Count;

                        await _dispatcher.RunAsync(
                            () =>
                            {
                                foreach (TItem item in items)
                                    this.Add(item);
                                return Task.FromResult<object>(null);
                            });
                    }

                    return new LoadMoreItemsResult() { Count = resultCount };

                }).AsAsyncOperation<LoadMoreItemsResult>();
        }
    }
}
