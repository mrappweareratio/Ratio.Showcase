using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;

namespace OneMSQFT.UILogic.Tests.Mocks
{
    public class MockLaunchActivatedEventArgs : ILaunchActivatedEventArgs
    {
        public ActivationKind Kind { get; set; }
        public ApplicationExecutionState PreviousExecutionState { get; set; }
        public SplashScreen SplashScreen { get; set; }
        public string Arguments { get; set; }
        public string TileId { get; set; }
    }

    public class MockSuspendingEventArgs : ISuspendingEventArgs
    {
        public SuspendingOperation SuspendingOperation { get; private set; }
    }
}
