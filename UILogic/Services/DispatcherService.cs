using System;
using System.Threading.Tasks;
using Windows.UI.Core;
using Ratio.Showcase.Shared.Services;

namespace Ratio.Showcase.UILogic.Services
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
