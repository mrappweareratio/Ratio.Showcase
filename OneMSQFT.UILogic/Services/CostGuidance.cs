using System;
using Windows.Networking.Connectivity;
using Ratio.Showcase.Shared.Services;

namespace Ratio.Showcase.UILogic.Services
{
    public class CostGuidance : ICostGuidance
    {
        public CostGuidance(ConnectionCost connectionCost)
        {
            Cost = NetworkCost.Normal;
            Init(connectionCost);
        }

        public CostGuidance()
        {
            Cost = NetworkCost.Normal;
        }

        public NetworkCost Cost { get; set; }
        public String Reason { get; set; }

        public void Init(ConnectionCost connectionCost)
        {
            if (connectionCost == null) return;
            if (connectionCost.Roaming || connectionCost.OverDataLimit)
            {
                Cost = NetworkCost.OptIn;
                Reason = connectionCost.Roaming
                    ? "Connection is roaming; using the connection may result in additional charge."
                    : "Connection has exceeded the usage cap limit.";
            }
            else if (connectionCost.NetworkCostType == NetworkCostType.Fixed
                     || connectionCost.NetworkCostType == NetworkCostType.Variable)
            {
                Cost = NetworkCost.Conservative;
                Reason = connectionCost.NetworkCostType == NetworkCostType.Fixed
                    ? "Connection has limited allowed usage."
                    : "Connection is charged based on usage. ";
            }
            else
            {
                Cost = NetworkCost.Normal;
                Reason = connectionCost.NetworkCostType == NetworkCostType.Unknown
                    ? "Connection is unknown"
                    : "Connection cost is unrestricted";
            }
        }
    }
}