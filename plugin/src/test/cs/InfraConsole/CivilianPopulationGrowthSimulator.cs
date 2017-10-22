using System;
using CivilianPopulation.Domain;
using System.Collections.Generic;
using System.Linq;

namespace InfraConsole
{
    class CivilianPopulationGrowthSimulator
    {
		private const double DAY_IN_SECONDS = 60 * 60 * 6;

		private CivilianPopulationGrowthService service;
        private CivilianVessel vessel;
		private System.Random rng;
        private int idx;

		public static void Main(string[] args)
        {
            CivilianPopulationGrowthSimulator simulator = new CivilianPopulationGrowthSimulator();
            simulator.run(2000);
        }

        public CivilianPopulationGrowthSimulator()
        {
            service = new CivilianPopulationGrowthService(setPregnant, birth);
            vessel = new CivilianVesselBuilder().build();
			vessel.addCrew(new CivilianKerbal("male-0", "Civilian", true, -1));
			vessel.addCrew(new CivilianKerbal("female-0", "Civilian", false, -1));
			this.rng = new System.Random();
            this.idx = 0;
		}

		public void setPregnant(CivilianKerbal kerbal, double at)
		{
			kerbal.setExpectingBirthAt(at);
		}

		public void birth(CivilianKerbal mother, bool male)
		{
			mother.setExpectingBirthAt(-1);
            CivilianKerbal kerbal = new CivilianKerbal("kerbal-"+idx, "Civilian", male, -1);
            idx = idx + 1;
            vessel.addCrew(kerbal);
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
                    + "\t" + vessel.getCrewCount()
					+ "\t" + vessel.getMales().Count()
					+ "\t" + vessel.getFemales().Count()
					+ "\t" + vessel.getFemales().Where(kerbal => kerbal.getExpectingBirthAt() > 0).Count()
                );
			}
		}
    }
}
