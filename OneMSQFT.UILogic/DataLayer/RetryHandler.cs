using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Ratio.Showcase.Shared;

namespace Ratio.Showcase.UILogic.DataLayer
{
    public class RetryHandler : DelegatingHandler
    {
        public int MaxRetries { get; set; }

        public RetryHandler(HttpMessageHandler innerHandler, int maxRetries)
            : base(innerHandler)
        {
            MaxRetries = maxRetries;
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            HttpResponseMessage response = null;

            for (var i = 0; i < MaxRetries; i++)
            {
                var count = i;
                response = await base.SendAsync(request, cancellationToken).TryCatchAsync(exception =>
                {
                    if (count == MaxRetries - 1)
                        throw exception;
                    return Task.FromResult<object>(null);
                }, () =>
                {
                    throw new TaskCanceledException();
                });

                if (response != null && response.IsSuccessStatusCode)
                {
                    return response;
                }
            }

            return response;
        }
    }
}