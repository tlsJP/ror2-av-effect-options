using BepInEx;
using BepInEx.Bootstrap;
using BepInEx.Configuration;
using RiskOfOptions;
using RiskOfOptions.Options;
using RoR2;
using RoR2.Projectile;
using RoR2.Audio;
using System;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AddressableAssets;

[BepInPlugin(".AVFX_Options..", "JP's AV Effect Options", "1.12.0")]
[BepInDependency("com.rune580.riskofoptions", (BepInDependency.DependencyFlags)2)]
public sealed class AvOptions : BaseUnityPlugin
{
    private static readonly bool ExistsRiskOfOptions = Chainloader.PluginInfos.ContainsKey("com.rune580.riskofoptions");

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

    //private static LoopSoundPlayer PlimpAudio;
    //private static LoopSoundDef PlimpFlightSoundLoop;
    //private static string PlimpFlightSoundSoundName;

    private static ProjectileController PlimpController;


    [MethodImpl(768)]
    private void Awake()
    {
        FrostRelicFOVConfig = Config.Bind("Item Effects", "Enable Frost Relic FOV", true, "Enables the temporary FOV change that Frost Relic's on-kill proc gives. Does not affect the particle effects (see the Frost Relic Particles option).");
        FrostRelicParticlesConfig = Config.Bind("Item Effects", "Enable Frost Relic Particles", true, "Enables the chunk and ring effects of Frost Relic. Does not affect the spherical area effect that indicates the item's area of effect, or the floating ice crystal that follows characters with the Frost Relic item.");
        FrostRelicSoundConfig = Config.Bind("Item Effects", "Enable Frost Relic Sound", true, "Enables the sound effects of Frost Relic's on-kill proc.");

        if (ExistsRiskOfOptions) AddToRiskOfOptions();

        // Blast Shower
        try
        {
            CleanseTransform = Addressables.LoadAsset<GameObject>("RoR2/Base/Cleanse/CleanseEffect.prefab").WaitForCompletion().transform;
            CleanseEffect = CleanseTransform.GetComponent<EffectComponent>();
            var CleanseVisualConfig = Config.Bind("Item Effects", "Enable Blast Shower", true, "Enables Blast Shower's effects.");
            if (!CleanseVisualConfig.Value)
            {
                CleanseEffect.effectIndex = (EffectIndex)(-1);
                for (var i = 0; i < CleanseTransform.childCount; i++)
                    CleanseTransform.GetChild(i).gameObject.SetActive(false);
            }
            CleanseVisualConfig.SettingChanged += BlastShowerConfigHandler;
            if (ExistsRiskOfOptions) AddOption(CleanseVisualConfig);
        }
        catch
        {
            Logger.LogError("Could not hook onto Blast Shower.");
        }

        BindAsset("KillEliteFrenzy/NoCooldownEffect", "Enable Brainstalks", "Enables Brainstalks' screen effect. Note: re-enabling may not take effect until next stage.");

        // Interstellar Desk Plant
        try
        {
            var DeskplantIndicatorTransform = Addressables.LoadAsset<GameObject>("RoR2/Base/Plant/DeskplantWard.prefab").WaitForCompletion().transform.Find("Indicator").gameObject.transform;
            DeskplantSpores = DeskplantIndicatorTransform.Find("Spores").gameObject;
            DeskplantSymbols = DeskplantIndicatorTransform.Find("HealingSymbols").gameObject;
            DeskplantMushrooms = DeskplantIndicatorTransform.Find("MushroomMeshes").gameObject;
            var DeskPlantIndicatorConfig = Config.Bind("Item Effects", "Enable Desk Plant Ward Particles", true, "Enables the spore, plus sign, and mushroom visual effects from Interstellar Desk Plant's healing ward indicator. Does not affect the particle effects of the Desk Plant seed, or the perimeter sphere of the ward.");
            IdpVisualConfigHandler(DeskPlantIndicatorConfig);
            DeskPlantIndicatorConfig.SettingChanged += IdpVisualConfigHandler;
            if (ExistsRiskOfOptions) AddOption(DeskPlantIndicatorConfig);
        }
        catch
        {
            Logger.LogError("Could not hook onto Interstellar Desk Plant ward.");
        }

        try
        {
            IcicleAuraAimRequest = typeof(IcicleAuraController).GetField("aimRequest", (BindingFlags)36);
            On.RoR2.IcicleAuraController.OnIciclesActivated += FrelicActivationEventHandler;
            On.RoR2.IcicleAuraController.OnIcicleGained += FrelicGainedEventHandler;
            if (ExistsRiskOfOptions)
            {
                AddOption(FrostRelicFOVConfig);
                AddOption(FrostRelicParticlesConfig);
                AddOption(FrostRelicSoundConfig);
            }
        }
        catch
        {
            Logger.LogError("Could not hook onto Frost Relic.");
        }

        BindAsset("IgniteOnKill/IgniteExplosionVFX", "Enable Gasoline", "Enables Gasoline's explosion");

        // Kjaro's Band
        try
        {
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
            FireTornadoConfig.SettingChanged += KjaroVisualConfigHandler;
            if (ExistsRiskOfOptions) AddOption(FireTornadoConfig);
        }
        catch
        {
            Logger.LogError("Could not hook onto Kjaro's Band.");
        }

        BindAsset("FireballsOnHit/FireMeatBallGhost", "Enable Molten Perforator", "Enables the Molten Perforator visuals");

        // Runald's Band
        try
        {
            IceRingExplosionPrefab = Addressables.LoadAsset<GameObject>("RoR2/Base/ElementalRings/IceRingExplosion.prefab").WaitForCompletion();
            var IceRingExplosionConfig = Config.Bind("Item Effects", "Enable Runalds Band", true, "Enables Runald's Band's ice explosion.");
            if (!IceRingExplosionConfig.Value)
                IceRingExplosionDestructor = IceRingExplosionPrefab.AddComponent<DestroyOnUpdate>();
            var IceRingExplosionTransform = IceRingExplosionPrefab.transform;
            for (var i = 0; i < IceRingExplosionTransform.childCount; i++)
                IceRingExplosionTransform.GetChild(i).gameObject.SetActive(IceRingExplosionConfig.Value);
            IceRingExplosionConfig.SettingChanged += IceRingExplosionConfigHandler;
            if (ExistsRiskOfOptions) AddOption(IceRingExplosionConfig);
        }
        catch
        {
            Logger.LogError("Could not hook onto Runald's Band.");
        }

        BindAsset("BleedOnHitAndExplode/BleedOnHitAndExplode_Explosion", "Enable Shatterspleen", "Enables Shatterspleen's explosion");
        BindAsset("Tonic/TonicBuffEffect", "Enable Spinel Tonic", "Enables Spinel Tonic's screen effect");
        BindAsset("StickyBomb/StickyBombGhost", "Enable Sticky Bomb Drops", "Enables Sticky Bomb's drops");
        BindAsset("StickyBomb/BehemothVFX", "Enable Sticky Bomb Explosion", "Enables Sticky Bomb's explosion");
        BindAsset("ExplodeOnDeath/WilloWispExplosion", "Enable Will-o-the-Wisp", "Enables Will o' the Wisp's explosion");

        // Weeping Bungus
        try
        {
            var MushroomVoidEffectPrefab = Addressables.LoadAsset<GameObject>("RoR2/DLC1/MushroomVoid/MushroomVoidEffect.prefab").WaitForCompletion();
            MushroomVoidVisual = MushroomVoidEffectPrefab.GetComponent<TemporaryVisualEffect>();
            MushroomVoidAudio = MushroomVoidEffectPrefab.GetComponent<LoopSoundPlayer>();
            var WungusAudioConfig = Config.Bind("SOTV Item Effects", "Enable Weeping Fungus Sound", true, "Enables Weeping Fungus' sound effect. Take effect immediately.");
            var WungusVisualsConfig = Config.Bind("SOTV Item Effects", "Enable Weeping Fungus Visuals", true, "Enables Weeping Fungus' visual particle effects. This includes the floating plus symbols, the floating spore particles, and the void star particle effects. Does not affect the generic green healing pulsing effect. Note: re-enabling may not take effect until next stage.");
            MushroomVoidAudio.enabled = WungusAudioConfig.Value;
            MushroomVoidVisual.enabled = WungusVisualsConfig.Value;
            WungusAudioConfig.SettingChanged += WungusAudioConfigHandler;
            WungusVisualsConfig.SettingChanged += WungusVisualConfigHandler;
            if (ExistsRiskOfOptions)
            {
                AddOption(WungusAudioConfig);
                AddOption(WungusVisualsConfig);
            }
        }
        catch
        {
            Logger.LogError("Could not hook onto Wungus.");
        }

        // Plasma Shrimp
        try
        {
            var PlimpPrefab = Addressables.LoadAsset<GameObject>("RoR2/DLC1/MissileVoid/MissileVoidProjectile.prefab").WaitForCompletion();

            PlimpController = PlimpPrefab.GetComponent<ProjectileController>();
            //PlimpFlightSoundLoop = PlimpController.flightSoundLoop;
            //PlimpFlightSoundSoundName = PlimpController.flightSoundLoop.startSoundName;

            var PlimpAudioConfig = Config.Bind("SOTV Item Effects", "Enable Plasma Shrimp Sounds", true, "Sounds like bowling!");

            PlimpController.startSound = "Play_item_void_critGlasses";

            var lsd = PlimpController.GetComponent<LoopSoundDef>();
            if (lsd != null)
            {
                lsd.startSoundName = null;
            }

            //if (PlimpAudioConfig.Value)
            //{

            //    PlimpController.startSound = "Play_item_void_critGlasses";
            //    //PlimpController.flightSoundLoop.startSoundName = PlimpFlightSoundSoundName;

            //}
            //else
            //{
            //    PlimpController.startSound = null;
            //    //PlimpController.flightSoundLoop.startSoundName="";

            //}

            //PlimpAudioConfig.SettingChanged += plimpAudioToggle;
            //if (riskOfOptionsLoaded)
            //{
            //    addOption(PlimpAudioConfig);
            //}


        }
        catch { Logger.LogError("Couldn't load plimp"); }

        BindAsset("Titan/TitanDeathEffect", "Enable Titan Death Effect", "Enables Stone Titan's on-death explosion. Disabling will cause Stone Titans to disappear on death instead of creating a corpse.", "Character Effects");
        BindAsset("Vagrant/VagrantDeathExplosion", "Enable Vagrant Death Explosion", "Enables Wandering Vagrant's on-death explosion. Disabling will cause Wandering Vagrants to disappear on death instead of creating a corpse.", "Character Effects");


        BindVoidAsset("DLC1/MissileVoid/MissileVoid", "Enable MissileVoid", "foo description", "SOTV Item Effects");
        BindVoidAsset("DLC1/MissileVoid/MissileVoidGhost", "Enable MissileVoidGhost", "foo description", "SOTV Item Effects");
        BindVoidAsset("DLC1/MissileVoid/MissileVoidOrbEffect", "Enable MissileVoidOrbEffect", "foo description", "SOTV Item Effects");
        BindVoidAsset("DLC1/MissileVoid/MissileVoidProjectile", "Enable MissileVoidProjectile", "foo description", "SOTV Item Effects");
        BindVoidAsset("DLC1/MissileVoid/VoidImpactEffect", "Enable VoidImpactEffect", "foo description", "SOTV Item Effects");
        BindVoidAsset("DLC1/VoidMegaCrab/MissileVoidBigGhost", "Enable MissileVoidBigGhost", "foo description", "SOTV Item Effects");
        BindVoidAsset("DLC1/VoidMegaCrab/MissileVoidBigProjectile", "Enable MissileVoidBigProjectile", "foo description", "SOTV Item Effects");
        BindVoidAsset("DLC1/VoidMegaCrab/MissileVoidMuzzleflash", "Enable MissileVoidMuzzleflash", "foo description", "SOTV Item Effects");
    }

    [MethodImpl(768)]
    private void BlastShowerConfigHandler(object x, EventArgs _)
    {
        var y = ((ConfigEntry<bool>)x).Value;
        CleanseEffect.effectIndex = (EffectIndex)(y ? 102 : -1);
        for (var i = 0; i < CleanseTransform.childCount; i++)
            CleanseTransform.GetChild(i).gameObject.SetActive(y);
    }

    [MethodImpl(768)]
    private void IceRingExplosionConfigHandler(object x, EventArgs _)
    {
        var transform = IceRingExplosionPrefab.transform;
        var childCount = transform.childCount;
        var y = ((ConfigEntry<bool>)x).Value;
        if (y && IceRingExplosionDestructor != null)
        {
            UnityEngine.Object.Destroy(IceRingExplosionDestructor);
            IceRingExplosionDestructor = null;
        }
        else
            IceRingExplosionDestructor = IceRingExplosionPrefab.AddComponent<DestroyOnUpdate>();
        for (var i = 0; i < childCount; i++)
            transform.GetChild(i).gameObject.SetActive(y);
    }

    [MethodImpl(768)]
    private void KjaroVisualConfigHandler(object x, EventArgs _)
    {
        var y = ((ConfigEntry<bool>)x).Value;
        FireTornadoSmoke.SetActive(y);
        FireTornadoMeshCore.SetActive(y);
        FireTornadoMeshWide.SetActive(y);
        FireTornadoEmbers.SetActive(y);
        FireTornadoLight.SetActive(y);
        FireTornadoBurst.SetActive(y);
    }

    private void PlimpAudioConfigHandler(object x, EventArgs _)
    {
        var enabled = ((ConfigEntry<bool>)x).Value;
        if (enabled)
        {
            PlimpController.startSound = "Play_item_void_critGlasses";
        }
        else
        {
            PlimpController.startSound = "Stop_item_void_critGlasses";
        }
    }

    [MethodImpl(768)]
    private void WungusAudioConfigHandler(object x, EventArgs _) =>
    MushroomVoidAudio.enabled = ((ConfigEntry<bool>)x).Value;

    [MethodImpl(768)]
    private void WungusVisualConfigHandler(object x, EventArgs _) =>
      MushroomVoidVisual.enabled = ((ConfigEntry<bool>)x).Value;

    [MethodImpl(768)]
    private void IdpVisualConfigHandler(object x, EventArgs _ = null)
    {
        var y = ((ConfigEntry<bool>)x).Value;
        DeskplantSpores.SetActive(y);
        DeskplantSymbols.SetActive(y);
        DeskplantMushrooms.SetActive(y);
    }

    private void FrelicGainedEventHandler(On.RoR2.IcicleAuraController.orig_OnIcicleGained orig, IcicleAuraController self)
    {
        // WARN: the following code is probably illegal in your jurisdiction! Arrr matey!
        foreach (ParticleSystem part in self.procParticles)
            if (FrostRelicParticlesConfig.Value | part.name == "Area")
                part.Play();
    }

    private void FrelicActivationEventHandler(On.RoR2.IcicleAuraController.orig_OnIciclesActivated orig, IcicleAuraController self)
    {
        // WARN: the following code is probably illegal in your jurisdiction! Arrr matey!
        if (FrostRelicSoundConfig.Value)
            Util.PlaySound("Play_item_proc_icicle", self.gameObject);
        if (FrostRelicFOVConfig.Value)
        {
            var ctp = self.owner.GetComponent<CameraTargetParams>();
            if (ctp) IcicleAuraAimRequest.SetValue(self, ctp.RequestAimType(CameraTargetParams.AimType.Aura));
        }
        foreach (ParticleSystem part in self.auraParticles)
            if (FrostRelicParticlesConfig.Value | part.name == "Area")
            {
                var main = part.main;
                main.loop = true;
                part.Play();
            }
    }

    [MethodImpl(768)]
    private void BindVoidAsset(string assetPath, string title, string description, string section = "Item Effects")
    {
        try
        {
            var prefab = Addressables.LoadAsset<GameObject>("RoR2/" + assetPath + ".prefab").WaitForCompletion();
            var config = Config.Bind(section, title, true, description);
            // todo: should the following class be merged into this one?
            config.SettingChanged += (x, _) =>
              prefab.SetActive(((ConfigEntry<bool>)x).Value);
            prefab.SetActive(config.Value);
            if (ExistsRiskOfOptions) AddOption(config);
        }
        catch { }
    }

    [MethodImpl(768)]
    private void BindAsset(string assetPath, string title, string description, string section = "Item Effects")
    {
        try
        {
            var prefab = Addressables.LoadAsset<GameObject>("RoR2/Base/" + assetPath + ".prefab").WaitForCompletion();
            var config = Config.Bind(section, title, true, description);
            // todo: should the following class be merged into this one?
            config.SettingChanged += (x, _) =>
              prefab.SetActive(((ConfigEntry<bool>)x).Value);
            prefab.SetActive(config.Value);
            if (ExistsRiskOfOptions) AddOption(config);
        }
        catch { }


    }

    [MethodImpl(520)]
    private void AddOption(ConfigEntry<bool> _) =>
      ModSettingsManager.AddOption(new CheckBoxOption(_));

    [MethodImpl(520)]
    private void AddToRiskOfOptions()
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

public sealed class DestroyOnUpdate : MonoBehaviour
{
    public void Update() =>
      UnityEngine.Object.Destroy(base.gameObject);
}
