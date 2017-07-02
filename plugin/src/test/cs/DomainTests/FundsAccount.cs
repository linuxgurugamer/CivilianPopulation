using System;

namespace CivilianPopulation.Domain
{
    internal class FundsAccount
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