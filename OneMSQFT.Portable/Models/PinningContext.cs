namespace Ratio.Showcase.Shared.Models
{
    public class PinningContext
    {
        public PinningContext(StartupItemType startupItemType = StartupItemType.None)
        {
            StartupItemType = startupItemType;
        }

        public PinningContext(StartupItemType startupItemType, string startupItemId)
        {
            StartupItemType = startupItemType;
            StartupItemId = startupItemId;
        }

        public StartupItemType StartupItemType { get; private set; }
        public string StartupItemId { get; private set; }
    }
}