
using BepInEx.Configuration;

using System;

using UnityEngine;
using UnityEngine.AddressableAssets;

namespace com.thejpaproject.avoptions.configurations
{
    internal class IdpVisualConfiguration : AvConfiguration
    {
        private static readonly String DESCRIPTION =
@"Enables the spore, plus sign, and mushroom visual effects from Interstellar Desk Plant's healing ward indicator. 
Does not affect the particle effects of the Desk Plant seed, or the perimeter sphere of the ward.

Disable: Effective immediately
Enable: Effective on next level
";

        private GameObject DeskplantSpores;
        private GameObject DeskplantSymbols;
        private GameObject DeskplantMushrooms;

        public IdpVisualConfiguration(ConfigFile configFile) :
            base(configFile, "Item Effects", "Enable Desk Plant Ward Particles", DESCRIPTION)
        { }

        private protected override void HandleEvent(object x, EventArgs args = null)
        {
            var enabled = ((ConfigEntry<bool>)x).Value;
            DeskplantSpores.SetActive(enabled);
            DeskplantSymbols.SetActive(enabled);
            DeskplantMushrooms.SetActive(enabled);
        }

        private protected override void SetBehavior()
        {
            var DeskplantIndicatorTransform = Addressables.LoadAsset<GameObject>("RoR2/Base/Plant/DeskplantWard.prefab")
                .WaitForCompletion().transform
                .Find("Indicator").gameObject.transform;
            DeskplantSpores = DeskplantIndicatorTransform.Find("Spores").gameObject;
            DeskplantSymbols = DeskplantIndicatorTransform.Find("HealingSymbols").gameObject;
            DeskplantMushrooms = DeskplantIndicatorTransform.Find("MushroomMeshes").gameObject;

        }
    }
}
