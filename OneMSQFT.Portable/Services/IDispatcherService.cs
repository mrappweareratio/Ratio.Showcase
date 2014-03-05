using System;
using System.Threading.Tasks;

namespace OneMSQFT.Common.Services
{
    public interface IDispatcherService
    {
        Task RunAsync(Func<Task> action);
        Task RunBackgroundAsync(Func<Task> action);
    }
}