using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.Connectivity;
using OneMSQFT.Common.Services;

namespace OneMSQFT.UILogic.Services
{
    public class InternetConnectionService : IInternetConnection
    {
        public bool IsConnected()
        {
            return NetworkInformation.GetInternetConnectionProfile() != null;
        }
        
        public event EventHandler InternetConnectionChanged;

        InternetConnectionService()
        {
            NetworkInformation.NetworkStatusChanged += NetworkInformation_NetworkStatusChanged;
        }
 
        private void NetworkInformation_NetworkStatusChanged(object sender)
        {
            var arg = new InternetConnectionChangedEventArgs
                          {IsConnected = (NetworkInformation.GetInternetConnectionProfile() != null)};
 
            if (InternetConnectionChanged != null)
                InternetConnectionChanged(null, arg);
        }
    }
}
