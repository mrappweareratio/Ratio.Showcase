using System;

namespace OneMSQFT.Common.DataLayer
{
    public interface IApiConfiguration
    {
        /// <summary>
        /// Returns "http://1msqft-stage.azurewebsites.net/api"
        /// portion of http://1msqft-stage.azurewebsites.net/api/get_site_data
        /// </summary>
        string ApiEndpointUrl { get; }
        
        /// <summary>
        /// Return "http://1msqft.com"
        /// </summary>
        string WebSiteUrl { get; }

        /// <summary>
        /// Returns the endpoint for addressable video ids
        /// Example "http://vimeo.com"
        /// </summary>
        string VideoSiteUrl { get; }
    }
}