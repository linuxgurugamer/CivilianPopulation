using UnityEngine;

namespace CivilianPopulation.Infra
{
    public class CivilianPopulationHousingModule : PartModule
    {
        [KSPField(isPersistant = true, guiActive = false)]
        public bool activated;

        public void Start()
        {
            // log(" - Start !");
        }

        public void Update()
        {
            Events["activate"].active = !activated;
            Events["deactivate"].active = activated;
        }

        public void FixedUpdate()
        {
            // log(" - FixedUpdate !");
        }

        [KSPField(guiName = "Breeding Status:", isPersistant = true, guiActive = true, guiActiveEditor = false)]
        public string breedingStatus = "Not allowed";

        [KSPEvent(guiName = "Allow breeding", active = true, guiActive = true, guiActiveEditor = false)]
        public void activate()
        {
            this.getCivilianPopulationVesselModule().setAllowBreeding(true);
            this.activated = true;
            breedingStatus = "Allowed";
        }

        [KSPEvent(guiName = "Forbid breeding", active = false, guiActive = true)]
        public void deactivate()
        {
            this.getCivilianPopulationVesselModule().setAllowBreeding(false);
            this.activated = false;
            breedingStatus = "Not Allowed";

        }

        public bool isActivated()
        {
            return this.activated;
        }

        private CivilianPopulationVesselModule getCivilianPopulationVesselModule()
        {
            foreach (VesselModule module in vessel.vesselModules)
            {
                if (module.GetType() == typeof(CivilianPopulationVesselModule))
                {
                    CivilianPopulationVesselModule civPopModule = (CivilianPopulationVesselModule)module;
                    return civPopModule;
                }
            }
            return null;
        }

        private void log(string message)
        {
            Debug.Log(this.GetType().Name + message);
        }

    }
}
