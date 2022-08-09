using BepInEx.Configuration;
using RoR2;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace com.thejpaproject.avoptions.configurations
{
    internal class BlastShowerConfiguration : AvConfiguration
    {
        private static Transform CleanseTransform;
        private static EffectComponent effect;

        private static readonly String DESCRIPTION =
@"Enables Blast Shower's effects.

Toggle: Effective immediately
";

        public BlastShowerConfiguration(ConfigFile configFile) :
            base(configFile, "Item Effects", "Enable Blast Shower", DESCRIPTION)
        { }

        private protected override void HandleEvent(object x, EventArgs args)
        {

            var enabled = ((ConfigEntry<bool>)x).Value;
            logger.LogDebug("Cleanse effects enabled=" + enabled);

            effect.enabled = enabled;
            logger.LogDebug("Index set to : " + effect.effectIndex);

            for (var i = 0; i < CleanseTransform.childCount; i++)
                CleanseTransform.GetChild(i).gameObject.SetActive(enabled);

        }

        private protected override void SetBehavior()
        {
            var prefab = Addressables.LoadAsset<GameObject>("RoR2/Base/Cleanse/CleanseEffect.prefab").WaitForCompletion();
            CleanseTransform = prefab.transform;
            effect = prefab.GetComponent<EffectComponent>();
            effect.enabled = ConfigEntry.Value;
        }
    }
}
