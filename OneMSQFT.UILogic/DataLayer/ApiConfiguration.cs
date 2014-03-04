using OneMSQFT.Common.DataLayer;

namespace OneMSQFT.UILogic.DataLayer
{
    public class ApiConfiguration : IApiConfiguration
    {
        public string ApiEndpointUrl
        {
            get
            {
#if RELEASE
                return "http://1msqft.azurewebsites.net/api";
#else
                return "http://1msqft-stage.azurewebsites.net/api";
#endif
            }
        }

        public string WebSiteUrl
        {
            get
            {
#if RELEASE
                return "http://1msqft.com";
#else
                return "http://1msqft-stage.azurewebsites.net";
#endif
            }
        }

        public string VideoSiteUrl { get { return "http://vimeo.com"; } }
    }
}