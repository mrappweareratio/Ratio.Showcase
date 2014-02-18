namespace OneMSQFT.Common.DataLayer
{
    public interface IApiConfiguration
    {
        /// <summary>
        /// Returns "http://1msqft-stage.azurewebsites.net/api"
        /// portion of http://1msqft-stage.azurewebsites.net/api/get_site_data
        /// </summary>
        string ApiEndpointUrl { get; }
    }
}