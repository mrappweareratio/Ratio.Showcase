using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Ratio.Showcase.Shared.Models;
using Ratio.Showcase.UILogic.Services;

namespace Ratio.Showcase.UILogic.Tests.Services
{
    [TestClass]
    public class ConfigurationServiceFixture
    {
        [TestMethod]
        public void Persists_Event()
        {
            var configuration = new ConfigurationService();
            configuration.SetStartupEvent("0");
            Assert.AreEqual(configuration.StartupItemType, StartupItemType.Event);
            Assert.AreEqual(configuration.StartupItemId, "0");
        }
        [TestMethod]
        public void Persists_Exhibit()
        {
            var configuration = new ConfigurationService();
            configuration.SetStartupExhibit("0");
            Assert.AreEqual(configuration.StartupItemType, StartupItemType.Exhibit);
            Assert.AreEqual(configuration.StartupItemId, "0");
        }
        [TestMethod]
        public void Clear()
        {
            var configuration = new ConfigurationService();
            configuration.SetStartupExhibit("0");
            configuration.ClearStartupItem();
            Assert.AreEqual(configuration.StartupItemType, StartupItemType.None);            
            Assert.IsNull(configuration.StartupItemId);
        }
        [TestMethod]
        public void Persists_Exhibit_LifeCycle()
        {
            var configuration = new ConfigurationService();
            configuration.SetStartupExhibit("0");            
            configuration = new ConfigurationService();
            Assert.AreEqual(configuration.StartupItemType, StartupItemType.Exhibit);
            Assert.AreEqual(configuration.StartupItemId, "0");
        }
    }
}
