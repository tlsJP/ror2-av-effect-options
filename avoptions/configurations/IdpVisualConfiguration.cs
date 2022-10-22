using BepInEx.Configuration;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace com.thejpaproject.avoptions.configurations
{
    internal class IdpVisualConfiguration : AvConfiguration
    {
        private const String Description =
@"Enables the spore, plus sign, and mushroom visual effects from Interstellar Desk Plant's healing ward indicator. 
Does not affect the particle effects of the Desk Plant seed, or the perimeter sphere of the ward.

Disable: Effective immediately
Enable: Effective on next level
";

        private GameObject _deskPlantSpores;
        private GameObject _deskPlantSymbols;
        private GameObject _deskPlantMushrooms;

        public IdpVisualConfiguration(ConfigFile configFile) :
            base(configFile, Category.BASE_VFX, "Interstellar Desk Plant Vfx", Description)
        { }

        private protected override void HandleEvent(object x, EventArgs args = null)
        {
            var enabled = ((ConfigEntry<bool>)x).Value;
            _deskPlantSpores.SetActive(enabled);
            _deskPlantSymbols.SetActive(enabled);
            _deskPlantMushrooms.SetActive(enabled);
        }

        private protected override void SetBehavior()
        {
            var DeskplantIndicatorTransform = Addressables.LoadAsset<GameObject>("RoR2/Base/Plant/DeskplantWard.prefab")
                .WaitForCompletion().transform
                .Find("Indicator").gameObject.transform;
            _deskPlantSpores = DeskplantIndicatorTransform.Find("Spores").gameObject;
            _deskPlantSymbols = DeskplantIndicatorTransform.Find("HealingSymbols").gameObject;
            _deskPlantMushrooms = DeskplantIndicatorTransform.Find("MushroomMeshes").gameObject;

        }
    }
}
