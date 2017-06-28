using System;

namespace CivilianPopulation.Domain
{
    internal class MockedCivilianPopulationAdapter : CivilianPopulationAdapter
    {
        private int funds;

        public void addFunds(int amount)
        {
            this.funds += amount;
        }

        internal void setFunds(int amount)
        {
            this.funds = amount;
        }

        internal int getFunds()
        {
			return this.funds;
		}
    }
}