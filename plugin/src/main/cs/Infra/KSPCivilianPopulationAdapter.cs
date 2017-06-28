using System;
using CivilianPopulation.Domain;

namespace CivilianPopulation.Infra
{
    internal class KSPCivilianPopulationAdapter : CivilianPopulationAdapter
    {
        public void addFunds(int amount)
        {
			Funding.Instance.AddFunds(amount, TransactionReasons.Progression);
		}
    }
}