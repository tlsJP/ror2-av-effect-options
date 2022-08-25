using BepInEx.Configuration;
using System;

namespace com.thejpaproject.avoptions.configurations
{
    internal class ViendAudioConfiguration : AvConfiguration
    {
        public ViendAudioConfiguration(ConfigFile configFile, string category, string key, string description, bool defaultSetting = true) : 
            base(configFile, "SOTV Item Effects", "Enable Void Fiend SFX", "Enable run/jump sound effects", defaultSetting)
        {}

        private protected override void HandleEvent(object x, EventArgs args)
        {
            // TODO
        }

        private protected override void SetBehavior()
        {
            // TODO
        }
    }
}
