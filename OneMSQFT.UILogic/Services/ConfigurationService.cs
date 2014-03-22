using System;
using Windows.Storage;
using Ratio.Showcase.Shared.Models;
using Ratio.Showcase.Shared.Services;

namespace Ratio.Showcase.UILogic.Services
{
    public class ConfigurationService : IConfigurationService
    {
        readonly ApplicationDataContainer _localSettings = ApplicationData.Current.LocalSettings;
        private readonly ApplicationDataContainer _container;

        public ConfigurationService()
        {
            _container = _localSettings.CreateContainer("Configuration", ApplicationDataCreateDisposition.Always);
        }

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

        public StartupItemType StartupItemType
        {
            get { return (StartupItemType)(Convert.ToInt32(_container.Values["StartupItemType"])); }
            private set { _container.Values["StartupItemType"] = (int)value; }
        }

        public string StartupItemId
        {
            get { return _container.Values["StartupItemId"] as string; }
            private set { _container.Values["StartupItemId"] = value; }
        }
    }
}
