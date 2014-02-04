using System.Dynamic;

namespace OneMSQFT.Common.Models
{
    public class BaseResponse<TResult> : IBaseResponse<TResult, Error> 
        where TResult : class        
    {
        public string SystemResponse { get; set; }
        public bool Success { get; set; }
        public Error Error { get; set; }
        public TResult Result { get; set; }
    }
}
