using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OneMSQFT.Common.Services;
using OneMSQFT.UILogic.Tests.ViewModels;

namespace OneMSQFT.UILogic.Services
{
    public class ConfigurationService : IConfigurationService
    {
        public void SetStartupEvent(string id)
        {
            StartupItemType = StartupItemType.Event;
            StartupItemId = id;
        }

        public void SetStartupExhibit(string id)
        {

            StartupItemType = StartupItemType.Exhibit;
            StartupItemId = id;
        }

        public void ClearStartupItem()
        {           
            StartupItemType = StartupItemType.None;
            StartupItemId = null;
        }

        public StartupItemType StartupItemType { get; private set; }
        public string StartupItemId { get; private set; }
    }
}
