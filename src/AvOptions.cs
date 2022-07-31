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


namespace com.thejpaproject.avoptions
{
    [BepInPlugin("com.thejpaproject.AVFX_Options", "JP's AV Effect Options", "1.13.1")]
    [BepInDependency("com.rune580.riskofoptions", (BepInDependency.DependencyFlags)2)]
    public sealed class AvOptions : BaseUnityPlugin
    {

        private static readonly RiskOfOptions RiskOfOptions = new();

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

        private static ProjectileController PlimpController;
        private static EffectComponent MissileVoidOrbEffect;


        [MethodImpl(768)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "RoR2 Mod Lifecycle Method")]
        private void Awake()
        {
            FrostRelicFOVConfig = Config.Bind("Item Effects", "Enable Frost Relic FOV", true, "Enables the temporary FOV change that Frost Relic's on-kill proc gives. Does not affect the particle effects (see the Frost Relic Particles option).");
            FrostRelicParticlesConfig = Config.Bind("Item Effects", "Enable Frost Relic Particles", true, "Enables the chunk and ring effects of Frost Relic. Does not affect the spherical area effect that indicates the item's area of effect, or the floating ice crystal that follows characters with the Frost Relic item.");
            FrostRelicSoundConfig = Config.Bind("Item Effects", "Enable Frost Relic Sound", true, "Enables the sound effects of Frost Relic's on-kill proc.");


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
                CleanseVisualConfig.SettingChanged += BlastShowerConfigEventHandler;
                RiskOfOptions.AddOption(CleanseVisualConfig);
            }
            catch
            {
                Logger.LogError("Could not hook onto Blast Shower.");
            }

            BindBaseAsset("KillEliteFrenzy/NoCooldownEffect", "Enable Brainstalks", "Enables Brainstalks' screen effect. Note: re-enabling may not take effect until next stage.");

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
                RiskOfOptions.AddOption(DeskPlantIndicatorConfig);
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

                RiskOfOptions.AddOption(FrostRelicFOVConfig);
                RiskOfOptions.AddOption(FrostRelicParticlesConfig);
                RiskOfOptions.AddOption(FrostRelicSoundConfig);

            }
            catch
            {
                Logger.LogError("Could not hook onto Frost Relic.");
            }

            BindBaseAsset("IgniteOnKill/IgniteExplosionVFX", "Enable Gasoline", "Enables Gasoline's explosion");

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
                FireTornadoConfig.SettingChanged += KjaroVisualConfigEventHandler;
                RiskOfOptions.AddOption(FireTornadoConfig);
            }
            catch
            {
                Logger.LogError("Could not hook onto Kjaro's Band.");
            }

            BindBaseAsset("FireballsOnHit/FireMeatBallGhost", "Enable Molten Perforator", "Enables the Molten Perforator visuals");

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
                IceRingExplosionConfig.SettingChanged += IceRingExplosionConfigEventHandler;
                RiskOfOptions.AddOption(IceRingExplosionConfig);
            }
            catch
            {
                Logger.LogError("Could not hook onto Runald's Band.");
            }

            BindBaseAsset("BleedOnHitAndExplode/BleedOnHitAndExplode_Explosion", "Enable Shatterspleen", "Enables Shatterspleen's explosion");
            BindBaseAsset("Tonic/TonicBuffEffect", "Enable Spinel Tonic", "Enables Spinel Tonic's screen effect");
            BindBaseAsset("StickyBomb/StickyBombGhost", "Enable Sticky Bomb Drops", "Enables Sticky Bomb's drops");
            BindBaseAsset("StickyBomb/BehemothVFX", "Enable Sticky Bomb Explosion", "Enables Sticky Bomb's explosion");
            BindBaseAsset("ExplodeOnDeath/WilloWispExplosion", "Enable Will-o-the-Wisp", "Enables Will o' the Wisp's explosion");

            // Weeping Bungus
            try
            {
                var mushroomVoidEffectPrefab = Addressables.LoadAsset<GameObject>("RoR2/DLC1/MushroomVoid/MushroomVoidEffect.prefab").WaitForCompletion();

                MushroomVoidVisual = mushroomVoidEffectPrefab.GetComponent<TemporaryVisualEffect>();
                var wungusVisualsConfig = Config.Bind("SOTV Item Effects", "Enable Weeping Fungus Visuals", true, "Enables Weeping Fungus' visual particle effects. This includes the floating plus symbols, the floating spore particles, and the void star particle effects. Does not affect the generic green healing pulsing effect. Note: re-enabling may not take effect until next stage.");
                MushroomVoidVisual.enabled = wungusVisualsConfig.Value;
                wungusVisualsConfig.SettingChanged += WungusVisualConfigHandler;
                RiskOfOptions.AddOption(wungusVisualsConfig);

                MushroomVoidAudio = mushroomVoidEffectPrefab.GetComponent<LoopSoundPlayer>();
                var wungusAudioConfig = Config.Bind("SOTV Item Effects", "Enable Weeping Fungus Sound", true, "Enables Weeping Fungus' sound effect. Take effect immediately.");
                MushroomVoidAudio.enabled = wungusAudioConfig.Value;
                wungusAudioConfig.SettingChanged += WungusAudioConfigEventHandler;
                RiskOfOptions.AddOption(wungusAudioConfig);

            }
            catch
            {
                Logger.LogError("Could not hook onto Wungus.");
            }

            // Plasma Shrimp
            try
            {
                var plimpAudioConfig = Config.Bind("SOTV Item Effects", "Enable Plasma Shrimp Sounds", true, "Sounds like bowling! \nRequires restart to take effect :(");

                var plimpPrefab = Addressables.LoadAsset<GameObject>("RoR2/DLC1/MissileVoid/MissileVoidProjectile.prefab").WaitForCompletion();
                PlimpController = plimpPrefab.GetComponent<ProjectileController>();

                var missileVoidOrbEffectPrefab = Addressables.LoadAsset<GameObject>("RoR2/DLC1/MissileVoid/MissileVoidOrbEffect.prefab").WaitForCompletion();
                MissileVoidOrbEffect = missileVoidOrbEffectPrefab.GetComponent<EffectComponent>();



                if (plimpAudioConfig.Value)
                {
                    PlimpController.startSound = "Play_item_void_critGlasses";
                    MissileVoidOrbEffect.soundName = "Play_item_void_critGlasses";
                }
                else
                {
                    PlimpController.startSound = "";
                    MissileVoidOrbEffect.soundName = "";
                }

                plimpAudioConfig.SettingChanged += PlimpAudioConfigEventHandler;

                RiskOfOptions.AddOption(plimpAudioConfig);



            }
            catch { Logger.LogError("Couldn't load plimp"); }

            BindBaseAsset("Titan/TitanDeathEffect", "Enable Titan Death Effect", "Enables Stone Titan's on-death explosion. Disabling will cause Stone Titans to disappear on death instead of creating a corpse.", "Character Effects");
            BindBaseAsset("Vagrant/VagrantDeathExplosion", "Enable Vagrant Death Explosion", "Enables Wandering Vagrant's on-death explosion. Disabling will cause Wandering Vagrants to disappear on death instead of creating a corpse.", "Character Effects");

            
            BindVoidAsset("MissileVoid/MissileVoidGhost", "Enable PlimpGhost", "Pew pew", "SOTV Item Effects");
            BindVoidAsset("MissileVoid/MissileVoidOrbEffect", "Enable PlimpOrbEffect", "Pew pew", "SOTV Item Effects");
            BindVoidAsset("MissileVoid/MissileVoidProjectile", "Enable PlimpProjectile", "Pew pew", "SOTV Item Effects");
            BindVoidAsset("MissileVoid/VoidImpactEffect", "Enable VoidImpactEffect", "Pew pew", "SOTV Item Effects");
            //BindVoidAsset("VoidMegaCrab/MissileVoidBigGhost", "Enable PlimpBigGhost", "Pew pew", "SOTV Item Effects");
            //BindVoidAsset("VoidMegaCrab/MissileVoidBigProjectile", "Enable PlimpBigProjectile", "Pew pew", "SOTV Item Effects");
            //BindVoidAsset("VoidMegaCrab/MissileVoidMuzzleflash", "Enable PlimpMuzzleflash", "Pew pew", "SOTV Item Effects");
        }

        [MethodImpl(768)]
        private void BlastShowerConfigEventHandler(object x, EventArgs _)
        {
            var y = ((ConfigEntry<bool>)x).Value;
            CleanseEffect.effectIndex = (EffectIndex)(y ? 102 : -1);
            for (var i = 0; i < CleanseTransform.childCount; i++)
                CleanseTransform.GetChild(i).gameObject.SetActive(y);
        }

        [MethodImpl(768)]
        private void IceRingExplosionConfigEventHandler(object x, EventArgs _)
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
        private void KjaroVisualConfigEventHandler(object x, EventArgs _)
        {
            var y = ((ConfigEntry<bool>)x).Value;
            FireTornadoSmoke.SetActive(y);
            FireTornadoMeshCore.SetActive(y);
            FireTornadoMeshWide.SetActive(y);
            FireTornadoEmbers.SetActive(y);
            FireTornadoLight.SetActive(y);
            FireTornadoBurst.SetActive(y);
        }

        private void PlimpAudioConfigEventHandler(object x, EventArgs _)
        {
            var enabled = ((ConfigEntry<bool>)x).Value;
            if (enabled)
            {
                PlimpController.startSound = "Play_item_void_critGlasses";
                MissileVoidOrbEffect.soundName = "Play_item_void_critGlasses";
            }
            else
            {
                PlimpController.startSound = "";
                MissileVoidOrbEffect.soundName = "";
            }
        }

        [MethodImpl(768)]
        private void WungusAudioConfigEventHandler(object x, EventArgs _) =>
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
        private void BindVoidAsset(string assetPath, string title, string description, string section = "Item Effects") => BindAsset("DLC1/" + assetPath, title, description, section);

        [MethodImpl(768)]
        private void BindBaseAsset(string assetPath, string title, string description, string section = "Item Effects") => BindAsset("Base/" + assetPath, title, description, section);

        private void BindAsset(string assetPath, string title, string description, string section = "Item Effects")
        {
            try
            {
                var prefab = Addressables.LoadAsset<GameObject>("RoR2/" + assetPath + ".prefab").WaitForCompletion();
                var config = Config.Bind(section, title, true, description);
                config.SettingChanged += (x, _) => prefab.SetActive(((ConfigEntry<bool>)x).Value);
                prefab.SetActive(config.Value);
                RiskOfOptions.AddOption(config);
            }
            catch { }
        }
    }

    public sealed class DestroyOnUpdate : MonoBehaviour
    {
        public void Update() => UnityEngine.Object.Destroy(base.gameObject);
    }
}