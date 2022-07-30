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

[BepInPlugin(".AVFX_Options..", "AV Effect Options", "1.12.0")]
[BepInDependency("com.rune580.riskofoptions", (BepInDependency.DependencyFlags) 2)]
public sealed class _: BaseUnityPlugin {  
  private static bool ᛢᛪᛔᚸᚽᚹᛃᚬ = Chainloader.PluginInfos.ContainsKey("com.rune580.riskofoptions");
  
  private static ConfigEntry<bool> FrostRelicSoundConfig;
  private static ConfigEntry<bool> FrostRelicFOVConfig;
  private static ConfigEntry<bool> FrostRelicParticlesConfig;
  
  private static FieldInfo IcicleAuraAimRequest;
  
  private static GameObject DeskplantSpores;
  private static GameObject DeskplantSymbols;
  private static GameObject DeskplantMushrooms;
  private static GameObject FireTornadoSmoke;
  private static GameObject FireTornadoMeshCore;
  private static GameObject FireTornadoMeshWide;
  private static GameObject FireTornadoEmbers;
  private static GameObject FireTornadoLight;
  private static GameObject FireTornadoBurst;
  private static GameObject IceRingExplosionPrefab;
  
  private static Transform CleanseTransform;
  private static LoopSoundPlayer MushroomVoidAudio;
  private static TemporaryVisualEffect MushroomVoidVisual;
  private static DestroyOnUpdate IceRingExplosionDestructor;
  private static EffectComponent CleanseEffect;
  
  [MethodImpl(768)]
  private void Awake() {
    FrostRelicFOVConfig = Config.Bind("Item Effects", "Enable Frost Relic FOV", true, "Enables the temporary FOV change that Frost Relic's on-kill proc gives. Does not affect the particle effects (see the Frost Relic Particles option).");
    FrostRelicParticlesConfig = Config.Bind("Item Effects", "Enable Frost Relic Particles", true, "Enables the chunk and ring effects of Frost Relic. Does not affect the spherical area effect that indicates the item's area of effect, or the floating ice crystal that follows characters with the Frost Relic item.");
    FrostRelicSoundConfig = Config.Bind("Item Effects", "Enable Frost Relic Sound", true, "Enables the sound effects of Frost Relic's on-kill proc.");
    
    if (ᛢᛪᛔᚸᚽᚹᛃᚬ) ᚧᛃᛍᛣᛩᚱᚦᛕ();
    
    // Blast Shower
    try {
      CleanseTransform = Addressables.LoadAsset<GameObject>("RoR2/Base/Cleanse/CleanseEffect.prefab").WaitForCompletion().transform;
      CleanseEffect = CleanseTransform.GetComponent<EffectComponent>();
      var CleanseVisualConfig = Config.Bind("Item Effects", "Enable Blast Shower", true, "Enables Blast Shower's effects.");
      if (!CleanseVisualConfig.Value) {
        CleanseEffect.effectIndex = (EffectIndex) (-1);
        for (var i = 0; i < CleanseTransform.childCount; i++)
          CleanseTransform.GetChild(i).gameObject.SetActive(false);
      }
      CleanseVisualConfig.SettingChanged += ᛑᛯᛜᛧᛇᛝᛓᚻ;
      if (ᛢᛪᛔᚸᚽᚹᛃᚬ) ᚾᛏᛧᛨᚭᛋᛳᚩ(CleanseVisualConfig);
    } catch {
      Logger.LogError("Could not hook onto Blast Shower.");
    }
    
    ᚭᛣᛮᛨᚶᛟᚷᚴ("KillEliteFrenzy/NoCooldownEffect" , "Enable Brainstalks"          , "Enables Brainstalks' screen effect. Note: re-enabling may not take effect until next stage.");
    
    // Interstellar Desk Plant
    try {
      var DeskplantIndicatorTransform = Addressables.LoadAsset<GameObject>("RoR2/Base/Plant/DeskplantWard.prefab").WaitForCompletion().transform.Find("Indicator").gameObject.transform;
      DeskplantSpores = DeskplantIndicatorTransform.Find("Spores").gameObject;
      DeskplantSymbols = DeskplantIndicatorTransform.Find("HealingSymbols").gameObject;
      DeskplantMushrooms = DeskplantIndicatorTransform.Find("MushroomMeshes").gameObject;
      var DeskPlantIndicatorConfig = Config.Bind("Item Effects", "Enable Desk Plant Ward Particles", true, "Enables the spore, plus sign, and mushroom visual effects from Interstellar Desk Plant's healing ward indicator. Does not affect the particle effects of the Desk Plant seed, or the perimeter sphere of the ward.");
      ᛁᛖᛑᛞᚤᚾᚹᛊ(DeskPlantIndicatorConfig);
      DeskPlantIndicatorConfig.SettingChanged += ᛁᛖᛑᛞᚤᚾᚹᛊ;
      if (ᛢᛪᛔᚸᚽᚹᛃᚬ) ᚾᛏᛧᛨᚭᛋᛳᚩ(DeskPlantIndicatorConfig);
    } catch {
      Logger.LogError("Could not hook onto Interstellar Desk Plant ward.");
    }
    
    try {
      IcicleAuraAimRequest = typeof(IcicleAuraController).GetField("aimRequest", (BindingFlags) 36);
      On.RoR2.IcicleAuraController.OnIciclesActivated += ᚫᛈᚸᛡᚩᚺᛩᛮ;
      On.RoR2.IcicleAuraController.OnIcicleGained += ᛗᛕᛈᚩᚴᚷᚢᛛ;
      if (ᛢᛪᛔᚸᚽᚹᛃᚬ) {
        ᚾᛏᛧᛨᚭᛋᛳᚩ(FrostRelicFOVConfig);
        ᚾᛏᛧᛨᚭᛋᛳᚩ(FrostRelicParticlesConfig);
        ᚾᛏᛧᛨᚭᛋᛳᚩ(FrostRelicSoundConfig);
      }
    } catch {
      Logger.LogError("Could not hook onto Frost Relic.");
    }
    
    ᚭᛣᛮᛨᚶᛟᚷᚴ("IgniteOnKill/IgniteExplosionVFX", "Enable Gasoline", "Enables Gasoline's explosion");
    
    // Kjaro's Band
    try {
      var FireTornadoGhost = Addressables.LoadAsset<GameObject>("RoR2/Base/ElementalRings/FireTornadoGhost.prefab").WaitForCompletion().transform;
      FireTornadoSmoke = FireTornadoGhost.Find("Smoke").gameObject;
      FireTornadoMeshCore = FireTornadoGhost.Find("TornadoMeshCore").gameObject;
      FireTornadoMeshWide = FireTornadoGhost.Find("TornadoMeshCore, Wide").gameObject;
      FireTornadoEmbers = FireTornadoGhost.Find("Embers").gameObject;
      FireTornadoLight = FireTornadoGhost.Find("Point Light").gameObject;
      FireTornadoBurst = FireTornadoGhost.Find("InitialBurst").gameObject;
      var FireTornadoConfig = Config.Bind("Item Effects", "Enable Kjaros Band", true, "Enables Kjaro's Band's fire tornado.");
      FireTornadoSmoke.SetActive(FireTornadoConfig.Value);
      FireTornadoMeshCore.SetActive(FireTornadoConfig.Value);
      FireTornadoMeshWide.SetActive(FireTornadoConfig.Value);
      FireTornadoEmbers.SetActive(FireTornadoConfig.Value);
      FireTornadoLight.SetActive(FireTornadoConfig.Value);
      FireTornadoBurst.SetActive(FireTornadoConfig.Value);
      FireTornadoConfig.SettingChanged += ᛝᛂᚨᛪᛖᛣᚡᚢ;
      if (ᛢᛪᛔᚸᚽᚹᛃᚬ) ᚾᛏᛧᛨᚭᛋᛳᚩ(FireTornadoConfig);
    } catch {
      Logger.LogError("Could not hook onto Kjaro's Band.");
    }
    
    ᚭᛣᛮᛨᚶᛟᚷᚴ("FireballsOnHit/FireMeatBallGhost", "Enable Molten Perforator", "Enables the Molten Perforator visuals");
    
    // Runald's Band
    try {
      IceRingExplosionPrefab = Addressables.LoadAsset<GameObject>("RoR2/Base/ElementalRings/IceRingExplosion.prefab").WaitForCompletion();
      var IceRingExplosionConfig = Config.Bind("Item Effects", "Enable Runalds Band", true, "Enables Runald's Band's ice explosion.");
      if (!IceRingExplosionConfig.Value)
        IceRingExplosionDestructor = IceRingExplosionPrefab.AddComponent<DestroyOnUpdate>();
      var IceRingExplosionTransform = IceRingExplosionPrefab.transform;
      for (var i = 0; i < IceRingExplosionTransform.childCount; i++)
        IceRingExplosionTransform.GetChild(i).gameObject.SetActive(IceRingExplosionConfig.Value);
      IceRingExplosionConfig.SettingChanged += ᛔᛋᛡᛆᛅᛛᛞᛘ;
      if (ᛢᛪᛔᚸᚽᚹᛃᚬ) ᚾᛏᛧᛨᚭᛋᛳᚩ(IceRingExplosionConfig);
    } catch {
      Logger.LogError("Could not hook onto Runald's Band.");
    }
    
    ᚭᛣᛮᛨᚶᛟᚷᚴ("BleedOnHitAndExplode/BleedOnHitAndExplode_Explosion", "Enable Shatterspleen", "Enables Shatterspleen's explosion");
    ᚭᛣᛮᛨᚶᛟᚷᚴ("Tonic/TonicBuffEffect"            , "Enable Spinel Tonic"         , "Enables Spinel Tonic's screen effect");
    ᚭᛣᛮᛨᚶᛟᚷᚴ("StickyBomb/StickyBombGhost"       , "Enable Sticky Bomb Drops"    , "Enables Sticky Bomb's drops");
    ᚭᛣᛮᛨᚶᛟᚷᚴ("StickyBomb/BehemothVFX"           , "Enable Sticky Bomb Explosion", "Enables Sticky Bomb's explosion");
    ᚭᛣᛮᛨᚶᛟᚷᚴ("ExplodeOnDeath/WilloWispExplosion", "Enable Will-o-the-Wisp"      , "Enables Will o' the Wisp's explosion");
    
    // Weeping Bungus
    try {
      var MushroomVoidEffectPrefab = Addressables.LoadAsset<GameObject>("RoR2/DLC1/MushroomVoid/MushroomVoidEffect.prefab").WaitForCompletion();
      MushroomVoidVisual = MushroomVoidEffectPrefab.GetComponent<TemporaryVisualEffect>();
      MushroomVoidAudio = MushroomVoidEffectPrefab.GetComponent<LoopSoundPlayer>();
      var WungusAudioConfig = Config.Bind("SOTV Item Effects", "Enable Weeping Fungus Sound", true, "Enables Weeping Fungus' sound effect. Take effect immediately.");
      var WungusVisualsConfig = Config.Bind("SOTV Item Effects", "Enable Weeping Fungus Visuals", true, "Enables Weeping Fungus' visual particle effects. This includes the floating plus symbols, the floating spore particles, and the void star particle effects. Does not affect the generic green healing pulsing effect. Note: re-enabling may not take effect until next stage.");
      MushroomVoidAudio.enabled = WungusAudioConfig.Value;
      MushroomVoidVisual.enabled = WungusVisualsConfig.Value;
      WungusAudioConfig.SettingChanged += ᚥᛤᚨᛕᛅᛯᚲᛚ;
      WungusVisualsConfig.SettingChanged += ᛝᚯᛠᛕᚪᛯᚼᛨ;
      if (ᛢᛪᛔᚸᚽᚹᛃᚬ) {
        ᚾᛏᛧᛨᚭᛋᛳᚩ(WungusAudioConfig);
        ᚾᛏᛧᛨᚭᛋᛳᚩ(WungusVisualsConfig);
      }
    } catch {
      Logger.LogError("Could not hook onto Wungus.");
    }
    
    ᚭᛣᛮᛨᚶᛟᚷᚴ("Titan/TitanDeathEffect"           , "Enable Titan Death Effect"   , "Enables Stone Titan's on-death explosion. Disabling will cause Stone Titans to disappear on death instead of creating a corpse.", "Character Effects");
    ᚭᛣᛮᛨᚶᛟᚷᚴ("Vagrant/VagrantDeathExplosion"    , "Enable Vagrant Death Explosion", "Enables Wandering Vagrant's on-death explosion. Disabling will cause Wandering Vagrants to disappear on death instead of creating a corpse.", "Character Effects");
  }
  
  [MethodImpl(768)]
  private void ᛑᛯᛜᛧᛇᛝᛓᚻ(object x, EventArgs _) {
    var y = ((ConfigEntry<bool>)x).Value;
    CleanseEffect.effectIndex = (EffectIndex) (y ? 102 : -1);
    for (var i = 0; i < CleanseTransform.childCount; i++)
      CleanseTransform.GetChild(i).gameObject.SetActive(y);
  }
  
  [MethodImpl(768)]
  private void ᛔᛋᛡᛆᛅᛛᛞᛘ(object x, EventArgs _) {
    var transform = IceRingExplosionPrefab.transform;
    var childCount = transform.childCount;
    var y = ((ConfigEntry<bool>)x).Value;
    if (y && IceRingExplosionDestructor != null) {
      UnityEngine.Object.Destroy(IceRingExplosionDestructor);
      IceRingExplosionDestructor = null;
    } else
      IceRingExplosionDestructor = IceRingExplosionPrefab.AddComponent<DestroyOnUpdate>();
    for (var i = 0; i < childCount; i++)
      transform.GetChild(i).gameObject.SetActive(y);
  }
  
  [MethodImpl(768)]
  private void ᛝᛂᚨᛪᛖᛣᚡᚢ(object x, EventArgs _) {
    var y = ((ConfigEntry<bool>)x).Value;
    FireTornadoSmoke.SetActive(y);
    FireTornadoMeshCore.SetActive(y);
    FireTornadoMeshWide.SetActive(y);
    FireTornadoEmbers.SetActive(y);
    FireTornadoLight.SetActive(y);
    FireTornadoBurst.SetActive(y);
  }
  
  [MethodImpl(768)]
  private void ᚥᛤᚨᛕᛅᛯᚲᛚ(object x, EventArgs _) =>
    MushroomVoidAudio.enabled = ((ConfigEntry<bool>)x).Value;
  
  [MethodImpl(768)]
  private void ᛝᚯᛠᛕᚪᛯᚼᛨ(object x, EventArgs _) =>
    MushroomVoidVisual.enabled = ((ConfigEntry<bool>)x).Value;
  
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
      if (ctp) IcicleAuraAimRequest.SetValue(self, ctp.RequestAimType(CameraTargetParams.AimType.Aura));
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

public sealed class DestroyOnUpdate : MonoBehaviour {
  public void Update () =>
    UnityEngine.Object.Destroy(base.gameObject);
}
