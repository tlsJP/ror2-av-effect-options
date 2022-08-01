using BepInEx.Configuration;
using System;
using BepInEx;
using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace com.thejpaproject.avoptions
{
    internal class WungusVisualConfiguration : AvConfiguration
    {
        private static TemporaryVisualEffect MushroomVoidVisual;

        public WungusVisualConfiguration(ConfigFile configFile) :
            base(configFile,  "SOTV Item Effects", "Enable Weeping Fungus Visuals", "Enables Weeping Fungus' visual particle effects. This includes the floating plus symbols, the floating spore particles, and the void star particle effects. Does not affect the generic green healing pulsing effect. Note: re-enabling may not take effect until next stage.",true)
        { }

        private protected override void HandleEvent(object x, EventArgs args) => MushroomVoidVisual.enabled = ((ConfigEntry<bool>)x).Value;

        private protected override void SetBehavior()
        {
            var mushroomVoidEffectPrefab = Addressables.LoadAsset<GameObject>("RoR2/DLC1/MushroomVoid/MushroomVoidEffect.prefab").WaitForCompletion();
            MushroomVoidVisual = mushroomVoidEffectPrefab.GetComponent<TemporaryVisualEffect>();
            MushroomVoidVisual.enabled = ConfigEntry.Value;
        }
    }
}
