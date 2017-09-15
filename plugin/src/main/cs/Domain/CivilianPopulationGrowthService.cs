using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CivilianPopulation.Domain
{
    public class CivilianPopulationGrowthService
    {
        private const double DAY_IN_SECONDS = 60 * 60 * 6;

        private double lastUpdate;

        public CivilianPopulationGrowthService()
        {
            this.lastUpdate = -1;
        }

        public void update(double currentDate, CivilianVessel vessel)
        {
            if (lastUpdate != -1 && getDay(lastUpdate) < getDay(currentDate))
            {
                doUpdate(vessel);
            }
            lastUpdate = currentDate;
        }

        protected virtual void doUpdate(CivilianVessel vessel)
        {
            // select males
            List<CivilianKerbal> males = vessel.getMales();
            // select females
            List<CivilianKerbal> females = vessel.getFemales();
            // make couples
            List<CivilianKerbalCouple> couples = makeCouples(males, females);
            // change females state
            changeFemaleState(couples);
            // birth
            birthOfNewCivilans(females);
        }

        private List<CivilianKerbalCouple> makeCouples(List<CivilianKerbal> males, List<CivilianKerbal> females)
        {
            throw new NotImplementedException();
        }

        private void changeFemaleState(List<CivilianKerbalCouple> couples)
        {
            throw new NotImplementedException();
        }

        private void birthOfNewCivilans(List<CivilianKerbal> females)
        {
            throw new NotImplementedException();
        }

        private double getDay(double date)
        {
            return Math.Floor(date / DAY_IN_SECONDS);
        }
    }
}
