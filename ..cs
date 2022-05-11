using BepInEx;
using BepInEx.Bootstrap;
using BepInEx.Configuration;
using RiskOfOptions;
using RiskOfOptions.Options;
using RoR2;
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
  private static ConfigEntry<bool> DeskPlantPlantConfig;
  
  [MethodImpl(768)]
  private void Awake() {
    FrostRelicFOVConfig = Config.Bind("Item Effects", "Enable Frost Relic On-Kill FOV", true, "Enables the temporary FOV change that Frost Relic's on-kill proc gives. Does not affect the particle effects (see the Frost Relic Particles option).");
    FrostRelicSoundConfig = Config.Bind("Item Effects", "Enable Frost Relic On-Kill Sound", true, "Enables the sound effects of Frost Relic's on-kill proc.");
    FrostRelicParticlesConfig = Config.Bind("Item Effects", "Enable Frost Relic Particles", true, "Enables the chunk and ring effects of Frost Relic. Does not affect the spherical area effect that indicates the item's area of effect, or the floating ice crystal that follows characters with the Frost Relic item.");
    try {
      On.RoR2.IcicleAuraController.OnIciclesActivated += ᚫᛈᚸᛡᚩᚺᛩᛮ;
      On.RoR2.IcicleAuraController.OnIcicleGained += ᛗᛕᛈᚩᚴᚷᚢᛛ;
    } catch {}
    
    ᚭᛣᛮᛨᚶᛟᚷᚴ("KillEliteFrenzy/NoCooldownEffect" , "Enable Brainstalks"          , "Enables Brainstalks' screen effect. Note: re-enabling may not take effect until next stage.");
    if (ᛢᛪᛔᚸᚽᚹᛃᚬ) {
      ᚧᛃᛍᛣᛩᚱᚦᛕ();
      ᚾᛏᛧᛨᚭᛋᛳᚩ(FrostRelicFOVConfig);
      ᚾᛏᛧᛨᚭᛋᛳᚩ(FrostRelicSoundConfig);
      ᚾᛏᛧᛨᚭᛋᛳᚩ(FrostRelicParticlesConfig);
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
    
    ᚭᛣᛮᛨᚶᛟᚷᚴ("Plant/DeskplantWard", "DeskplantWard", "Floofs <3" , "YIFFING");
    ᚭᛣᛮᛨᚶᛟᚷᚴ("Plant/DisplayInterstellarDeskPlant", "DisplayInterstellarDeskPlant", "Floofs <3" , "YIFFING");
    ᚭᛣᛮᛨᚶᛟᚷᚴ("Plant/InterstellarDeskPlantGroundEffects", "InterstellarDeskPlantGroundEffects", "Floofs <3" , "YIFFING");
    ᚭᛣᛮᛨᚶᛟᚷᚴ("Plant/InterstellarDeskPlant", "InterstellarDeskPlant", "Floofs <3" , "YIFFING");
    ᚭᛣᛮᛨᚶᛟᚷᚴ("Plant/PickupInterstellarDeskPlant", "PickupInterstellarDeskPlant", "Floofs <3" , "YIFFING");
    
    // Note: "InterstellarDeskPlant" disables the AOE circle, the sprouting mushroom effect,
    //   AND the actual healing (boo); but leaves everything else, such as the plant itself.
    // WRONG "InterstellarDeskPlant" child "Seed"/"Spores" disables the sprouting mushroom effect.
    
    System.Console.WriteLine("BEGIN DESK PLANT DEBUG");
    
    DeskPlantPlantConfig = Config.Bind("YIFFING", "Enable Desk Plant Spore Particles", true, "FLOOFS <3");
    if (ᛢᛪᛔᚸᚽᚹᛃᚬ) ᚾᛏᛧᛨᚭᛋᛳᚩ(DeskPlantPlantConfig);
    
    //var basetype = typeof(RoR2.DeskPlantController).GetNestedTypes(BindingFlags.NonPublic)[0].BaseType;
    var dpct = typeof(RoR2.DeskPlantController);
    var basetype = dpct.GetNestedType("DeskPlantBaseState", BindingFlags.NonPublic);
    System.Console.WriteLine(basetype);
    var seed = dpct.GetNestedType("SeedState", BindingFlags.NonPublic);
    System.Console.WriteLine(seed);
    var sprout = dpct.GetNestedType("SproutState", BindingFlags.NonPublic);
    System.Console.WriteLine(sprout);
    var main = dpct.GetNestedType("SproutState", BindingFlags.NonPublic);
    System.Console.WriteLine(main);
    var death = dpct.GetNestedType("DeathState", BindingFlags.NonPublic);
    System.Console.WriteLine(death);
    System.Console.WriteLine("-----");
    var plant_a = seed.GetProperty("showPlantObject", BindingFlags.NonPublic | BindingFlags.Instance);
    var plant_b = sprout.GetProperty("showPlantObject", BindingFlags.NonPublic | BindingFlags.Instance);
    var plant_c = main.GetProperty("showPlantObject", BindingFlags.NonPublic | BindingFlags.Instance);
    var plant_d = death.GetProperty("showPlantObject", BindingFlags.NonPublic | BindingFlags.Instance);
    System.Console.WriteLine(plant_a);
    System.Console.WriteLine(plant_b);
    System.Console.WriteLine(plant_c);
    System.Console.WriteLine(plant_d);
    
    try {
      On.RoR2.DeskPlantController.DeskPlantBaseState.OnEnter += ᛁᛖᛑᛞᚤᚾᚹᛊ;
    } catch {}
    
    System.Console.WriteLine("END DESK PLANT DEBUG");
  }
  
  private void ᛁᛖᛑᛞᚤᚾᚹᛊ(On.RoR2.DeskPlantController.DeskPlantBaseState.orig_OnEnter orig, EntityStates.BaseState self) {
    if (DeskPlantPlantConfig.Value) {
		  orig(self);
		} else {
      System.Console.WriteLine("Skipping Desk plant!");
      var selftype = self.GetType();
      var getcomponent = selftype.GetMethod("GetComponent", BindingFlags.NonPublic | BindingFlags.Instance, null, new [] {typeof(System.Type)}, null);
      var controller = (DeskPlantController)getcomponent.Invoke(self, new [] {typeof(DeskPlantController)});
      var showseed = selftype.GetProperty("showSeedObject", BindingFlags.NonPublic | BindingFlags.Instance);
      var selfcontroller = selftype.GetField("controller", BindingFlags.NonPublic | BindingFlags.Instance);
      selfcontroller.SetValue(self, controller);
      System.Console.WriteLine(selftype);
      System.Console.WriteLine(getcomponent);
      System.Console.WriteLine(controller);
      System.Console.WriteLine(selfcontroller);
      System.Console.WriteLine(showseed);
		  controller.seedObject.SetActive((bool)showseed.GetValue(self));
		  controller.plantObject.SetActive(false);
		}
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
