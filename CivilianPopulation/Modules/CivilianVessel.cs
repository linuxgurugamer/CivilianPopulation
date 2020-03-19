using System.Collections.Generic;

namespace CivilianPopulation
{
    public class CivilianVessel
    {
        private Vessel vessel;
        private int civilianCount;

        public CivilianVessel(Vessel vessel)
        {
            this.vessel = vessel;
            this.civilianCount = 0;
            foreach (ProtoCrewMember crew in this.vessel.GetVesselCrew())
            {
                if (crew.trait == "Civilian")
                {
                    civilianCount++;
                }
            }
        }

        public string getName()
        {
            return this.vessel.vesselName;
        }

        public int getCivilianCount()
        {
            return civilianCount;
        }

        public List<ProtoCrewMember> getCrew()
        {
            return this.vessel.GetVesselCrew();
        }
    }
}