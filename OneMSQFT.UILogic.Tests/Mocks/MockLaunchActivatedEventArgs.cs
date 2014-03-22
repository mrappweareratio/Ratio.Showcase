using Windows.ApplicationModel.Activation;

namespace Ratio.Showcase.UILogic.Tests.Mocks
{
    public class MockLaunchActivatedEventArgs : ILaunchActivatedEventArgs
    {
        public ActivationKind Kind { get; set; }
        public ApplicationExecutionState PreviousExecutionState { get; set; }
        public SplashScreen SplashScreen { get; set; }
        public string Arguments { get; set; }
        public string TileId { get; set; }
    }
}
