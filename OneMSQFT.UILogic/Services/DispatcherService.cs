using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using OneMSQFT.Common.Services;

namespace OneMSQFT.UILogic.Services
{
    public class DispatcherService : IDispatcherService
    {
        private readonly CoreDispatcher _dispatcher;

        public DispatcherService(CoreDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        public async Task RunAsync(Func<Task> action)
        {
            await _dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
            {
                await action();
            });
        }

        public async Task RunBackgroundAsync(Func<Task> action)
        {
            await Task.Run(action);
        }        
    }
}
