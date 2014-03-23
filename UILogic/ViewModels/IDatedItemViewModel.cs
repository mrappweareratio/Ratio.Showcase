namespace Ratio.Showcase.UILogic.ViewModels
{
    public interface IDatedItemViewModel
    {
        bool? IsInTheFuture { get; }
        bool? IsInThePast { get; }        
    }
}