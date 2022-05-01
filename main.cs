using BepInEx;
using BepInEx.Bootstrap;
using BepInEx.Configuration;
using RiskOfOptions;
using RiskOfOptions.Options;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AddressableAssets;

[BepInPlugin("XmdzoIwkDXCTKH64", "AV Effect Options", "1.0.0")]
[BepInDependency("com.rune580.riskofoptions", (BepInDependency.DependencyFlags) 2)]
public sealed class ᚠᛯᛇᛁᚢᛮᛶᛁᛋᚴᛪᛩᚣᛉᚠᛉ: BaseUnityPlugin {  
  private bool ᛢᛪᛔᚸᚽᚹᛃᚬ = false;
  
  [MethodImpl(512)]
  public void Awake() {
    ᛢᛪᛔᚸᚽᚹᛃᚬ = Chainloader.PluginInfos.ContainsKey("com.rune580.riskofoptions");
    ᚭᛣᛮᛨᚶᛟᚷᚴ("StickyBomb/StickyBombGhost", "Enable Sticky Bomb", "Enables the Sticky Bomb visuals");
    ᚭᛣᛮᛨᚶᛟᚷᚴ("FireballsOnHit/FireMeatBallGhost", "Enable Molten Perforator", "Enables the Molten Perforator visuals");
    ᚭᛣᛮᛨᚶᛟᚷᚴ("Tonic/TonicBuffEffect", "Enable Spinel Tonic", "Enables the Spinel Tonic screen effect");
  }
  
  [MethodImpl(768)]
  private void ᚭᛣᛮᛨᚶᛟᚷᚴ(string ᚱᛢᛊᚢᚴᛤᛉᚣ, string ᛆᛇᚶᛔᛒᛄᚤᛪ, string ᚡᛧᛞᛍᛏᛀᛕᚵ, string ᚩᛶᛠᚿᚵᚯᚹᛴ = "Item Effects") {
    var prefab = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/" + ᚱᛢᛊᚢᚴᛤᛉᚣ + ".prefab").WaitForCompletion();
    var config = Config.Bind(ᚩᛶᛠᚿᚵᚯᚹᛴ, ᛆᛇᚶᛔᛒᛄᚤᛪ, true, ᚡᛧᛞᛍᛏᛀᛕᚵ);
    config.SettingChanged += (x, _) =>
      prefab.SetActive(((ConfigEntry<bool>) x).Value);
    prefab.SetActive(config.Value);
    if (ᛢᛪᛔᚸᚽᚹᛃᚬ)
      ᚾᛏᛧᛨᚭᛋᛳᚩ(config);
  }
  
  [MethodImpl(520)]
  private void ᚾᛏᛧᛨᚭᛋᛳᚩ(ConfigEntry<bool> ᚵᛑᛖᚶᚿᚥᛘᛸ) {
    ModSettingsManager.AddOption(new CheckBoxOption(ᚵᛑᛖᚶᚿᚥᛘᛸ));
  }
}
