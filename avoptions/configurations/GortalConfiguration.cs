
using BepInEx.Configuration;
using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace com.thejpaproject.avoptions.configurations
{
  internal class GortalConfiguration : AvConfiguration
  {

    private static float timer = 0f;
    private static float delay = .5f;

    public GortalConfiguration(ConfigFile configFile) :
        base(configFile, Category.BASE_VFX, "Gold Shores Portal", "Gortal is too bright.\n\nEffect is relatively immmediate, though re-enabling may require a new level")
    { }

    private protected override void HandleEvent(object x, EventArgs args) { }

    private protected override void SetBehavior()
    {
      // I don't like using the update loop but attempting to access the prefab on startup
      // does not result in the gortal effects' behavior responding as expected
      On.RoR2.UI.HUD.Update += update;
    }

    private void update(On.RoR2.UI.HUD.orig_Update orig, RoR2.UI.HUD self)
    {
      orig(self);

      // Just time boxing the effect logic to attempt to reduce strain on the engine
      timer += Time.deltaTime;
      if (timer > delay)
      {
        var gameObject = GameObject.Find("PortalGoldshores(Clone)");

        if (gameObject != null)
        {
          var light = gameObject.GetComponentInChildren<Light>();
          light.range = _configEntry.Value ? 100 : 20;

          var postProcessVolume = gameObject.GetComponentInChildren<PostProcessVolume>();
          postProcessVolume.enabled = _configEntry.Value;
        }

        timer = timer - delay;

      }

    }

  }
}
