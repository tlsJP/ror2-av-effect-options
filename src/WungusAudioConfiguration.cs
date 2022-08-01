
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

        public WungusAudioConfiguration(ConfigFile configFile) :
            base(configFile, "SOTV Item Effects", "Enable Weeping Fungus Sound", "Enables Weeping Fungus' sound effect. Take effect immediately.", true)
        { }

        private protected override void HandleEvent(object x, EventArgs args) => MushroomVoidAudio.enabled = ((ConfigEntry<bool>)x).Value;

        private protected override void SetBehavior()
        {
            var mushroomVoidEffectPrefab = Addressables.LoadAsset<GameObject>("RoR2/DLC1/MushroomVoid/MushroomVoidEffect.prefab").WaitForCompletion();
            MushroomVoidAudio = mushroomVoidEffectPrefab.GetComponent<LoopSoundPlayer>();
            MushroomVoidAudio.enabled = ConfigEntry.Value;
        }

    }
}
