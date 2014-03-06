using System;

namespace OneMSQFT.UILogic.ViewModels
{
    public interface IDatedItemViewModel
    {
        bool? IsInTheFuture { get; }
        bool? IsInThePast { get; }        
    }
}