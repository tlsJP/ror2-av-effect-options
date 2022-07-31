
using BepInEx.Configuration;
using RoR2;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace com.thejpaproject.avoptions
{
    class PlasmaShrimpConfiguration : AvConfiguration
    {

        private static EffectComponent MissileVoidOrbEffect;

        public PlasmaShrimpConfiguration(ConfigEntry<bool> configEntry) : base(configEntry) { }

        private protected override void HandleEvent(object x, EventArgs args) => MissileVoidOrbEffect.enabled = ((ConfigEntry<bool>)x).Value;

        private protected override void SetBehavior()
        {
            var missileVoidOrbEffectPrefab = Addressables.LoadAsset<GameObject>("RoR2/DLC1/MissileVoid/MissileVoidOrbEffect.prefab").WaitForCompletion();
            MissileVoidOrbEffect = missileVoidOrbEffectPrefab.GetComponent<EffectComponent>();
            MissileVoidOrbEffect.enabled = ConfigEntry.Value;
            MissileVoidOrbEffect.soundName = ConfigEntry.Value ? "Play_item_void_critGlasses" : "";
        }

        
    }
}
