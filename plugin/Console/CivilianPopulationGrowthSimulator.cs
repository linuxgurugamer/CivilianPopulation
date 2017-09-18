using System;
using CivilianPopulation.Domain;
using System.Collections.Generic;
using System.Linq;

namespace InfraConsole
{
    class CivilianPopulationGrowthSimulator
    {
		private const double DAY_IN_SECONDS = 60 * 60 * 6;
		private const int PREGNANCY_DURATION_IN_DAYS = 75;

		private CivilianPopulationGrowthService service;
        private CivilianVessel vessel;
		private System.Random rng;


		public static void Main(string[] args)
        {
            CivilianPopulationGrowthSimulator simulator = new CivilianPopulationGrowthSimulator();
            simulator.run(2000);
        }

        public CivilianPopulationGrowthSimulator()
        {
            service = new CivilianPopulationGrowthService(setPregnant, birth);
            vessel = new CivilianVesselBuilder().build();
			vessel.addCivilian(new CivilianKerbal(true));
			vessel.addCivilian(new CivilianKerbal(false));
			this.rng = new System.Random();
		}

		public void setPregnant(CivilianKerbal kerbal, double at)
		{
			kerbal.setExpectingBirthAt(at + PREGNANCY_DURATION_IN_DAYS * DAY_IN_SECONDS);
		}

		public void birth(CivilianKerbal mother)
		{
			mother.setExpectingBirthAt(-1);
            CivilianKerbal kerbal = new CivilianKerbal(rng.Next() % 2 == 0);
            vessel.addCivilian(kerbal);
		}

		public void run(int days)
        {
			double now = 1;
            for (int today = 0; today <= days; today++)
            {
                Console.WriteLine("Today is day " + today);
                now = today * DAY_IN_SECONDS + 1;
                service.update(now, vessel);
                Console.WriteLine(
                    today 
                    + "\t" + vessel.getCivilianCount()
					+ "\t" + vessel.getMales().Count()
					+ "\t" + vessel.getFemales().Count()
					+ "\t" + vessel.getFemales().Where(kerbal => kerbal.getExpectingBirthAt() > 0).Count()
                );
			}
		}
    }
}
