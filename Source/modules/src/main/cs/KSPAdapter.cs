using System.Collections.Generic;

namespace CivilianPopulation
{
    internal class KSPAdapter : CivilianPopulationAdapter
    {
        public double getUniversalTime()
        {
            return Planetarium.GetUniversalTime();
        }

        public void addFunds(int amount)
        {
            Funding.Instance.AddFunds(amount, TransactionReasons.Progression);
        }

        public bool isCareer()
        {
            return HighLogic.CurrentGame.Mode == Game.Modes.CAREER;
        }

        public List<CivilianVessel> getVessels()
        {
            List<CivilianVessel> res = new List<CivilianVessel>();
            foreach (Vessel vessel in FlightGlobals.Vessels)
            {
                if (vessel != null)
                {
                    CivilianVessel civVessel = new CivilianVessel(vessel);
                    res.Add(civVessel);
                }
            }
            return res;
        }

    }
}