

using BepInEx.Configuration;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace com.thejpaproject.avoptions.configurations
{
    internal class RoseBucklerConfiguration : AvConfiguration
    {

        private static AkEvent s_event;
        private AK.Wwise.Event _enabledEvent;


        public RoseBucklerConfiguration(ConfigFile configFile) :
            base(configFile, Category.BASE_SFX, "Rose Buckler SFX", "Buckler noise when you start sprinting")
        { }

        private protected override void HandleEvent(object x, EventArgs args)
        {
            var enabled = ((ConfigEntry<bool>)x).Value;

            if (!enabled)
            {
                s_event.data = null;
            }
            else
            {
                s_event.data = _enabledEvent;
            }

        }

        private protected override void SetBehavior()
        {

            var prefab = Addressables.LoadAsset<GameObject>("RoR2/Base/SprintArmor/BucklerDefense.prefab").WaitForCompletion();
            s_event = prefab.GetComponent<AkEvent>();

            _enabledEvent = s_event.data;

        }
    }
}
