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
        public InternetConnectionService()
        {
            NetworkInformation.NetworkStatusChanged += NetworkInformation_NetworkStatusChanged;
        }

        public bool IsConnected()
        {
            var prof = NetworkInformation.GetInternetConnectionProfile();
            bool connected = false;

            if (prof != null)
            {
                connected = prof.GetNetworkConnectivityLevel() == NetworkConnectivityLevel.InternetAccess;
            }
            return connected;
        }
        
        public event EventHandler InternetConnectionChanged;

        private void NetworkInformation_NetworkStatusChanged(object sender)
        {
            var arg = new InternetConnectionChangedEventArgs
                          {IsConnected = this.IsConnected()};
 
            if (InternetConnectionChanged != null)
                InternetConnectionChanged(null, arg);
        }
    }
}
