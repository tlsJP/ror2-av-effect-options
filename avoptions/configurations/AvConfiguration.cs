
using BepInEx.Configuration;
using BepInEx.Logging;
using System;

namespace com.thejpaproject.avoptions.configurations
{
    abstract class AvConfiguration
    {
        private protected ConfigEntry<bool> ConfigEntry;
        private static readonly RiskOfOptions RiskOfOptions = RiskOfOptions.Instance;
        private protected ManualLogSource logger;
        private protected String name;

        private protected AvConfiguration(ConfigFile configFile, string category, string key, string description, bool defaultSetting = true)
        {
            name = "c.t.a.c." + GetType().Name;
            logger = BepInEx.Logging.Logger.CreateLogSource(name);
            try
            {
                ConfigEntry = configFile.Bind(category, key, defaultSetting, description);
                SetBehavior();                
                HandleEvent(ConfigEntry, null);
                ConfigEntry.SettingChanged += HandleEvent;
                RiskOfOptions.AddOption(ConfigEntry);
                logger.LogDebug("Configuration completed.");
            }
            catch(Exception ex)
            {
                throw new ConfigurationException(name, ex);
            }
        }
       

        private protected abstract void HandleEvent(object x, EventArgs args);

        private protected abstract void SetBehavior();      

    }


}
