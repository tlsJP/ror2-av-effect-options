
using BepInEx.Configuration;
using System;

namespace com.thejpaproject.avoptions.configurations
{
    abstract class AvConfiguration
    {
        private protected ConfigEntry<bool> ConfigEntry;
        private static readonly RiskOfOptions RiskOfOptions = new();

        protected AvConfiguration(ConfigFile configFile, string category, string key, string description, bool defaultSetting = true)
        {
            ConfigEntry = configFile.Bind(category, key, defaultSetting, description);
            this.SetBehavior();
            this.HandleEvent(ConfigEntry,null);
            ConfigEntry.SettingChanged += HandleEvent;
            RiskOfOptions.AddOption(this.ConfigEntry);
        }

        private protected abstract void HandleEvent(object x, EventArgs args);

        private protected abstract void SetBehavior();

    }


}
