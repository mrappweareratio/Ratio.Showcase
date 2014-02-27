using OneMSQFT.Common.DataLayer;

namespace OneMSQFT.UILogic.Tests.Mocks
{
    public class MockApiConfiguration : IApiConfiguration
    {
        public MockApiConfiguration(string apiEndpointUrl, string webSiteUrl = null, string videoSiteUrl = null)
        {
            ApiEndpointUrl = apiEndpointUrl;
            WebSiteUrl = webSiteUrl;
            VideoSiteUrl = videoSiteUrl;
        }

        public string ApiEndpointUrl { get; private set; }

        public string WebSiteUrl { get; private set; }

        public string VideoSiteUrl { get; set; }
    }
}