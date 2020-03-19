using System.Collections.Generic;
using UnityEngine;

namespace CivilianPopulation.Infra
{
    public class CivilianPopulationVesselModule : VesselModule
    {
        [KSPField(isPersistant = true, guiActive = false)]
        public int capacity;
        [KSPField(isPersistant = true, guiActive = false)]
        public bool allowDocking;
        [KSPField(isPersistant = true, guiActive = false)]
        public bool allowBreeding;

        new public void Start()
        {
        }

        public void Update()
        {
        }

        public void FixedUpdate()
        {
            if (FlightGlobals.ActiveVessel != null && FlightGlobals.ActiveVessel.id == vessel.id)
            {
                this.capacity = updateVesselCapacity();
            }
        }

        private int updateVesselCapacity()
        {
            int res = 0;
            List<CivilianPopulationHousingModule> houses = FlightGlobals.ActiveVessel.FindPartModulesImplementing<CivilianPopulationHousingModule>();
            if (houses.Count > 0)
            {
                foreach (CivilianPopulationHousingModule house in houses)
                {
                    res += house.part.CrewCapacity;
                }
            }
            return res;
        }

        public bool isAllowDocking()
        {
            return allowDocking;
        }

        public void setAllowDocking(bool allow)
        {
            this.allowDocking = allow;
            List<CivilianPopulationDockModule> docks = FlightGlobals.ActiveVessel.FindPartModulesImplementing<CivilianPopulationDockModule>();
            foreach (CivilianPopulationDockModule dock in docks)
            {
                dock.activated = allow;
            }
        }

        public bool isAllowBreeding()
        {
            return allowBreeding;
        }

        public void setAllowBreeding(bool allow)
        {
            this.allowBreeding = allow;
            List<CivilianPopulationHousingModule> houses = FlightGlobals.ActiveVessel.FindPartModulesImplementing<CivilianPopulationHousingModule>();
            foreach (CivilianPopulationHousingModule house in houses)
            {
                house.activated = allow;
            }
        }

        private void log(string message)
        {
            Debug.Log(this.GetType().Name + " - " + message);
        }
    }
}



