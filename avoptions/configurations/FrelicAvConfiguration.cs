using BepInEx.Configuration;
using RoR2;
using System.Reflection;
using UnityEngine;

namespace com.thejpaproject.avoptions.configurations
{
    internal class FrelicAvConfiguration
    {
        private readonly RiskOfOptions riskOfOptions = RiskOfOptions.Instance;

        private readonly ConfigEntry<bool> soundEnabled;
        private readonly ConfigEntry<bool> fovEnabled;
        private readonly ConfigEntry<bool> particlesEnabled;

        private readonly FieldInfo IcicleAuraAimRequest;

        internal FrelicAvConfiguration(ConfigFile configFile)
        {
            var fovDesc = "Enables the temporary FOV change that Frost Relic's on-kill proc gives. " +
                "Does not affect the particle effects (see the Frost Relic Particles option)." +
                "\n\nEffective immediately";
            fovEnabled = configFile.Bind("Item Effects", "Enable Frost Relic FOV", true, fovDesc);

            var particleDesc = "Enables the chunk and ring effects of Frost Relic. " +
                "Does not affect the spherical area effect that indicates the item's area of effect, or the floating ice crystal that follows characters with the Frost Relic item." +
                "\n\nEffective immediately";
            particlesEnabled = configFile.Bind("Item Effects", "Enable Frost Relic Particles", true, particleDesc);

            soundEnabled = configFile.Bind("Item Effects", "Enable Frost Relic Sound", true, "Enables the sound effects of Frost Relic's on-kill proc.\n\nEffective immediately");

            var bindingFlags = BindingFlags.NonPublic | BindingFlags.Instance;
            IcicleAuraAimRequest = typeof(IcicleAuraController).GetField("aimRequest", bindingFlags);

            On.RoR2.IcicleAuraController.OnIcicleGained += FrelicGainedEventHandler;
            On.RoR2.IcicleAuraController.OnIciclesActivated += FrelicActivationEventHandler;

            riskOfOptions.AddOption(fovEnabled);
            riskOfOptions.AddOption(particlesEnabled);
            riskOfOptions.AddOption(soundEnabled);
        }

        private void FrelicActivationEventHandler(On.RoR2.IcicleAuraController.orig_OnIciclesActivated _, IcicleAuraController self)
        {
            if (soundEnabled.Value)
                Util.PlaySound("Play_item_proc_icicle", self.gameObject);

            if (fovEnabled.Value)
            {
                var ctp = self.owner.GetComponent<CameraTargetParams>();
                if (ctp) IcicleAuraAimRequest.SetValue(self, ctp.RequestAimType(CameraTargetParams.AimType.Aura));
            }

            foreach (ParticleSystem part in self.auraParticles)
                if (particlesEnabled.Value | part.name == "Area")
                {
                    var main = part.main;
                    main.loop = true;
                    part.Play();
                }
        }

        private void FrelicGainedEventHandler(On.RoR2.IcicleAuraController.orig_OnIcicleGained orig, IcicleAuraController self)
        {
            foreach (ParticleSystem part in self.procParticles)
                if (particlesEnabled.Value | part.name == "Area")
                    part.Play();
        }

    }
}
