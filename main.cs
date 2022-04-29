namespace ᛪᚶᚬᛣᚿᛐᚯᚰᛛᚸᚡᚢᛯᛆᛒᛡ { // C# 10.0 supports file-wide namespaces, but currently only C# 8.0 is supported.

[BepInEx.BepInPlugin("XmdzoIwkDXCTKH64", "AV Effect Options", "1.0.0")]
[BepInEx.BepInDependency("com.rune580.riskofoptions", BepInEx.BepInDependency.DependencyFlags.SoftDependency)]
public sealed class ᚠᛯᛇᛁᚢᛮᛶᛁᛋᚴᛪᛩᚣᛉᚠᛉ: BepInEx.BaseUnityPlugin {

  private static UnityEngine.GameObject StickyBombPrefab = UnityEngine.AddressableAssets.Addressables.LoadAssetAsync<UnityEngine.GameObject>("RoR2/Base/StickyBomb/StickyBombGhost.prefab").WaitForCompletion();
  private static UnityEngine.GameObject MoltenPerforatorPrefab = UnityEngine.AddressableAssets.Addressables.LoadAssetAsync<UnityEngine.GameObject>("RoR2/Base/FireballsOnHit/FireMeatBallGhost.prefab").WaitForCompletion();
  
  private BepInEx.Configuration.ConfigEntry<bool> enableStickyBomb;
  private BepInEx.Configuration.ConfigEntry<bool> enableMoltenPerforator;
  
  
  public void Awake() {
    enableStickyBomb = Config.Bind("Item Effects", "Remove Sticky Bomb", true, "Enables the Sticky Bomb Visual");
		enableMoltenPerforator = Config.Bind("Item Effects", "Remove Molten Perforator", true, "Enables Molten Perforator Visuals");
		
		StickyBombPrefab.SetActive(enableStickyBomb.Value);
		MoltenPerforatorPrefab.SetActive(enableMoltenPerforator.Value);
		
		System.Console.WriteLine("Hewwos!!");
		
    enableStickyBomb.SettingChanged += (sender, args) => {
      System.Console.WriteLine("Sticky Bomb Changed!");
      System.Console.WriteLine(sender);
      System.Console.WriteLine(args);
      StickyBombPrefab.SetActive(enableStickyBomb.Value);
    };
    enableMoltenPerforator.SettingChanged += (sender, args) => { 
      System.Console.WriteLine("Molten Perferator Changed!");
      System.Console.WriteLine(sender);
      System.Console.WriteLine(args);
      MoltenPerforatorPrefab.SetActive(enableMoltenPerforator.Value);
    };
		
    if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.rune580.riskofoptions"))
      ᚻᛀᛧᚠᚾᛢᚻᛐᛉᚼᛤᛪᚲᛇᛎᛛ();
  }
  
  private void ᚻᛀᛧᚠᚾᛢᚻᛐᛉᚼᛤᛪᚲᛇᛎᛛ() {
    RiskOfOptions.ModSettingsManager.AddOption(new RiskOfOptions.Options.CheckBoxOption(enableStickyBomb));
		RiskOfOptions.ModSettingsManager.AddOption(new RiskOfOptions.Options.CheckBoxOption(enableMoltenPerforator));
  }

}

}
