
using BepInEx.Configuration;
using RoR2;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace com.thejpaproject.avoptions.configurations
{
    class PlasmaShrimpConfiguration : AvConfiguration
    {
        private static EffectComponent MissileVoidOrbEffect;

        public PlasmaShrimpConfiguration(ConfigFile configFile) :
            base(configFile, "SOTV Item Effects", "Enable Plasma Shrimp Sounds", "Sounds like bowling! \nRequires restart to take effect :(", true)
        { }

        private protected override void HandleEvent(object x, EventArgs args) {            
            MissileVoidOrbEffect.enabled = ((ConfigEntry<bool>)x).Value;
            MissileVoidOrbEffect.soundName = ConfigEntry.Value ? "Play_item_void_critGlasses" : "";
        }

        private protected override void SetBehavior()
        {
            var missileVoidOrbEffectPrefab = Addressables.LoadAsset<GameObject>("RoR2/DLC1/MissileVoid/MissileVoidOrbEffect.prefab").WaitForCompletion();
            MissileVoidOrbEffect = missileVoidOrbEffectPrefab.GetComponent<EffectComponent>();
            MissileVoidOrbEffect.enabled = ConfigEntry.Value;
            MissileVoidOrbEffect.soundName = ConfigEntry.Value ? "Play_item_void_critGlasses" : "";
        }
    }
}
