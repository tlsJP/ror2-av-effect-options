using BepInEx;
using BepInEx.Bootstrap;
using BepInEx.Configuration;
using RiskOfOptions;
using RiskOfOptions.Options;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AddressableAssets;

[BepInPlugin(".AVFX_Options..", "AV Effect Options", "1.4.0")]
[BepInDependency("com.rune580.riskofoptions", (BepInDependency.DependencyFlags) 2)]
public sealed class _: BaseUnityPlugin {  
  private static bool ᛢᛪᛔᚸᚽᚹᛃᚬ = Chainloader.PluginInfos.ContainsKey("com.rune580.riskofoptions");
  
  [MethodImpl(768)]
  private void Awake() {
    if (ᛢᛪᛔᚸᚽᚹᛃᚬ) ᚧᛃᛍᛣᛩᚱᚦᛕ();
    ᚭᛣᛮᛨᚶᛟᚷᚴ("KillEliteFrenzy/NoCooldownEffect" , "Enable Brainstalks"      , "Enables Brainstalks' screen effect. Note: re-enabling may not take effect until next stage.");
    ᚭᛣᛮᛨᚶᛟᚷᚴ("IgniteOnKill/IgniteExplosionVFX"  , "Enable Gasoline"         , "Enables Gasoline's explosion");
    ᚭᛣᛮᛨᚶᛟᚷᚴ("ElementalRings/FireTornado"       , "Enable Kjaros Band"      , "Enables Kjaro's Band's tornado");
    ᚭᛣᛮᛨᚶᛟᚷᚴ("FireballsOnHit/FireMeatBallGhost" , "Enable Molten Perforator", "Enables the Molten Perforator visuals");
    ᚭᛣᛮᛨᚶᛟᚷᚴ("ElementalRings/IceRingExplosion"  , "Enable Runalds Band"     , "Enables Runald's Band's explosion");
    ᚭᛣᛮᛨᚶᛟᚷᚴ("Tonic/TonicBuffEffect"            , "Enable Spinel Tonic"     , "Enables Spinel Tonic's screen effect");
    ᚭᛣᛮᛨᚶᛟᚷᚴ("StickyBomb/StickyBombGhost"       , "Enable Sticky Bomb"      , "Enables Sticky Bomb's visuals");
    ᚭᛣᛮᛨᚶᛟᚷᚴ("ExplodeOnDeath/WilloWispExplosion", "Enable Will-o-the-Wisp"  , "Enables Will o' the Wisp's explosion");
  }
  
  [MethodImpl(768)]
  private void ᚭᛣᛮᛨᚶᛟᚷᚴ(string ᚱᛢᛊᚢᚴᛤᛉᚣ, string ᛆᛇᚶᛔᛒᛄᚤᛪ, string ᚡᛧᛞᛍᛏᛀᛕᚵ, string ᚩᛶᛠᚿᚵᚯᚹᛴ = "Item Effects") {
    try {
      var prefab = Addressables.LoadAsset<GameObject>("RoR2/Base/" + ᚱᛢᛊᚢᚴᛤᛉᚣ + ".prefab").WaitForCompletion();
      var config = Config.Bind(ᚩᛶᛠᚿᚵᚯᚹᛴ, ᛆᛇᚶᛔᛒᛄᚤᛪ, true, ᚡᛧᛞᛍᛏᛀᛕᚵ);
      // todo: should the following class be merged into this one?
      config.SettingChanged += (x, _) =>
        prefab.SetActive(((ConfigEntry<bool>) x).Value);
      prefab.SetActive(config.Value);
      if (ᛢᛪᛔᚸᚽᚹᛃᚬ) ᚾᛏᛧᛨᚭᛋᛳᚩ(config);
    } catch {}
  }
  
  [MethodImpl(520)]
  private void ᚾᛏᛧᛨᚭᛋᛳᚩ(ConfigEntry<bool> _) =>
    ModSettingsManager.AddOption(new CheckBoxOption(_));
  
  [MethodImpl(520)]
  private void ᚧᛃᛍᛣᛩᚱᚦᛕ() {
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
