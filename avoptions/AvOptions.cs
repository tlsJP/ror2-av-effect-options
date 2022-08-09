using BepInEx;
using BepInEx.Configuration;
using com.thejpaproject.avoptions.configurations;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AddressableAssets;


namespace com.thejpaproject.avoptions
{
    [BepInPlugin("com.thejpaproject.AVFX_Options", "JP's AV Effect Options", "1.13.4")]
    [BepInDependency("com.rune580.riskofoptions", (BepInDependency.DependencyFlags)2)]
    public sealed class AvOptions : BaseUnityPlugin
    {


        private static readonly RiskOfOptions RiskOfOptions = RiskOfOptions.GetInstance();


        private BlastShowerConfiguration BlastShowerConfiguration;
        private FrelicAvConfiguration FrelicBaseConfiguration;
        private IdpVisualConfiguration IdpVisualConfiguration;
        private KjaroVisualConfiguration KjaroVisualConfiguration;
        private RunaldsVisualConfiguration RunaldsVisualConfiguration;
        private WungusVisualConfiguration WungusVisualConfiguration;
        private WungusAudioConfiguration WungusAudioConfiguration;
        private PlasmaShrimpConfiguration PlasmaShrimpConfiguration;


        [MethodImpl(768)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "RoR2 Mod Lifecycle Method")]
        private void Awake()
        {

            try
            {
                BlastShowerConfiguration = new BlastShowerConfiguration(Config);

                FrelicBaseConfiguration = new FrelicAvConfiguration(Config);
                IdpVisualConfiguration = new IdpVisualConfiguration(Config);
                KjaroVisualConfiguration = new KjaroVisualConfiguration(Config);

                PlasmaShrimpConfiguration = new PlasmaShrimpConfiguration(Config);
                RunaldsVisualConfiguration = new RunaldsVisualConfiguration(Config);

                WungusVisualConfiguration = new WungusVisualConfiguration(Config);
                WungusAudioConfiguration = new WungusAudioConfiguration(Config);

            }
            catch (ConfigurationException e)
            {
                Logger.LogError(String.Format("Failed to register configuration for {0}\n{1}", e.Message, e.StackTrace));
            }


            // Direct Base Bindings
            BindBaseAsset("BleedOnHitAndExplode/BleedOnHitAndExplode_Explosion", "Enable Shatterspleen", "Enables Shatterspleen's explosion");
            BindBaseAsset("ExplodeOnDeath/WilloWispExplosion", "Enable Will-o-the-Wisp", "Enables Will o' the Wisp's explosion");
            BindBaseAsset("FireballsOnHit/FireMeatBallGhost", "Enable Molten Perforator", "Enables the Molten Perforator visuals");
            BindBaseAsset("IgniteOnKill/IgniteExplosionVFX", "Enable Gasoline", "Enables Gasoline's explosion");
            BindBaseAsset("KillEliteFrenzy/NoCooldownEffect", "Enable Brainstalks", "Enables Brainstalks' screen effect. Note: re-enabling may not take effect until next stage.");
            BindBaseAsset("StickyBomb/StickyBombGhost", "Enable Sticky Bomb Drops", "Enables Sticky Bomb's drops");
            BindBaseAsset("StickyBomb/BehemothVFX", "Enable Sticky Bomb Explosion", "Enables Sticky Bomb's explosion");
            BindBaseAsset("Titan/TitanDeathEffect", "Enable Titan Death Effect", "Enables Stone Titan's on-death explosion. Disabling will cause Stone Titans to disappear on death instead of creating a corpse.", "Character Effects");
            BindBaseAsset("Tonic/TonicBuffEffect", "Enable Spinel Tonic", "Enables Spinel Tonic's screen effect");
            BindBaseAsset("Vagrant/VagrantDeathExplosion", "Enable Vagrant Death Explosion", "Enables Wandering Vagrant's on-death explosion. Disabling will cause Wandering Vagrants to disappear on death instead of creating a corpse.", "Character Effects");

            // Direct Void Bindings
            //BindVoidAsset("MissileVoid/MissileVoidGhost", "Enable PlimpGhost", "Pew pew", "SOTV Item Effects");
            BindVoidAsset("MissileVoid/MissileVoidOrbEffect", "Enable Plasma Shrimp Orbs", "It's the missiles that shoot out", "SOTV Item Effects");
            //BindVoidAsset("MissileVoid/MissileVoidProjectile", "Enable PlimpProjectile", "Pew pew", "SOTV Item Effects"); 
            //BindVoidAsset("MissileVoid/VoidImpactEffect", "Enable VoidImpactEffect", "Pew pew", "SOTV Item Effects");
            //BindVoidAsset("VoidMegaCrab/MissileVoidBigGhost", "Enable PlimpBigGhost", "Pew pew", "SOTV Item Effects");
            //BindVoidAsset("VoidMegaCrab/MissileVoidBigProjectile", "Enable PlimpBigProjectile", "Pew pew", "SOTV Item Effects");
            //BindVoidAsset("VoidMegaCrab/MissileVoidMuzzleflash", "Enable PlimpMuzzleflash", "Pew pew", "SOTV Item Effects");
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
}