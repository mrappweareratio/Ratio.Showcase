using Ratio.Showcase.UILogic.Interfaces;

namespace Ratio.Showcase.Win8
{
    public static class AppLocator
    {
        public static void Register(IOneMsqftApplication application)
        {
            Current = application;
        }

        public static IOneMsqftApplication Current { get; private set; }
    }
}