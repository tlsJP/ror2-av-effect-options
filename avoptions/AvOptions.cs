using BepInEx;
using com.thejpaproject.avoptions.configurations;
using System.Runtime.CompilerServices;

namespace com.thejpaproject.avoptions
{
  [BepInPlugin("com.thejpaproject.AVFX_Options", "JP's AV Effect Options", "2.0.0")]
  [BepInDependency("com.rune580.riskofoptions", (BepInDependency.DependencyFlags)2)]
  public sealed class AvOptions : BaseUnityPlugin
  {

    private PrefabConfigurer _prefabConfigurer;

    private BlastShowerConfiguration _blastShowerConfiguration;
    private FireworkTailConfiguration _fireworkTailConfiguration;
    private FrelicAvConfiguration _frelicAvConfiguration;
    private IdpVisualConfiguration _idpVisualConfiguration;
    private KjaroVisualConfiguration _kjaroVisualConfiguration;
    private PlasmaShrimpConfiguration _plasmaShrimpConfiguration;
    private RoseBucklerConfiguration _roseBucklerConfiguration;
    private RunaldsVisualConfiguration _runaldsVisualConfiguration;
    private ViendAudioConfiguration _viendAudioConfiguration;
    private WungusVisualConfiguration _wungusVisualConfiguration;
    private WungusAudioConfiguration _wungusAudioConfiguration;
    private BortalConfiguration _bortalConfiguration;

    [MethodImpl(768)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "RoR2 Mod Lifecycle Method")]
    private void Awake()
    {

      _prefabConfigurer = PrefabConfigurer.GetInstance(Config);

      try
      {

        _blastShowerConfiguration = new BlastShowerConfiguration(Config);
        _bortalConfiguration = new BortalConfiguration(Config);
        _prefabConfigurer.BindVfx("KillEliteFrenzy/NoCooldownEffect", "Brainstalks", "Enables Brainstalks' screen effect. Note: re-enabling may not take effect until next stage.");

        _fireworkTailConfiguration = new FireworkTailConfiguration(Config);
        _frelicAvConfiguration = new FrelicAvConfiguration(Config);

        _prefabConfigurer.BindVfx("IgniteOnKill/IgniteExplosionVFX", "Gasoline", "Enables Gasoline's explosion");

        _idpVisualConfiguration = new IdpVisualConfiguration(Config);

        _kjaroVisualConfiguration = new KjaroVisualConfiguration(Config);

        _prefabConfigurer.BindVfx("FireballsOnHit/FireMeatBallGhost", "Molten Perforator", "Enables the Molten Perforator visuals");

        _plasmaShrimpConfiguration = new PlasmaShrimpConfiguration(Config);
        _prefabConfigurer.BindVoidVfx("MissileVoid/MissileVoidOrbEffect", "Plasma Shrimp Orbs", "It's the missiles that shoot out");

        _roseBucklerConfiguration = new RoseBucklerConfiguration(Config);
        _runaldsVisualConfiguration = new RunaldsVisualConfiguration(Config);

        _prefabConfigurer.BindVfx("BleedOnHitAndExplode/BleedOnHitAndExplode_Explosion", "Shatterspleen", "Enables Shatterspleen's explosion");
        _prefabConfigurer.BindVfx("Tonic/TonicBuffEffect", "Spinel Tonic", "Enables Spinel Tonic's screen effect");
        _prefabConfigurer.BindVfx("StickyBomb/StickyBombGhost", "Sticky Bomb Drops", "Enables Sticky Bomb's drops");
        _prefabConfigurer.BindVfx("StickyBomb/BehemothVFX", "Sticky Bomb Explosion", "Enables Sticky Bomb's explosion");

        _prefabConfigurer.BindAsset(Category.UNIT_EFFECTS, "Base/Titan/TitanDeathEffect", "Titan Death", "Enables Stone Titan's on-death explosion. Disabling will cause Stone Titans to disappear on death instead of creating a corpse.");

        _viendAudioConfiguration = new ViendAudioConfiguration(Config);

        _prefabConfigurer.BindAsset(Category.UNIT_EFFECTS, "Base/Vagrant/VagrantDeathExplosion", "Wandering Vagrant Death", "Enables Wandering Vagrant's on-death explosion. Disabling will cause Wandering Vagrants to disappear on death instead of creating a corpse.");
        _prefabConfigurer.BindVfx("ExplodeOnDeath/WilloWispExplosion", "Will-o-the-Wisp", "Enables Will o' the Wisp's explosion");
        _wungusVisualConfiguration = new WungusVisualConfiguration(Config);
        _wungusAudioConfiguration = new WungusAudioConfiguration(Config);

      }
      catch (ConfigurationException e)
      {
        Logger.LogError(e);
        Logger.LogError($"Failed to register configuration for {e.Message}\r{e.StackTrace}");
      }

      Logger.LogInfo("Finished!");

    }

  }
}