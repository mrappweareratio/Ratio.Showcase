using System;
using OneMSQFT.Common.Services;
using OneMSQFT.UILogic.Services;

namespace OneMSQFT.UILogic.Tests.Mocks
{
    public class MockInternetConnectionService : IInternetConnectionService
    {
        public bool IsConnected { get; private set; }
        public ICostGuidance CostGuidance { get; private set; }
        public event EventHandler<IInternetConnection> InternetConnectionChanged;

        public MockInternetConnectionService(bool isConnected, ICostGuidance costGuidance)
        {
            IsConnected = isConnected;
            CostGuidance = costGuidance;
        }

        public MockInternetConnectionService(bool isConnected)
        {
            IsConnected = isConnected;
            CostGuidance = new CostGuidance();
        }

        public void RaiseInternetConnectionChanged(IInternetConnection internetConnection)
        {
            var handler = InternetConnectionChanged;
            if (handler != null)
                handler(null, internetConnection);
        }
    }
}
