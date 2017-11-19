using System;
using CivilianPopulation.Domain;
using System.Collections.Generic;
using System.Linq;

namespace InfraConsole
{
    class CivilianPopulationGrowthSimulator
    {

        private CivilianPopulationGrowthService growth;
        private CivilianPopulationDeathService death;

        private System.Random rng;
        private int idx;

        private CivilianKerbalRoster roster;
        private CivilianVessel vessel;

        private double now;

		public static void Main(string[] args)
        {
            CivilianPopulationGrowthSimulator simulator = new CivilianPopulationGrowthSimulator();
            simulator.run(TimeUnit.DAYS_PER_YEARS*100);
        }

        public CivilianPopulationGrowthSimulator()
        {
            growth = new CivilianPopulationGrowthService(setPregnant, birth);
            death = new CivilianPopulationDeathService(kill);

            roster = new CivilianKerbalRoster();
            roster.add(new CivilianKerbal("male-0", "Civilian", true, false, 0, -1));
            roster.add(new CivilianKerbal("female-0", "Civilian", true, false, 0, -1));

            vessel = new CivilianVesselBuilder().allowingBreeding(true).build();

			this.rng = new System.Random();
            this.idx = 0;
		}

		public void setPregnant(CivilianKerbal kerbal, double at)
		{
            kerbal.setExpectingBirthAt(at);
		}

        public void birth(CivilianVessel vessel, CivilianKerbal mother, bool male)
		{
			mother.setExpectingBirthAt(-1);
            CivilianKerbal kerbal = new CivilianKerbal("kerbal-"+idx, "Civilian", male, false, now, -1);
            roster.add(kerbal);
            vessel.addCrew(kerbal);
            idx = idx + 1;
		}

        private void kill(CivilianKerbal kerbal)
        {
            Console.WriteLine(kerbal.getName() + " is dead");
            kerbal.setDead(true);
        }

		public void run(int days)
        {
			now = 1;
            for (int today = 0; today <= days; today++)
            {
                now = today * TimeUnit.DAY + 1;
                growth.update(now, vessel);

                CivilianKerbalRoster roster = new CivilianKerbalRoster();
                foreach (CivilianKerbal kerbal in vessel.getFemales())
                {
                    roster.add(kerbal);
                }
                foreach (CivilianKerbal kerbal in vessel.getMales())
                {
                    roster.add(kerbal);
                }
                death.update(now, roster);

                Console.WriteLine(
                    (today / TimeUnit.DAYS_PER_YEARS)
                    + "\t" + today
                    + "\t" + vessel.getCrewCount()
					+ "\t" + vessel.getMales().Count()
					+ "\t" + vessel.getFemales().Count()
					+ "\t" + vessel.getFemales().Where(kerbal => kerbal.getExpectingBirthAt() > 0).Count()
                );
			}
		}
    }
}
