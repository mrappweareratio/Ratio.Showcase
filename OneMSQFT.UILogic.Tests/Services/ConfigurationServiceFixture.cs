using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using OneMSQFT.UILogic.Services;
using OneMSQFT.UILogic.Tests.ViewModels;

namespace OneMSQFT.UILogic.Tests.Services
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
    }
}
