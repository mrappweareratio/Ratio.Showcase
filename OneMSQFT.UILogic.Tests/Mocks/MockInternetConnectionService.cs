using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OneMSQFT.Common.Services;

namespace OneMSQFT.UILogic.Tests.Mocks
{
    public class MockInternetConnectionService : IInternetConnection
    {
        public Func<bool> IsConnectedDelegate { get; set; }

        public bool IsConnected()
        {
            if (IsConnectedDelegate != null)
                return IsConnectedDelegate();
            return false;
        }
    }
}
