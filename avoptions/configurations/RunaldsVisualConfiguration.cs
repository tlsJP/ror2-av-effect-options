
using BepInEx.Configuration;

using System;

using UnityEngine;
using UnityEngine.AddressableAssets;

namespace com.thejpaproject.avoptions.configurations
{
    internal class RunaldsVisualConfiguration : AvConfiguration
    {
        private static GameObject IceRingExplosionPrefab;
        private static DestroyOnUpdate IceRingExplosionDestructor;

        public RunaldsVisualConfiguration(ConfigFile configFile) :
            base(configFile, "Item Effects", "Enable Runalds Band", "Enables Runald's Band's ice explosion.\n\nEffective immediately")
        { }

        private protected override void HandleEvent(object x, EventArgs args)
        {
            
            var transform = IceRingExplosionPrefab.transform;
            var childCount = transform.childCount;
            var enabled = ((ConfigEntry<bool>)x).Value;
            if (enabled && IceRingExplosionDestructor is not null)
            {
                logger.LogDebug(String.Format("runaldsA : {0} - {1}", enabled, IceRingExplosionDestructor));
                UnityEngine.Object.Destroy(IceRingExplosionDestructor);
                IceRingExplosionDestructor = null;
            }
            else
            {
                logger.LogDebug(String.Format("runaldsB : {0} - {1}", enabled, IceRingExplosionDestructor));
                IceRingExplosionDestructor = IceRingExplosionPrefab.AddComponent<DestroyOnUpdate>();
            }

            for (var i = 0; i < childCount; i++)
            {
                var child = transform.GetChild(i);
                logger.LogDebug(String.Format("setting state for child : {0}", child));
                child.gameObject.SetActive(enabled);
            }
        }

        private protected override void SetBehavior()
        {
            IceRingExplosionPrefab = Addressables.LoadAsset<GameObject>("RoR2/Base/ElementalRings/IceRingExplosion.prefab").WaitForCompletion();
            if (!ConfigEntry.Value)
                IceRingExplosionDestructor = IceRingExplosionPrefab.AddComponent<DestroyOnUpdate>();
            var IceRingExplosionTransform = IceRingExplosionPrefab.transform;
            for (var i = 0; i < IceRingExplosionTransform.childCount; i++)
                IceRingExplosionTransform.GetChild(i).gameObject.SetActive(ConfigEntry.Value);
        }
    }
}
