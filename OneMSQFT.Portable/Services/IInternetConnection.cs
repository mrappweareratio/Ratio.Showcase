using System;

namespace Ratio.Showcase.Shared.Services
{
    public interface IInternetConnectionService : IInternetConnection
    {
        event EventHandler<InternetConnectionChangedEventArgs> InternetConnectionChanged;
    }

    public interface IInternetConnection
    {
        bool IsConnected { get; }
        ICostGuidance CostGuidance { get; }
    }

    public class InternetConnectionChangedEventArgs : EventArgs, IInternetConnection
    {
        public InternetConnectionChangedEventArgs(IInternetConnection connection)
            : this(connection.IsConnected, connection.CostGuidance)
        {

        }

        public InternetConnectionChangedEventArgs(bool isConnected, ICostGuidance costGuidance)
        {
            IsConnected = isConnected;
            CostGuidance = costGuidance;
        }

        public bool IsConnected { get; set; }
        public ICostGuidance CostGuidance { get; set; }
    }

    public enum NetworkCost { Normal, Conservative, OptIn };

    public interface ICostGuidance
    {
        NetworkCost Cost { get; }
        String Reason { get; }
    }
}
