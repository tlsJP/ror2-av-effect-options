using BepInEx.Configuration;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace com.thejpaproject.avoptions.configurations
{
    internal class KjaroVisualConfiguration : AvConfiguration
    {
        private static GameObject s_smoke;
        private static GameObject s_tornadoMeshCore;
        private static GameObject s_tornadoMeshCoreWide;
        private static GameObject s_embers;
        private static GameObject s_pointLight;
        private static GameObject s_initialBurst;

        public KjaroVisualConfiguration(ConfigFile configFile) :
        base(configFile, "Item Effects", "Enable Kjaros Band", "Enables Kjaro's Band's fire tornado.\n\nEffective immediately")
        { }

        private protected override void HandleEvent(object x, EventArgs args) => ToggleEffects(((ConfigEntry<bool>)x).Value);

        private protected override void SetBehavior()
        {
            var FireTornadoGhost = Addressables.LoadAsset<GameObject>("RoR2/Base/ElementalRings/FireTornadoGhost.prefab").WaitForCompletion().transform;
            s_smoke = FireTornadoGhost.Find("Smoke").gameObject;
            s_tornadoMeshCore = FireTornadoGhost.Find("TornadoMeshCore").gameObject;
            s_tornadoMeshCoreWide = FireTornadoGhost.Find("TornadoMeshCore, Wide").gameObject;
            s_embers = FireTornadoGhost.Find("Embers").gameObject;
            s_pointLight = FireTornadoGhost.Find("Point Light").gameObject;
            s_initialBurst = FireTornadoGhost.Find("InitialBurst").gameObject;

            ToggleEffects(_configEntry.Value);
        }

        private void ToggleEffects(bool enabled)
        {
            s_smoke.SetActive(enabled);
            s_tornadoMeshCore.SetActive(enabled);
            s_tornadoMeshCoreWide.SetActive(enabled);
            s_embers.SetActive(enabled);
            s_pointLight.SetActive(enabled);
            s_initialBurst.SetActive(enabled);
        }
    }
}
