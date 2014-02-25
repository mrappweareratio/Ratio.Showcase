using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
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

        async public Task<SiteData> GetSiteData()
        {
            var response = await GetSiteDataResponse();
            var themes = response.Result.Data.Themes.ToList();
            foreach (var evt in response.Result.Data.Events)
            {
                var theme = themes.FirstOrDefault(x => x.Id.Equals(evt.ThemeId));
                if (theme == null) continue;
                evt.Color = theme.Color;
                if (String.IsNullOrEmpty(evt.Color)) continue;
                foreach (var ex in evt.Exhibits)
                {
                    ex.Color = evt.Color;
                }
            }
            return response.Result.Data;
        }

        async public Task<SiteDataResponse> GetSiteDataResponse()
        {
            var httpClient = new HttpClient();
            //httpClient.DefaultRequestHeaders.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));
            httpClient.DefaultRequestHeaders.UserAgent.Add(ProductInfoHeaderValue.Parse("MSAppHost"));
            httpClient.Timeout = TimeSpan.FromSeconds(15);
            var uri = GetApiUri("get_site_data");
            var httpResponseMessage = await httpClient.GetAsync(uri);
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
