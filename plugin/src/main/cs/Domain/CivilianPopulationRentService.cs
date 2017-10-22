using System;
using System.Collections.Generic;

namespace CivilianPopulation.Domain
{
    public class CivilianPopulationRentService
    {
		private const double timeBetweenRents = 6*60*60;
		private const int rentAmountPerCivilian = 200;

		private Action<int> addFunds;

		private double nextTaxesDate;

        public CivilianPopulationRentService(Action<int> addFunds)
        {
            this.addFunds = addFunds;
        }

		public void update(double universalTime,
                           List<CivilianVessel> vessels)
        {
			double next = getNextTaxesDate(universalTime);
			if (nextTaxesDate < next)
			{
				nextTaxesDate = next;
                int numCivilians = getCiviliansInFlight(vessels);
                this.addFunds(numCivilians * rentAmountPerCivilian);
			}
		}

		private int getCiviliansInFlight(List<CivilianVessel> vessels)
		{
			int res = 0;
			foreach (CivilianVessel vessel in vessels)
			{
                res += vessel.getCivilianCount();
			}
			return res;
		}

        public double getNextTaxesDate(double universalTime)
		{
            double currentDay = Math.Floor(universalTime / timeBetweenRents);
            return (currentDay + 1) * timeBetweenRents;
		}
    }
}
 