using Windows.ApplicationModel;

namespace Ratio.Showcase.UILogic.Tests.Mocks
{
    public class MockSuspendingEventArgs : ISuspendingEventArgs
    {
        public SuspendingOperation SuspendingOperation { get; private set; }
    }
}