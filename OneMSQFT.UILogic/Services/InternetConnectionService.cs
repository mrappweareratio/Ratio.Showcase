using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Networking.Connectivity;
using OneMSQFT.Common.Services;

namespace OneMSQFT.UILogic.Services
{
    public class InternetConnectionService : IInternetConnectionService
    {
        public InternetConnectionService()
        {
            IsConnected = true;
            CostGuidance = new CostGuidance();

            NetworkInformation.NetworkStatusChanged += NetworkInformationNetworkStatusChanged;
            Task.Run(() => this.NetworkInformationNetworkStatusChanged(null));
        }

        public bool IsConnected { get; private set; }

        public ICostGuidance CostGuidance { get; private set; }

        public event EventHandler<IInternetConnection> InternetConnectionChanged;

        private CancellationTokenSource _cancellation;
        private Task<IInternetConnection> _task;

        async private void NetworkInformationNetworkStatusChanged(object sender)
        {
            if (_cancellation == null)
                _cancellation = new CancellationTokenSource();

            if (_task != null && !_cancellation.IsCancellationRequested)
            {
                _cancellation.Cancel();
                _cancellation = new CancellationTokenSource();
            }

            try
            {
                var token = _cancellation.Token;
                _task = Task.Run(async () => await GetInternetConnectionAsync(token).ConfigureAwait(false), token);
                
                var args = await _task;

                _task = null;

                IsConnected = args.IsConnected;
                CostGuidance = args.CostGuidance;

                var handler = InternetConnectionChanged;
                if (handler != null)
                    handler(null, args);
            }
            catch (TaskCanceledException)
            {
                Debug.WriteLine("NetworkStatusChanged TaskCanceledException");
            }            
        }

        private static Task<IInternetConnection> GetInternetConnectionAsync(CancellationToken token)
        {
            var args = new InternetConnectionChangedEventArgs();

            token.ThrowIfCancellationRequested();
            var prof = NetworkInformation.GetInternetConnectionProfile();
            if (prof == null)
            {
                args.IsConnected = true;
                args.CostGuidance = new CostGuidance();
                return Task.FromResult<IInternetConnection>(args);
            }

            token.ThrowIfCancellationRequested();
            var connectivity = prof.GetNetworkConnectivityLevel();
            args.IsConnected = connectivity == NetworkConnectivityLevel.InternetAccess;

            token.ThrowIfCancellationRequested();
            args.CostGuidance = new CostGuidance(prof.GetConnectionCost());

            return Task.FromResult<IInternetConnection>(args);
        }
    }
}
