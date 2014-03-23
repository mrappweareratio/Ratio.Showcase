using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Ratio.Showcase.Shared.Services;
using Ratio.Showcase.UILogic.Services;

namespace Ratio.Showcase.UILogic.Tests.Services
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
