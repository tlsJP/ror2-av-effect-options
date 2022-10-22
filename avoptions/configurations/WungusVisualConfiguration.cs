using BepInEx.Configuration;
using RoR2;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace com.thejpaproject.avoptions.configurations
{
    internal class WungusVisualConfiguration : AvConfiguration
    {
        private TemporaryVisualEffect _mushroomVoidVisual;
        private const String Description =
@"Enables Weeping Fungus' visual particle effects. 
This includes the floating plus symbols, the floating spore particles, and the void star particle effects. 
Does not affect the generic green healing pulsing effect. 

Disable: Effective immediately
Enable: Effective on next level";

        public WungusVisualConfiguration(ConfigFile configFile) :
            base(configFile, Category.VOID_VFX, "Weeping Fungus Visuals", Description)
        { }

        private protected override void HandleEvent(object x, EventArgs args) => _mushroomVoidVisual.enabled = ((ConfigEntry<bool>)x).Value;

        private protected override void SetBehavior()
        {
            var mushroomVoidEffectPrefab = Addressables.LoadAsset<GameObject>("RoR2/DLC1/MushroomVoid/MushroomVoidEffect.prefab").WaitForCompletion();
            _mushroomVoidVisual = mushroomVoidEffectPrefab.GetComponent<TemporaryVisualEffect>();
            _mushroomVoidVisual.enabled = _configEntry.Value;
        }
    }
}
