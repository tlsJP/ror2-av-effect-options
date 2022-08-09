using BepInEx.Configuration;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace com.thejpaproject.avoptions.configurations
{
    internal class KjaroVisualConfiguration : AvConfiguration
    {
        private static GameObject FireTornadoSmoke;
        private static GameObject FireTornadoMeshCore;
        private static GameObject FireTornadoMeshWide;
        private static GameObject FireTornadoEmbers;
        private static GameObject FireTornadoLight;
        private static GameObject FireTornadoBurst;

        public KjaroVisualConfiguration(ConfigFile configFile) :
        base(configFile, "Item Effects", "Enable Kjaros Band", "Enables Kjaro's Band's fire tornado.\n\nEffective immediately")
        { }

        private protected override void HandleEvent(object x, EventArgs args) => ToggleEffects(((ConfigEntry<bool>)x).Value);

        private protected override void SetBehavior()
        {
            var FireTornadoGhost = Addressables.LoadAsset<GameObject>("RoR2/Base/ElementalRings/FireTornadoGhost.prefab").WaitForCompletion().transform;
            FireTornadoSmoke = FireTornadoGhost.Find("Smoke").gameObject;
            FireTornadoMeshCore = FireTornadoGhost.Find("TornadoMeshCore").gameObject;
            FireTornadoMeshWide = FireTornadoGhost.Find("TornadoMeshCore, Wide").gameObject;
            FireTornadoEmbers = FireTornadoGhost.Find("Embers").gameObject;
            FireTornadoLight = FireTornadoGhost.Find("Point Light").gameObject;
            FireTornadoBurst = FireTornadoGhost.Find("InitialBurst").gameObject;

            ToggleEffects(ConfigEntry.Value);
        }

        private void ToggleEffects(bool enabled)
        {
            FireTornadoSmoke.SetActive(enabled);
            FireTornadoMeshCore.SetActive(enabled);
            FireTornadoMeshWide.SetActive(enabled);
            FireTornadoEmbers.SetActive(enabled);
            FireTornadoLight.SetActive(enabled);
            FireTornadoBurst.SetActive(enabled);
        }
    }
}
