
using BepInEx.Configuration;
using RoR2;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace com.thejpaproject.avoptions.configurations
{
    class WungusAudioConfiguration : AvConfiguration
    {
        private LoopSoundPlayer MushroomVoidAudio;

        private const String Description =
@"Enables Weeping Fungus sound effects. 

Disable: Effective immediately
Enable: Effective on next level";

        public WungusAudioConfiguration(ConfigFile configFile) :
            base(configFile, "SOTV Item Effects", "Enable Weeping Fungus Sound", Description)
        { }

        private protected override void HandleEvent(object x, EventArgs args) => MushroomVoidAudio.enabled = ((ConfigEntry<bool>)x).Value;

        private protected override void SetBehavior()
        {
            var mushroomVoidEffectPrefab = Addressables.LoadAsset<GameObject>("RoR2/DLC1/MushroomVoid/MushroomVoidEffect.prefab").WaitForCompletion();
            MushroomVoidAudio = mushroomVoidEffectPrefab.GetComponent<LoopSoundPlayer>();
            MushroomVoidAudio.enabled = _configEntry.Value;
        }

    }
}
