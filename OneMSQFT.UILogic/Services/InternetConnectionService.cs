using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OneMSQFT.Common.Services;

namespace OneMSQFT.UILogic.Services
{
    public class InternetConnectionService : IInternetConnection
    {
        public bool IsConnected()
        {
            return true;
            //return IsConnectedInternal();
        }
    }
}
