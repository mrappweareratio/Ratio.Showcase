using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OneMSQFT.Common;
using OneMSQFT.Common.DataLayer;
using OneMSQFT.Common.Models;

namespace OneMSQFT.UILogic.DataLayer
{
    public class ApiDataRepository : IDataRepository
    {
        private readonly IApiConfiguration _configuration;

        public ApiDataRepository(IApiConfiguration configuration)
        {
            _configuration = configuration;
        }

        async public Task<SiteData> GetSiteData(CancellationToken token)
        {            
                var response = await GetSiteDataResponse(token);
                var themes = response.Result.Data.Themes.ToList();
                foreach (var evt in response.Result.Data.Events)
                {
                    var theme = themes.FirstOrDefault(x => x.Id.Equals(evt.ThemeId));
                    if (theme == null) continue;
                    evt.Color = theme.Color;
                    if (String.IsNullOrEmpty(evt.Color)) continue;
                    foreach (var ex in evt.Exhibits)
                    {
                        //persist event color to exhibits
                        ex.Color = evt.Color;
                        //persist event id to exhibits
                        ex.EventId = ex.EventId ?? evt.Id;
                        ex.DateEnd = ex.DateEnd ?? evt.DateEnd;
                        //persist event dates to exhibits
                        ex.DateStart = ex.DateStart ?? evt.DateStart;
                    }
                }
                return response.Result.Data;            
        }

        async public Task<SiteDataResponse> GetSiteDataResponse(CancellationToken token)
        {
            var httpClient = new HttpClient(new RetryHandler(new HttpClientHandler(), _configuration.MaxRetries));
            httpClient.DefaultRequestHeaders.UserAgent.Add(ProductInfoHeaderValue.Parse("MSAppHost"));
            httpClient.Timeout = TimeSpan.FromSeconds(_configuration.TimeoutSeconds);
            var uri = GetApiUri("get_site_data");
            var httpResponseMessage = await httpClient.GetAsync(uri, token);
            var json = await httpResponseMessage.Content.ReadAsStringAsync();            
            var result = JsonHelper.DeserializeObject<SiteDataResponse>(json);
            return result;
        }

        private Uri GetApiUri(string method)
        {
            var url = String.Format("{0}/{1}", _configuration.ApiEndpointUrl, method);
            return new Uri(url, UriKind.Absolute);
        }
    }
}
