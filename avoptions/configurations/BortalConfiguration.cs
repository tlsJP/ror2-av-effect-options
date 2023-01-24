
using BepInEx.Configuration;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Rendering.PostProcessing;

namespace com.thejpaproject.avoptions.configurations
{
  internal class BortalConfiguration : AvConfiguration
  {


    private Light _light;
    private PostProcessVolume _postProcessVolume;

    public BortalConfiguration(ConfigFile configFile) :
        base(configFile, Category.BASE_VFX, "Bortal Glow", "Bortal is too bright")
    { }

    private protected override void HandleEvent(object x, EventArgs args)
    {
      if (_light != null && _postProcessVolume != null)
      {
        var val = ((ConfigEntry<bool>)x).Value;

        _light.enabled = val;
        _postProcessVolume.enabled = val;
      }
    }

    private protected override void SetBehavior()
    {
      // Using a callback here because for some reason, the portal shop prefab won't load on construction of this class.
      On.RoR2.UI.HUD.Awake += callback;
    }

    private void callback(On.RoR2.UI.HUD.orig_Awake orig, RoR2.UI.HUD self)
    {
      var prefab = Addressables.LoadAsset<GameObject>("RoR2/Base/PortalShop/PortalShop.prefab");
      var gameObject = prefab.WaitForCompletion();

      _light = gameObject.GetComponentInChildren<Light>();
      _light.enabled = _configEntry.Value;

      _postProcessVolume = gameObject.GetComponentInChildren<PostProcessVolume>();
      _postProcessVolume.enabled = _configEntry.Value;

      orig(self);
    }

  }
}
