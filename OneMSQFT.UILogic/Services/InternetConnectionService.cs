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
        private bool _statusInitialized = false;
        private bool _isConnected = false;
        
        public InternetConnectionService()
        {
            NetworkInformation.NetworkStatusChanged += NetworkInformation_NetworkStatusChanged;
        }

        public bool IsConnected()
        {
            if (_statusInitialized) 
                return this._isConnected;

            var prof = NetworkInformation.GetInternetConnectionProfile();
            if (prof != null)
            {
                this._isConnected = prof.GetNetworkConnectivityLevel() == NetworkConnectivityLevel.InternetAccess;
                _statusInitialized = true;
            }

            return this._isConnected;
        }
        
        public event EventHandler InternetConnectionChanged;

        private void NetworkInformation_NetworkStatusChanged(object sender)
        {
            _statusInitialized = false;
            var arg = new InternetConnectionChangedEventArgs
                          {IsConnected = this.IsConnected()};
 
            if (InternetConnectionChanged != null)
                InternetConnectionChanged(null, arg);
        }
    }
}
