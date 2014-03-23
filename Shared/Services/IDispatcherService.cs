using System;
using System.Threading.Tasks;

namespace Ratio.Showcase.Shared.Services
{
    public interface IDispatcherService
    {
        Task RunAsync(Func<Task> action);
        Task RunBackgroundAsync(Func<Task> action);
    }
}