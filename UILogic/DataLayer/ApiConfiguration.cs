﻿using Ratio.Showcase.Shared.DataLayer;

namespace Ratio.Showcase.UILogic.DataLayer
{
    public class ApiConfiguration : IApiConfiguration
    {
        public string ApiEndpointUrl
        {
            get
            {
#if RELEASE
                return "http://www.1msqft.com/api";
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
                return "http://www.1msqft.com";
#else
                return "http://1msqft-stage.azurewebsites.net";
#endif
            }
        }

        public string VideoSiteUrl { get { return "http://vimeo.com"; } }

        public double TimeoutSeconds
        {
            get
            {
#if RELEASE
                return 20;
#else
                return 5;
#endif
            }
        }

        public int MaxRetries
        {
            get
            {
#if RELEASE
                return 0;
#else
                return 3;
#endif
            }
        }
    }
}