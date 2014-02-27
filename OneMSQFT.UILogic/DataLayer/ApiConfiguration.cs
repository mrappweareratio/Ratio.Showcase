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
            get { return "http://1msqft.com"; }
        }

        public string VideoSiteUrl { get { return "http://vimeo.com"; } }
    }
}