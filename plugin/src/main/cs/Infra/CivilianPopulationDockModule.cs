using System;
using UnityEngine;

namespace CivilianPopulation.Infra
{
    public class CivilianPopulationDockModule : PartModule
    {
        private const double DELIVERY_DELAY = 60 * 60 * 6;

        [KSPField(isPersistant = true, guiActive = false)]
        public bool activated;

        public void Start()
        {
            // log(" - Start !");
        }

        public void Update()
        {
            // log(" - Update !");
        }

        public void FixedUpdate()
        {
            // log(" - FixedUpdate !");
			foreach (VesselModule module in vessel.vesselModules)
            {
                if (module.GetType() == typeof(CivilianPopulationVesselModule))
                {
					CivilianPopulationVesselModule civPopModule = (CivilianPopulationVesselModule)module;
                    if (civPopModule.waiting > 0) 
                    {
                        log(" - spawn " + civPopModule.waiting + " civilian(s).");
                        civPopModule.waiting = 0;
                    }
                }
            }
        }

        [KSPEvent(guiName = "Activate", active = true, guiActive = true)]
        public void activate()
        {
            log(" - Activate !");
            this.activated = true;

			foreach (VesselModule module in vessel.vesselModules)
			{
				if (module.GetType() == typeof(CivilianPopulationVesselModule))
				{
					CivilianPopulationVesselModule civPopModule = (CivilianPopulationVesselModule)module;
                    civPopModule.setDeliveryDate(Planetarium.GetUniversalTime() + DELIVERY_DELAY);
				}
			}
		}

        [KSPEvent(guiName = "Deactivate", active = true, guiActive = true)]
        public void deactivate()
        {
            log(" - Deactivate !");
            this.activated = false;
        }

        public bool isActivated() 
        {
            return this.activated;
        }

        private void log(string message)
        {
            Debug.Log(this.GetType().Name + message);
        }
    }
}
