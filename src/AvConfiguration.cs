
using BepInEx.Configuration;
using System;

namespace com.thejpaproject.avoptions
{
    abstract class AvConfiguration
    {
        private protected ConfigEntry<bool> ConfigEntry;
        private static readonly RiskOfOptions RiskOfOptions = new();

        protected AvConfiguration(ConfigEntry<bool> configEntry)
        {
            this.ConfigEntry = configEntry;
            this.SetBehavior();
            ConfigEntry.SettingChanged += HandleEvent;
            RiskOfOptions.AddOption(this.ConfigEntry);
        }

        private protected abstract void HandleEvent(object x, EventArgs args);

        private protected abstract void SetBehavior();

    }


}
