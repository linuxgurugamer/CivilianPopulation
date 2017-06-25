using System;
using System.Collections.Generic;

namespace CivilianPopulation
{
    class CivilianVessel
    {
        private Vessel vessel;
        private bool populated;

        public CivilianVessel(Vessel vessel)
        {
            this.vessel = vessel;
            foreach (ProtoCrewMember crew in this.vessel.GetVesselCrew())
            {
                if (crew.trait == "Civilian")
                {
                    this.populated = true;
                }
            }
        }

        public string getName()
        {
            return this.vessel.vesselName;
        }

        public List<ProtoCrewMember> getCrew()
        {
            return this.vessel.GetVesselCrew();
        }

        public bool isPopulated()
        {
            return this.populated;
        }
    }
}