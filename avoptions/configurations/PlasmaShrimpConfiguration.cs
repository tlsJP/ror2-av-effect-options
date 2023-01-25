
using BepInEx.Configuration;
using RoR2;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace com.thejpaproject.avoptions.configurations
{
    class PlasmaShrimpConfiguration : AvConfiguration
    {
        private static EffectComponent s_effectComponent;

        public PlasmaShrimpConfiguration(ConfigFile configFile) :
            base(configFile, Category.VOID_SFX, "Plasma Shrimp Sounds", "Sounds like bowling! \nRequires restart to take effect :(", true)
        { }

        private protected override void HandleEvent(object x, EventArgs args)
        {
            s_effectComponent.enabled = ((ConfigEntry<bool>)x).Value;
            s_effectComponent.soundName = _configEntry.Value ? "Play_item_void_critGlasses" : "";
        }

        private protected override void SetBehavior()
        {
            var missileVoidOrbEffectPrefab = Addressables.LoadAsset<GameObject>("RoR2/DLC1/MissileVoid/MissileVoidOrbEffect.prefab").WaitForCompletion();
            s_effectComponent = missileVoidOrbEffectPrefab.GetComponent<EffectComponent>();
            s_effectComponent.enabled = _configEntry.Value;
            s_effectComponent.soundName = _configEntry.Value ? "Play_item_void_critGlasses" : "";
        }
    }
}
