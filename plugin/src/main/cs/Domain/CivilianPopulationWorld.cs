using System;
using System.Collections.Generic;

namespace CivilianPopulation.Domain
{
    public class CivilianPopulationWorld
    {
		private bool career;
		private double universalTime;
        private List<CivilianVessel> vessels;

        public CivilianPopulationWorld(bool career, double universalTime, List<CivilianVessel> vessels)
        {
            this.career = career;
            this.universalTime = universalTime;
            this.vessels = vessels;
        }

		internal bool isCareer()
        {
            return career;
        }

        internal double getUniversalTime()
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

        internal List<CivilianVessel> getVessels()
        {
            return vessels;
        }
    }
}