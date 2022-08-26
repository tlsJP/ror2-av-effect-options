using BepInEx.Configuration;
using System;
using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace com.thejpaproject.avoptions.configurations
{
    internal class ViendAudioConfiguration : AvConfiguration
    {

        private SprintEffectController _sprintEffectController;

        public ViendAudioConfiguration(ConfigFile configFile) : 
            base(configFile, "Character Effects", "Enable Viend Sprint/Jump SFX", "Enable run/jump sound effects\nRequires new level/run to take effect", true)
        {}

        private protected override void HandleEvent(object x, EventArgs args)
        {
            var enabled = ((ConfigEntry<bool>)x).Value;

            _sprintEffectController.enabled = enabled;
        }

        private protected override void SetBehavior()
        {
            var voidManPrefab = Addressables.LoadAsset<GameObject>("RoR2/DLC1/VoidSurvivor/VoidSurvivorBody.prefab").WaitForCompletion();
            _sprintEffectController = voidManPrefab.GetComponent<SprintEffectController>();

            
        }
    }
}
