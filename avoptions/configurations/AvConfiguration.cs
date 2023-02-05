
using BepInEx.Configuration;
using BepInEx.Logging;
using System;

namespace com.thejpaproject.avoptions.configurations
{

    static class Category
    {
        public static readonly String BASE_VFX = "VFX";
        public static readonly String BASE_SFX = "SFX";
        public static readonly String VOID_VFX = "SOTV VFX";
        public static readonly String VOID_SFX = "SOTV SFX";
        public static readonly String UNIT_EFFECTS = "Unit FX";

    }

    abstract class AvConfiguration
    {
        private protected ConfigEntry<bool> _configEntry;
        private static readonly RiskOfOptions s_riskOfOptions = RiskOfOptions.Instance;
        private protected ManualLogSource _logger;
        private protected String _name;

        private protected AvConfiguration(ConfigFile configFile, string category, string key, string description, bool defaultSetting = true)
        {
            _name = $"c.t.a.c.{GetType().Name}";
            _logger = BepInEx.Logging.Logger.CreateLogSource(_name);
            try
            {
                _configEntry = configFile.Bind(category, key, defaultSetting, description);
                SetBehavior();
                HandleEvent(_configEntry, null);
                _configEntry.SettingChanged += HandleEvent;
                s_riskOfOptions.AddOption(_configEntry);
                _logger.LogDebug("Configuration completed.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                throw new ConfigurationException(_name, ex);
            }
        }

        private protected abstract void HandleEvent(object x, EventArgs args);

        private protected abstract void SetBehavior();

    }


}
