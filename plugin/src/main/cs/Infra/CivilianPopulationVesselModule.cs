using System;
using System.Collections.Generic;
using UnityEngine;

namespace CivilianPopulation.Infra
{
    public class CivilianPopulationVesselModule : VesselModule
    {

        [KSPField(isPersistant = true, guiActive = false)]
        public bool hasCivilianDocks;
        
		public CivilianPopulationVesselModule()
        {
        }

        public void Update()
        {
            //log(" - Update !");
        }

        public void FixedUpdate()
        {
            List<CivilianPopulationDockModule> docks = vessel.FindPartModulesImplementing<CivilianPopulationDockModule>();
            if (docks.Count > 0)
            {
                this.hasCivilianDocks = true;
            }
		}

        private void log(string message)
        {
            Debug.Log(this.GetType().Name + message);
        }
    }
}
