using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Windows.Networking.Connectivity;
using Ratio.Showcase.Shared.Services;

namespace Ratio.Showcase.UILogic.Services
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

        public event EventHandler<InternetConnectionChangedEventArgs> InternetConnectionChanged;

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

                var internetConnection = await _task;

                _task = null;

                IsConnected = internetConnection.IsConnected;
                CostGuidance = internetConnection.CostGuidance;

                var handler = InternetConnectionChanged;
                if (handler != null)
                    handler(null, new InternetConnectionChangedEventArgs(internetConnection));
            }
            catch (TaskCanceledException)
            {
                Debug.WriteLine("NetworkStatusChanged TaskCanceledException");
            }
        }

        private static Task<IInternetConnection> GetInternetConnectionAsync(CancellationToken token)
        {
            var args = new InternetConnectionChangedEventArgs(true, new CostGuidance());

            token.ThrowIfCancellationRequested();
            var prof = NetworkInformation.GetInternetConnectionProfile();
            if (prof == null)
            {
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
