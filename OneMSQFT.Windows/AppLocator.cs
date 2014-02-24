using OneMSQFT.UILogic.Interfaces;

namespace OneMSQFT.WindowsStore
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