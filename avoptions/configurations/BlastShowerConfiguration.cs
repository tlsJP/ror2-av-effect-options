using BepInEx.Configuration;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace com.thejpaproject.avoptions.configurations
{
    internal class BlastShowerConfiguration : AvConfiguration
    {
        private static Transform CleanseTransform;

        private static readonly String DESCRIPTION =
@"Enables Blast Shower's effects.

Disable: Effective immediately
Enable: Effective on next level
";

        public BlastShowerConfiguration(ConfigFile configFile) :
            base(configFile, "Item Effects", "Enable Blast Shower", DESCRIPTION)
        { }

        private protected override void HandleEvent(object x, EventArgs args) => SetChildrenStatus(((ConfigEntry<bool>)x).Value);

        private protected override void SetBehavior()
        {

            var prefab = Addressables.LoadAsset<GameObject>("RoR2/Base/Cleanse/CleanseEffect.prefab").WaitForCompletion();
            CleanseTransform = prefab.transform;
            SetChildrenStatus(ConfigEntry.Value);
        }

        private void SetChildrenStatus(bool active)
        {
            for (var i = 0; i < CleanseTransform.childCount; i++)
                CleanseTransform.GetChild(i).gameObject.SetActive(active);
        }
    }
}
