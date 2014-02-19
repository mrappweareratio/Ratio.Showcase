using Microsoft.Practices.Unity;
using OneMSQFT.UILogic.Interfaces;

namespace OneMSQFT.Windows
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