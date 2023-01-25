

using BepInEx.Logging;
using UnityEngine;
using BepInEx.Configuration;
using System;

using UnityEngine.AddressableAssets;
using com.thejpaproject.avoptions.configurations;

namespace com.thejpaproject.avoptions
{
    public class PrefabConfigurer
    {

        private static ManualLogSource s_logger = BepInEx.Logging.Logger.CreateLogSource("c.t.a.PrefabConfigurer");

        private static PrefabConfigurer s_instance = null;

        private static readonly UnityEngine.Object s_mut = new();

        private ConfigFile _configFile;
        private RiskOfOptions _riskOfOptions = RiskOfOptions.GetInstance();

        public static PrefabConfigurer GetInstance(ConfigFile configFile)
        {
            if (s_instance != null)
            {
                s_logger.LogWarning("Configurer has already been created.");
                return s_instance;
            }

            lock (s_mut)
            {
                if (s_instance == null)
                {
                    s_instance = new PrefabConfigurer(configFile);
                }
            }
            return s_instance;
        }

        private PrefabConfigurer(ConfigFile configFile)
        {
            _configFile = configFile;
        }


        public void BindVfx(string assetPath, string title, string description) => this.BindAsset(Category.BASE_VFX, "Base/" + assetPath, title, description);
        public void BindSfx(string assetPath, string title, string description) => this.BindAsset(Category.BASE_SFX, "Base/" + assetPath, title, description);
        public void BindVoidVfx(string assetPath, string title, string description) => this.BindAsset(Category.VOID_VFX, "DLC1/" + assetPath, title, description);
        public void BindVoidSfx(string assetPath, string title, string description) => this.BindAsset(Category.VOID_SFX, "DLC1/" + assetPath, title, description);
        public void BindUnitFx(string assetPath, string title, string description) => this.BindAsset(Category.VOID_SFX, "DLC1/" + assetPath, title, description);


        public void BindAsset(string category, string assetPath, string title, string description)
        {
            try
            {
                var prefab = Addressables.LoadAsset<GameObject>("RoR2/" + assetPath + ".prefab").WaitForCompletion();
                var config = _configFile.Bind(category, title, true, description);
                config.SettingChanged += (x, _) => prefab.SetActive(((ConfigEntry<bool>)x).Value);
                prefab.SetActive(config.Value);
                _riskOfOptions.AddOption(config);
            }
            catch (Exception e)
            {
                throw new ConfigurationException(assetPath, e);
            }
        }

    }
}