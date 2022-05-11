using BepInEx;
using BepInEx.Bootstrap;
using BepInEx.Configuration;
using RiskOfOptions;
using RiskOfOptions.Options;
using RoR2;
using System;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AddressableAssets;

[BepInPlugin(".AVFX_Options..", "AV Effect Options", "1.8.0")]
[BepInDependency("com.rune580.riskofoptions", (BepInDependency.DependencyFlags) 2)]
public sealed class _: BaseUnityPlugin {  
  private static bool ᛢᛪᛔᚸᚽᚹᛃᚬ = Chainloader.PluginInfos.ContainsKey("com.rune580.riskofoptions");
  
  private static ConfigEntry<bool> FrostRelicSoundConfig;
  private static ConfigEntry<bool> FrostRelicFOVConfig;
  private static ConfigEntry<bool> FrostRelicParticlesConfig;
  
  private static GameObject DeskplantSpores;
  private static GameObject DeskplantSymbols;
  private static GameObject DeskplantMushrooms;
  
  [MethodImpl(768)]
  private void Awake() {
    FrostRelicFOVConfig = Config.Bind("Item Effects", "Enable Frost Relic FOV", true, "Enables the temporary FOV change that Frost Relic's on-kill proc gives. Does not affect the particle effects (see the Frost Relic Particles option).");
    FrostRelicParticlesConfig = Config.Bind("Item Effects", "Enable Frost Relic Particles", true, "Enables the chunk and ring effects of Frost Relic. Does not affect the spherical area effect that indicates the item's area of effect, or the floating ice crystal that follows characters with the Frost Relic item.");
    FrostRelicSoundConfig = Config.Bind("Item Effects", "Enable Frost Relic Sound", true, "Enables the sound effects of Frost Relic's on-kill proc.");
    
    var DeskPlantIndicatorConfig = Config.Bind("Item Effects", "Enable Desk Plant Ward Particles", true, "Enables the spore, plus sign, and mushroom visual effects from Interstellar Desk Plant's healing ward indicator. Does not affect the particle effects of the Desk Plant seed, or the perimeter sphere of the ward.");
    
    if (ᛢᛪᛔᚸᚽᚹᛃᚬ) ᚧᛃᛍᛣᛩᚱᚦᛕ();
    
    ᚭᛣᛮᛨᚶᛟᚷᚴ("KillEliteFrenzy/NoCooldownEffect" , "Enable Brainstalks"          , "Enables Brainstalks' screen effect. Note: re-enabling may not take effect until next stage.");
    
    try {
      var DeskplantIndicatorTransform = Addressables.LoadAsset<GameObject>("RoR2/Base/Plant/DeskplantWard.prefab").WaitForCompletion().transform.Find("Indicator").gameObject.transform;
      DeskplantSpores = DeskplantIndicatorTransform.Find("Spores").gameObject;
      DeskplantSymbols = DeskplantIndicatorTransform.Find("HealingSymbols").gameObject;
      DeskplantMushrooms = DeskplantIndicatorTransform.Find("MushroomMeshes").gameObject;
      DeskPlantIndicatorConfig.SettingChanged += ᛁᛖᛑᛞᚤᚾᚹᛊ;
      ᛁᛖᛑᛞᚤᚾᚹᛊ(DeskPlantIndicatorConfig);
      if (ᛢᛪᛔᚸᚽᚹᛃᚬ) ᚾᛏᛧᛨᚭᛋᛳᚩ(DeskPlantIndicatorConfig);
    } catch {
      Logger.LogError("Could not hook onto Interstellar Desk Plant ward particles.");
    }
    
    try {
      On.RoR2.IcicleAuraController.OnIciclesActivated += ᚫᛈᚸᛡᚩᚺᛩᛮ;
      On.RoR2.IcicleAuraController.OnIcicleGained += ᛗᛕᛈᚩᚴᚷᚢᛛ;
      if (ᛢᛪᛔᚸᚽᚹᛃᚬ) {
        ᚾᛏᛧᛨᚭᛋᛳᚩ(FrostRelicFOVConfig);
        ᚾᛏᛧᛨᚭᛋᛳᚩ(FrostRelicParticlesConfig);
        ᚾᛏᛧᛨᚭᛋᛳᚩ(FrostRelicSoundConfig);
      }
    } catch {
      Logger.LogError("Could not hook onto Frost Relic particles.");
    }
    
    ᚭᛣᛮᛨᚶᛟᚷᚴ("IgniteOnKill/IgniteExplosionVFX"  , "Enable Gasoline"             , "Enables Gasoline's explosion");
    ᚭᛣᛮᛨᚶᛟᚷᚴ("ElementalRings/FireTornado"       , "Enable Kjaros Band"          , "Enables Kjaro's Band's tornado");
    ᚭᛣᛮᛨᚶᛟᚷᚴ("FireballsOnHit/FireMeatBallGhost" , "Enable Molten Perforator"    , "Enables the Molten Perforator visuals");
    ᚭᛣᛮᛨᚶᛟᚷᚴ("ElementalRings/IceRingExplosion"  , "Enable Runalds Band"         , "Enables Runald's Band's explosion");
    ᚭᛣᛮᛨᚶᛟᚷᚴ("BleedOnHitAndExplode/BleedOnHitAndExplode_Explosion", "Enable Shatterspleen", "Enables Shatterspleen's explosion");
    ᚭᛣᛮᛨᚶᛟᚷᚴ("Tonic/TonicBuffEffect"            , "Enable Spinel Tonic"         , "Enables Spinel Tonic's screen effect");
    ᚭᛣᛮᛨᚶᛟᚷᚴ("StickyBomb/StickyBombGhost"       , "Enable Sticky Bomb Drops"    , "Enables Sticky Bomb's drops");
    ᚭᛣᛮᛨᚶᛟᚷᚴ("StickyBomb/BehemothVFX"           , "Enable Sticky Bomb Explosion", "Enables Sticky Bomb's explosion");
    ᚭᛣᛮᛨᚶᛟᚷᚴ("ExplodeOnDeath/WilloWispExplosion", "Enable Will-o-the-Wisp"      , "Enables Will o' the Wisp's explosion");
    ᚭᛣᛮᛨᚶᛟᚷᚴ("Titan/TitanDeathEffect"           , "Enable Titan Death Effect"   , "Enables Stone Titan's on-death explosion. Disabling will cause Stone Titans to disappear on death instead of creating a corpse.", "Character Effects");
    ᚭᛣᛮᛨᚶᛟᚷᚴ("Vagrant/VagrantDeathExplosion"    , "Enable Vagrant Death Explosion", "Enables Wandering Vagrant's on-death explosion. Disabling will cause Wandering Vagrants to disappear on death instead of creating a corpse.", "Character Effects");
    
  }
  
  [MethodImpl(768)]
  private void ᛁᛖᛑᛞᚤᚾᚹᛊ(object x, EventArgs _ = null) {
    var y = ((ConfigEntry<bool>)x).Value;
    DeskplantSpores.SetActive(y);
    DeskplantSymbols.SetActive(y);
    DeskplantMushrooms.SetActive(y);
  }
  
  private void ᛗᛕᛈᚩᚴᚷᚢᛛ(On.RoR2.IcicleAuraController.orig_OnIcicleGained orig, IcicleAuraController self) {
    // WARN: the following code is probably illegal in your jurisdiction! Arrr matey!
    foreach (ParticleSystem part in self.procParticles)
      if (FrostRelicParticlesConfig.Value | part.name == "Area")
        part.Play();
  }
  
  private void ᚫᛈᚸᛡᚩᚺᛩᛮ(On.RoR2.IcicleAuraController.orig_OnIciclesActivated orig, IcicleAuraController self) {
    // WARN: the following code is probably illegal in your jurisdiction! Arrr matey!
    if (FrostRelicSoundConfig.Value)
      Util.PlaySound("Play_item_proc_icicle", self.gameObject);
    if (FrostRelicFOVConfig.Value) {
      var ctp = self.owner.GetComponent<CameraTargetParams>();
      if (ctp)
        typeof(IcicleAuraController).GetField("aimRequest", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(
          self, ctp.RequestAimType(CameraTargetParams.AimType.Aura)
        );
    }
    foreach (ParticleSystem part in self.auraParticles)
      if (FrostRelicParticlesConfig.Value | part.name == "Area") {
        var main = part.main;
        main.loop = true;
        part.Play();
      }
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
