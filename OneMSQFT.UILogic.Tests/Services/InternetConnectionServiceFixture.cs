using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.Connectivity;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using OneMSQFT.Common.Services;
using OneMSQFT.UILogic.Services;

namespace OneMSQFT.UILogic.Tests.Services
{
    [TestClass]
    public class InternetConnectionServiceFixture
    {
        [TestMethod]
        public void IsConnected_Default()
        {
            var internet = new InternetConnectionService();
            Assert.IsTrue(internet.IsConnected, "IsConnected");
            Assert.AreEqual(NetworkCost.Normal, internet.CostGuidance.Cost, "Normal Cost");
        }
    }
}
