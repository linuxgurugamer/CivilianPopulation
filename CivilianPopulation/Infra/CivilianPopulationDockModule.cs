using UnityEngine;

namespace CivilianPopulation.Infra
{
    public class CivilianPopulationDockModule : PartModule
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

        [KSPEvent(guiName = "Allow contractors", active = true, guiActive = true)]
        public void activate()
        {
            this.getCivilianPopulationVesselModule().setAllowDocking(true);
            this.activated = true;
        }

        [KSPEvent(guiName = "Refuse contractors", active = false, guiActive = true)]
        public void deactivate()
        {
            this.getCivilianPopulationVesselModule().setAllowDocking(false);
            this.activated = false;
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
