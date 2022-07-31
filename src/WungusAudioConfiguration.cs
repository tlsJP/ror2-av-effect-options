
using BepInEx.Configuration;
using RoR2;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace com.thejpaproject.avoptions
{
    class WungusAudioConfiguration : AvConfiguration
    {
        private static LoopSoundPlayer MushroomVoidAudio;

        public WungusAudioConfiguration(ConfigEntry<bool> configEntry) : base(configEntry) { }

        private protected override void HandleEvent(object x, EventArgs args) => MushroomVoidAudio.enabled = ((ConfigEntry<bool>)x).Value;

        private protected override void SetBehavior()
        {
            var mushroomVoidEffectPrefab = Addressables.LoadAsset<GameObject>("RoR2/DLC1/MushroomVoid/MushroomVoidEffect.prefab").WaitForCompletion();
            MushroomVoidAudio = mushroomVoidEffectPrefab.GetComponent<LoopSoundPlayer>();
            MushroomVoidAudio.enabled = ConfigEntry.Value;
        }

    }
}
