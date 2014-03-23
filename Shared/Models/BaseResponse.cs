using System;
using Newtonsoft.Json;

namespace Ratio.Showcase.Shared.Models
{    
    public abstract class BaseResponse<TResult, TResultData> : IBaseResponse<TResult, TResultData, Error>        
        where TResult : IBaseResult<TResultData>
        where TResultData: class
    {
        [JsonProperty("sys_response")]
        public string SystemResponse { get; set; }
        public int Success { get; set; }
        [JsonIgnore]
        public bool IsSuccess { get { return Success == 1; } set { Success = Convert.ToInt32(value); } }
        public Error Error { get; set; }
        [JsonProperty("result")]
        public abstract TResult Result { get; set; }
    }

    public class SiteDataResponse : BaseResponse<SiteDataResult, SiteData>
    {
        public override SiteDataResult Result { get; set; }
    }
}
