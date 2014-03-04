using System;
using OneMSQFT.Common.Services;
using OneMSQFT.UILogic.Services;

namespace OneMSQFT.UILogic.Tests.Mocks
{
    public class MockInternetConnectionService : IInternetConnectionService
    {
        public bool IsConnected { get; set; }
        public ICostGuidance CostGuidance { get; set; }
        public event EventHandler<IInternetConnection> InternetConnectionChanged;

        public MockInternetConnectionService(bool isConnected, MockCostGuidance costGuidance)
        {
            IsConnected = isConnected;
            CostGuidance = costGuidance;
        }

        public MockInternetConnectionService(bool isConnected)
        {
            IsConnected = isConnected;
            CostGuidance = new MockCostGuidance();
        }

        public void RaiseInternetConnectionChanged(IInternetConnection internetConnection)
        {
            var handler = InternetConnectionChanged;
            if (handler != null)
                handler(null, internetConnection);
        }
    }

    public class MockCostGuidance : ICostGuidance
    {
        public NetworkCost Cost { get; set; }
        public string Reason { get; set; }
    }
}
