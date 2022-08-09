
using BepInEx.Bootstrap;
using BepInEx.Configuration;
using BepInEx.Logging;
using RiskOfOptions;
using RiskOfOptions.Options;
using UnityEngine;

namespace com.thejpaproject.avoptions
{
    public class RiskOfOptions
    {
        private protected static ManualLogSource logger = BepInEx.Logging.Logger.CreateLogSource("c.t.a.RiskOfOptions");
        private static RiskOfOptions instance = null;
        private static readonly Object _mut = new();
        private static readonly bool enabled = Chainloader.PluginInfos.ContainsKey("com.rune580.riskofoptions");

        public void AddOption(ConfigEntry<bool> value)
        {
            if (enabled) Add(value);
        }

        private void Add(ConfigEntry<bool> value)
        {
            ModSettingsManager.AddOption(new CheckBoxOption(value));
        }

        public static RiskOfOptions Instance
        {
            get
            {
                if (instance == null) throw new System.Exception("Instance not yet created");
                return instance;
            }
        }

        public static RiskOfOptions GetInstance()
        {
            if (instance == null)
            {

                lock (_mut)
                {
                    if (instance == null)
                    {
                        logger.LogDebug("Creating instance with options enabled=" + enabled);
                        instance = new RiskOfOptions();
                    }
                }

            }
            return instance;
        }

        private void Configure()
        {
            ModSettingsManager.SetModDescription("Enable or disable various item audio/visual effects.");
            using var stream = GetType().Assembly.GetManifestResourceStream(".");
            var texture = new Texture2D(0, 0);
            var imgdata = new byte[stream.Length];
            stream.Read(imgdata, 0, imgdata.Length);
            if (ImageConversion.LoadImage(texture, imgdata))
                ModSettingsManager.SetModIcon(
                  Sprite.Create(
                    texture,
                    new Rect(0, 0, texture.width, texture.height),
                    new Vector2(0, 0)
                  )
                );
        }

        private RiskOfOptions()
        {

            if (RiskOfOptions.enabled) Configure();
            logger.LogDebug("RiskOfOptions integration enabled=" + enabled);
        }
    }
}
