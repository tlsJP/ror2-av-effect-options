
using BepInEx.Bootstrap;
using BepInEx.Configuration;
using RiskOfOptions;
using RiskOfOptions.Options;
using UnityEngine;

namespace com.thejpaproject.avoptions
{
    public class RiskOfOptions
    {

        private static readonly bool ExistsRiskOfOptions = Chainloader.PluginInfos.ContainsKey("com.rune580.riskofoptions");
        private static RiskOfOptions instance = null;

        public static RiskOfOptions Instance
        {
            get {
                if (instance == null) instance = new();
                return instance;
            }
        }


        public void AddOption(ConfigEntry<bool> value)
        {
            if (ExistsRiskOfOptions) ModSettingsManager.AddOption(new CheckBoxOption(value));
        }

        public RiskOfOptions()
        {
            ModSettingsManager.SetModDescription(
              "Enable or disable various item audio/visual effects."
            );
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
