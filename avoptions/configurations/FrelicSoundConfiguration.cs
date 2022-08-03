
using BepInEx.Configuration;
using System;

namespace com.thejpaproject.avoptions.configurations
{
    internal class FrelicSoundConfiguration : AvConfiguration
    {
        public FrelicSoundConfiguration(ConfigFile configFile) :
            base(configFile, "Item Effects", "Enable Frost Relic Sound", "Enables the sound effects of Frost Relic's on-kill proc.")
        { }

        private protected override void HandleEvent(object x, EventArgs args)
        {
            throw new NotImplementedException();
        }

        private protected override void SetBehavior()
        {
            throw new NotImplementedException();
        }
    }
}
