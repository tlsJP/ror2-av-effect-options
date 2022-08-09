
using BepInEx.Configuration;
using RoR2;
using System;

using UnityEngine;
using UnityEngine.AddressableAssets;

namespace com.thejpaproject.avoptions.configurations
{
    internal class RunaldsVisualConfiguration : AvConfiguration
    {

        private static EffectComponent effectComponent;
        private static Transform transform;

        public RunaldsVisualConfiguration(ConfigFile configFile) :
            base(configFile, "Item Effects", "Enable Runalds Band", "Enables Runald's Band's ice explosion.\n\nEffective immediately")
        { }

        private protected override void HandleEvent(object x, EventArgs args)
        {
            var enabled = ((ConfigEntry<bool>)x).Value;
            effectComponent.enabled = enabled;

            for (var i = 0; i < transform.childCount; i++)
            {
                var child = transform.GetChild(i);
                child.gameObject.SetActive(enabled);
            }
        }

        private protected override void SetBehavior()
        {
            var prefab = Addressables.LoadAsset<GameObject>("RoR2/Base/ElementalRings/IceRingExplosion.prefab").WaitForCompletion();
            transform = prefab.transform;
            effectComponent = prefab.GetComponent<EffectComponent>();
            effectComponent.enabled = ConfigEntry.Value;
        }

    }
}
