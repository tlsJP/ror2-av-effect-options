

using BepInEx.Configuration;
using RoR2.Projectile;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace com.thejpaproject.avoptions.configurations
{
    internal class FireworkTailConfiguration : AvConfiguration
    {

        private static TrailRenderer s_trailRenderer;

        public FireworkTailConfiguration(ConfigFile configFile) :
            base(configFile, Category.BASE_VFX, "Fireworks Tails", "Fireworks normally have tails")
        { }

        private protected override void HandleEvent(object x, EventArgs args)
        {
            var enabled = ((ConfigEntry<bool>)x).Value;

            s_trailRenderer.enabled = enabled;
        }

        private protected override void SetBehavior()
        {

            var fireworkPrefab = Addressables.LoadAsset<GameObject>("RoR2/Base/Firework/FireworkGhost.prefab").WaitForCompletion();
            var controller = fireworkPrefab.GetComponent<ProjectileGhostController>();
            s_trailRenderer = controller.GetComponent<TrailRenderer>();

        }
    }
}
