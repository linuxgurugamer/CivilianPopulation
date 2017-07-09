using System;
using System.Collections.Generic;

namespace CivilianPopulation.Domain
{
    public class CivilianPopulationWorld
    {
		private double universalTime;
        private List<CivilianVessel> vessels;

        public CivilianPopulationWorld(bool career, double universalTime, List<CivilianVessel> vessels)
        {
            this.universalTime = universalTime;
            this.vessels = vessels;
        }

        public double getUniversalTime()
        {
			return universalTime;
		}

        internal int getCiviliansInFlight()
        {
            int res = 0;
            foreach (CivilianVessel vessel in this.getVessels()) {
                res += vessel.getCivilianCount();
            }
            return res;
		}

        public List<CivilianVessel> getVessels()
        {
            return vessels;
        }
    }
}