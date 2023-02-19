
using BepInEx.Configuration;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;


namespace com.thejpaproject.avoptions.configurations
{
    internal class BortalCenterConfiguration : AvConfiguration
    {


        private Mesh _mesh;

        public BortalCenterConfiguration(ConfigFile configFile) :
            base(configFile, Category.BASE_VFX, "Blue Portal Center", "Toggle the center of the blue portal")
        { }

        private protected override void HandleEvent(object x, EventArgs args)
        {

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

            var portalCenterQuad = findPortalCenter(bortal);
            togglePortalCenter(portalCenterQuad);
        }

        private GameObject findPortalCenter(GameObject gameObject)
        {
            _logger.LogInfo("Looking for bortal center...");


            foreach (GameObject go in gameObject.GetComponents<GameObject>())
            {
                _logger.LogDebug($"Found child : {go.name}");
            }

            foreach (GameObject go in gameObject.GetComponentsInChildren<GameObject>())
            {
                _logger.LogDebug($"Found child : {go.name}");
            }



            var quad = gameObject.GetComponentsInChildren<GameObject>().FirstOrDefault(go => go.name == "Quad");
            _logger.LogInfo($"Bortal center ? {quad}");
            return quad;
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


            var portalCenterQuad = findPortalCenter(gameObject);

            _logger.LogInfo("Looking for mesh filter...");
            var meshFilter = portalCenterQuad.GetComponent<MeshFilter>();
            _logger.LogInfo($"Mesh filter ? ${meshFilter}");
            _mesh = meshFilter.sharedMesh;
            _logger.LogInfo($"Bortal center found ? ${_mesh}");

            togglePortalCenter(portalCenterQuad);

            orig(self);
        }

        private void togglePortalCenter(GameObject quad)
        {
            var meshFilter = quad.GetComponent<MeshFilter>();

            meshFilter.sharedMesh = _configEntry.Value ? _mesh : null;
        }


    }
}
