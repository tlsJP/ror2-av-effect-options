
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
        private protected static ManualLogSource s_logger = BepInEx.Logging.Logger.CreateLogSource("c.t.a.RiskOfOptions");
        private static RiskOfOptions s_instance = null;
        private static readonly Object s_mut = new();
        private static readonly bool s_enabled = Chainloader.PluginInfos.ContainsKey("com.rune580.riskofoptions");

        public void AddOption(ConfigEntry<bool> value)
        {
            if (s_enabled) Add(value);
        }

        private void Add(ConfigEntry<bool> value)
        {
            ModSettingsManager.AddOption(new CheckBoxOption(value));
        }

        public static RiskOfOptions Instance
        {
            get
            {
                if (s_instance == null) throw new System.Exception("Instance not yet created");
                return s_instance;
            }
        }

        public static RiskOfOptions GetInstance()
        {
            if (s_instance == null)
            {

                lock (s_mut)
                {
                    if (s_instance == null)
                    {
                        s_logger.LogDebug("Creating instance with options enabled=" + s_enabled);
                        s_instance = new RiskOfOptions();
                    }
                }

            }
            return s_instance;
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

            if (RiskOfOptions.s_enabled) Configure();
            s_logger.LogDebug($"RiskOfOptions integration enabled={s_enabled}");
        }
    }
}
