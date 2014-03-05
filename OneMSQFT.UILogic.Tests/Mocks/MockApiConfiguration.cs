using OneMSQFT.Common.DataLayer;

namespace OneMSQFT.UILogic.Tests.Mocks
{
    public class MockApiConfiguration : IApiConfiguration
    {
        public MockApiConfiguration(string apiEndpointUrl, string webSiteUrl = null, string videoSiteUrl = null, double timeout = 15, int maxRetries = 0)
        {
            ApiEndpointUrl = apiEndpointUrl;
            WebSiteUrl = webSiteUrl;
            VideoSiteUrl = videoSiteUrl;
            TimeoutSeconds = timeout;
            MaxRetries = maxRetries;
        }

        public string ApiEndpointUrl { get; private set; }

        public string WebSiteUrl { get; private set; }

        public string VideoSiteUrl { get; set; }

        public double TimeoutSeconds { get; private set; }

        public int MaxRetries { get; private set; }
    }
}