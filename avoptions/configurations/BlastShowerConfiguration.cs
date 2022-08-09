using BepInEx.Configuration;
using RoR2;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace com.thejpaproject.avoptions.configurations
{
    internal class BlastShowerConfiguration : AvConfiguration
    {
        private static Transform s_cleanseTransform;
        private static EffectComponent s_effectComponent;

        private const String Description =
@"Enables Blast Shower's effects.

Toggle: Effective immediately
";

        public BlastShowerConfiguration(ConfigFile configFile) :
            base(configFile, "Item Effects", "Enable Blast Shower", Description)
        { }

        private protected override void HandleEvent(object x, EventArgs args)
        {

            var enabled = ((ConfigEntry<bool>)x).Value;
            _logger.LogDebug($"Cleanse effects enabled={enabled}");

            s_effectComponent.enabled = enabled;
            _logger.LogDebug($"Index set to : {s_effectComponent.effectIndex}");

            for (var i = 0; i < s_cleanseTransform.childCount; i++)
                s_cleanseTransform.GetChild(i).gameObject.SetActive(enabled);

        }

        private protected override void SetBehavior()
        {
            var prefab = Addressables.LoadAsset<GameObject>("RoR2/Base/Cleanse/CleanseEffect.prefab").WaitForCompletion();
            s_cleanseTransform = prefab.transform;
            s_effectComponent = prefab.GetComponent<EffectComponent>();
            s_effectComponent.enabled = _configEntry.Value;
        }
    }
}
