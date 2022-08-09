using BepInEx.Configuration;
using System;
using UnityEngine;
using RoR2;
using UnityEngine.AddressableAssets;

namespace com.thejpaproject.avoptions.configurations
{
    internal class BlastShowerConfiguration : AvConfiguration
    {
        private static EffectComponent effect;

        private static readonly String DESCRIPTION =
@"Enables Blast Shower's effects.

Disable: Effective immediately
Enable: Effective on next level
";

        public BlastShowerConfiguration(ConfigFile configFile) :
            base(configFile, "Item Effects", "Enable Blast Shower", DESCRIPTION)
        { }

        private protected override void HandleEvent(object x, EventArgs args) {

            var enabled = ((ConfigEntry<bool>)x).Value;
            logger.LogDebug("Cleanse effects enabled=" + enabled);

            effect.enabled = !enabled;
            logger.LogDebug("Index set to : " + effect.effectIndex);
        }
            
        private protected override void SetBehavior()
        {
            var prefab = Addressables.LoadAsset<GameObject>("RoR2/Base/Cleanse/CleanseEffect.prefab").WaitForCompletion();
            effect = prefab.GetComponent<EffectComponent>();
        }        
    }
}
