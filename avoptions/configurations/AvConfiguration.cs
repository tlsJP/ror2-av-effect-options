
using BepInEx.Configuration;
using BepInEx.Logging;
using System;

namespace com.thejpaproject.avoptions.configurations
{
    abstract class AvConfiguration
    {
        private protected ConfigEntry<bool> ConfigEntry;
        private static readonly RiskOfOptions RiskOfOptions = new();
        private ManualLogSource logger = BepInEx.Logging.Logger.CreateLogSource("AvConfiguration");

        private protected AvConfiguration(ConfigFile configFile, string category, string key, string description, bool defaultSetting = true)
        {
            try
            {
                ConfigEntry = configFile.Bind(category, key, defaultSetting, description);
                this.SetBehavior();
                this.HandleEvent(ConfigEntry, null);
                ConfigEntry.SettingChanged += HandleEvent;
                RiskOfOptions.AddOption(this.ConfigEntry);
                logger.LogDebug(String.Format("Configuration registered for {0}",this.GetType().Name));
            }
            catch
            {
                throw new ConfigurationException(this.GetType().FullName);
            }
        }
       

        private protected abstract void HandleEvent(object x, EventArgs args);

        private protected abstract void SetBehavior();

    }


}
