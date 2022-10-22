
using BepInEx.Configuration;
using RoR2;
using System;

using UnityEngine;
using UnityEngine.AddressableAssets;

namespace com.thejpaproject.avoptions.configurations
{
    internal class RunaldsVisualConfiguration : AvConfiguration
    {

        private static EffectComponent s_effectComponent;
        private static Transform s_transform;

        public RunaldsVisualConfiguration(ConfigFile configFile) :
            base(configFile, Category.BASE_VFX, "Enable Runalds Band", "Enables Runald's Band's ice explosion.\n\nEffective immediately")
        { }

        private protected override void HandleEvent(object x, EventArgs args)
        {
            var enabled = ((ConfigEntry<bool>)x).Value;
            s_effectComponent.enabled = enabled;

            for (var i = 0; i < s_transform.childCount; i++)
            {
                var child = s_transform.GetChild(i);
                child.gameObject.SetActive(enabled);
            }
        }

        private protected override void SetBehavior()
        {
            var prefab = Addressables.LoadAsset<GameObject>("RoR2/Base/ElementalRings/IceRingExplosion.prefab").WaitForCompletion();
            s_transform = prefab.transform;
            s_effectComponent = prefab.GetComponent<EffectComponent>();
            s_effectComponent.enabled = _configEntry.Value;
        }

    }
}
