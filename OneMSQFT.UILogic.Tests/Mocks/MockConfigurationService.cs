using System;
using OneMSQFT.Common.Services;
using OneMSQFT.UILogic.Tests.ViewModels;

namespace OneMSQFT.UILogic.Tests.Mocks
{
    public class MockConfigurationService : IConfigurationService
    {
        public Action<String> SetStartupEventDelegate { get; set; }
        public Action<String> SetStartupExhibitDelegate { get; set; }
        public Action ClearStartupItemDelegate { get; set; }

        public void SetStartupEvent(string id)
        {
            if (SetStartupEventDelegate == null)
                return;
            SetStartupEventDelegate(id);
        }

        public void SetStartupExhibit(string id)
        {
            if (SetStartupExhibitDelegate == null)
                return;
            SetStartupExhibitDelegate(id);
        }

        public void ClearStartupItem()
        {
            if (ClearStartupItemDelegate == null)
                return;
            ClearStartupItemDelegate();
        }

        public StartupItemType StartupItemType { get; private set; }
        public string StartupItemId { get; private set; }
    }
}