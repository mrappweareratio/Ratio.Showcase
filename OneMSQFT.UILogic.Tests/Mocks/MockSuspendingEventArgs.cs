using Windows.ApplicationModel;

namespace OneMSQFT.UILogic.Tests.Mocks
{
    public class MockSuspendingEventArgs : ISuspendingEventArgs
    {
        public SuspendingOperation SuspendingOperation { get; private set; }
    }
}