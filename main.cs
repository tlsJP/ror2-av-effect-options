[BepInEx.BepInPlugin("XmdzoIwkDXCTKH64", "AV Effect Options", "1.0.0")]
[BepInEx.BepInDependency("com.rune580.riskofoptions", BepInEx.BepInDependency.DependencyFlags.SoftDependency)]
public sealed class ᚠᛯᛇᛁᚢᛮᛶᛁᛋᚴᛪᛩᚣᛉᚠᛉ: BepInEx.BaseUnityPlugin {

  private static UnityEngine.GameObject StickyBombPrefab = UnityEngine.AddressableAssets.Addressables.LoadAssetAsync<UnityEngine.GameObject>("RoR2/Base/StickyBomb/StickyBombGhost.prefab").WaitForCompletion();
  private static UnityEngine.GameObject MoltenPerforatorPrefab = UnityEngine.AddressableAssets.Addressables.LoadAssetAsync<UnityEngine.GameObject>("RoR2/Base/FireballsOnHit/FireMeatBallGhost.prefab").WaitForCompletion();
  private static UnityEngine.GameObject SpinelTonicPrefab = UnityEngine.AddressableAssets.Addressables.LoadAssetAsync<UnityEngine.GameObject>("RoR2/Base/Tonic/TonicBuffEffect.prefab").WaitForCompletion();
  
  private BepInEx.Configuration.ConfigEntry<bool> enableStickyBomb;
  private BepInEx.Configuration.ConfigEntry<bool> enableMoltenPerforator;
  private BepInEx.Configuration.ConfigEntry<bool> enableSpinelTonic;
  
  public void Awake() {
    enableStickyBomb = Config.Bind("Item Effects", "Enable Sticky Bomb", true, "Enables the Sticky Bomb visual");
		enableMoltenPerforator = Config.Bind("Item Effects", "Enable Molten Perforator", true, "Enables Molten Perforator visuals");
		enableSpinelTonic = Config.Bind("Item Effects", "Enable Spinel Tonic", true, "Enables the Spinel Tonic screen effect");
		
		StickyBombPrefab.SetActive(enableStickyBomb.Value);
		MoltenPerforatorPrefab.SetActive(enableMoltenPerforator.Value);
		SpinelTonicPrefab.SetActive(enableSpinelTonic.Value);
		
    enableStickyBomb.SettingChanged += (x, _) => 
      StickyBombPrefab.SetActive(((BepInEx.Configuration.ConfigEntry<bool>) x).Value);
    enableMoltenPerforator.SettingChanged += (x, _) =>
      MoltenPerforatorPrefab.SetActive(((BepInEx.Configuration.ConfigEntry<bool>) x).Value);
    enableSpinelTonic.SettingChanged += (x, _) =>
      SpinelTonicPrefab.SetActive(((BepInEx.Configuration.ConfigEntry<bool>) x).Value);
		
    if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.rune580.riskofoptions"))
      ᚻᛀᛧᚠᚾᛢᚻᛐᛉᚼᛤᛪᚲᛇᛎᛛ();
  }
  
  [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
  private void ᚻᛀᛧᚠᚾᛢᚻᛐᛉᚼᛤᛪᚲᛇᛎᛛ() {
    RiskOfOptions.ModSettingsManager.AddOption(new RiskOfOptions.Options.CheckBoxOption(enableStickyBomb));
		RiskOfOptions.ModSettingsManager.AddOption(new RiskOfOptions.Options.CheckBoxOption(enableMoltenPerforator));
		RiskOfOptions.ModSettingsManager.AddOption(new RiskOfOptions.Options.CheckBoxOption(enableSpinelTonic));
  }

}
