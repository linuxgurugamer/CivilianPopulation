using System;
using System.Collections.Generic;
using UnityEngine;

namespace CivilianPopulation.Infra
{
    public class CivilianPopulationVesselModule : VesselModule
    {

		[KSPField(isPersistant = true, guiActive = false)]
		public bool hasCivilianDocks;
		[KSPField(isPersistant = true, guiActive = false)]
		public int capacity;

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
                this.capacity = 0;
                foreach(CivilianPopulationDockModule dock in docks)
                {
                    if (dock.isActivated())
                    {
                        capacity += dock.part.CrewCapacity - dock.part.protoModuleCrew.Count;
					}
                }
            }
		}

        public int getCapacity()
        {
            return this.capacity;
        }

        private void log(string message)
        {
            Debug.Log(this.GetType().Name + message);
        }
    }
}
