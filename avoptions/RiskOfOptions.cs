
using BepInEx.Configuration;
using RiskOfOptions;
using RiskOfOptions.Options;
using UnityEngine;

namespace com.thejpaproject.avoptions
{
    public class RiskOfOptions
    {

        private static RiskOfOptions instance = null;
        private static bool enabled;

        public void AddOption(ConfigEntry<bool> value)
        {
            if (enabled) ModSettingsManager.AddOption(new CheckBoxOption(value));
        }

        private RiskOfOptions() { }

        public static RiskOfOptions Instance
        {
            get
            {
                if (instance == null) throw new System.Exception("Instance not yet created");
                return instance;
            }
        }

        public static RiskOfOptions GetInstance(bool enabled)
        {
            if (instance == null)
            {
                instance = new RiskOfOptions(enabled);
            }
            return instance;
        }

        private RiskOfOptions(bool enabled)
        {
            if (enabled)
            {
                RiskOfOptions.enabled = enabled;
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
        }
    }
}
