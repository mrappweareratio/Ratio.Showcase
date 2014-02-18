using OneMSQFT.Common.DataLayer;

namespace OneMSQFT.UILogic.Tests.Mocks
{
    public class MockApiConfiguration : IApiConfiguration
    {
        public MockApiConfiguration(string apiEndpointUrl)
        {
            ApiEndpointUrl = apiEndpointUrl;
        }

        public string ApiEndpointUrl { get; set; }        
    }
}