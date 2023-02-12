
using BepInEx.Configuration;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Rendering.PostProcessing;

namespace com.thejpaproject.avoptions.configurations
{
    internal class BortalVfxConfiguration : AvConfiguration
    {

        private Light _light;
        private PostProcessVolume _postProcessVolume;

        public BortalVfxConfiguration(ConfigFile configFile) :
            base(configFile, Category.BASE_VFX, "Blue Portal Glow", "Bortal is too bright.\n\nRequires next stage to take effect")
        { }

        private protected override void HandleEvent(object x, EventArgs args)
        {
            if (_light != null && _postProcessVolume != null)
            {
                var enabled = ((ConfigEntry<bool>)x).Value;
                _light.enabled = enabled;
                _postProcessVolume.enabled = enabled;
            }
        }

        private protected override void SetBehavior()
        {
            // Using a callback here because for some reason, the portal shop prefab won't load on construction of this class.
            On.RoR2.UI.HUD.Awake += callback;
            On.RoR2.Stage.Start += stageStart;
        }

        /*
          This exists solely to capture the bortal inside the lunar shop, because the bortal in there doesn't seem to respond
          to the callback function below
        */
        private void stageStart(On.RoR2.Stage.orig_Start orig, RoR2.Stage self)
        {
            orig(self);
            var bortal = GameObject.Find("PortalShop");
            if (bortal == null)
            {
                return;
            }

            var light = bortal.GetComponentInChildren<Light>();
            setLightRange(light);
        }

        /*
          The biggest offender, especially on rallypoint delta is the PostProcessVolume.
          Simply disabling this effect is arguably enough, but lowering the light's range
          helps as well
        */
        private void callback(On.RoR2.UI.HUD.orig_Awake orig, RoR2.UI.HUD self)
        {
            var prefab = Addressables.LoadAsset<GameObject>("RoR2/Base/PortalShop/PortalShop.prefab");
            var gameObject = prefab.WaitForCompletion();

            _light = gameObject.GetComponentInChildren<Light>();
            setLightRange(_light);

            _postProcessVolume = gameObject.GetComponentInChildren<PostProcessVolume>();
            _postProcessVolume.enabled = _configEntry.Value;

            orig(self);
        }

        private void setLightRange(Light light) => light.range = _configEntry.Value ? 100 : 20;

    }
}
