using System;
using System.Threading.Tasks;
using OneMSQFT.Common.Services;

namespace OneMSQFT.UILogic.Tests.Mocks
{
    public class MockDispatcherService : IDispatcherService
    {
        public async Task RunAsync(Func<Task> action)
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(priority: Windows.UI.Core.CoreDispatcherPriority.Normal, agileCallback: async () =>
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