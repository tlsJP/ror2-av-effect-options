using BepInEx.Configuration;
using System;
using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace com.thejpaproject.avoptions.configurations
{
    internal class BlastShowerConfiguration : AvConfiguration
    {
        private static Transform CleanseTransform;

        public BlastShowerConfiguration(ConfigFile configFile) :
            base(configFile, "Item Effects", "Enable Blast Shower", "Enables Blast Shower's effects.", true)
        { }

        private protected override void HandleEvent(object x, EventArgs args)
        {
            var y = ConfigEntry.Value;
            for (var i = 0; i < CleanseTransform.childCount; i++)
                CleanseTransform.GetChild(i).gameObject.SetActive(y);
        }

        private protected override void SetBehavior()
        {

            var prefab = Addressables.LoadAsset<GameObject>("RoR2/Base/Cleanse/CleanseEffect.prefab").WaitForCompletion();
            CleanseTransform = prefab.transform;

            if (!ConfigEntry.Value)
            {
                for (var i = 0; i < CleanseTransform.childCount; i++)
                    CleanseTransform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
}
