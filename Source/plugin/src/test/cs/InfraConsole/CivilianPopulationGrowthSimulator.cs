using System;
using System.Collections.Generic;
using System.Linq;
using CivilianPopulation.Domain.Services;
using CivilianPopulation.Domain.Repository;
using CivilianPopulation.Domain;

namespace InfraConsole
{
    class CivilianPopulationGrowthSimulator
    {

        private CivPopServiceGrowth growth;
        private CivPopServiceDeath death;

        private System.Random rng;
        private int idx;

        private CivPopRepository repo;

        private double now;

		public static void Main(string[] args)
        {
            CivilianPopulationGrowthSimulator simulator = new CivilianPopulationGrowthSimulator();
            simulator.run(TimeUnit.DAYS_PER_YEARS*100);
        }

        public CivilianPopulationGrowthSimulator()
        {
            this.rng = new System.Random();
            this.idx = 0;

            CivPopKerbalBuilder builder = new CivPopKerbalBuilder(GetName);
            growth = new CivPopServiceGrowth(builder);
            death = new CivPopServiceDeath();

            repo = new CivPopRepository();

            CivPopVessel vessel = new CivPopVessel("vessel");
            vessel.SetAllowBreeding(true);
            vessel.SetCapacity(1000000);
            repo.Add(vessel);

            CivPopKerbal male = new CivPopKerbal(GetName(CivPopKerbalGender.MALE), CivPopKerbalGender.MALE, 0, true);
            male.SetVesselId("vessel");
            repo.Add(male);
            CivPopKerbal female = new CivPopKerbal(GetName(CivPopKerbalGender.FEMALE), CivPopKerbalGender.FEMALE, 0, true);
            female.SetVesselId("vessel");
            repo.Add(female);
		}

        public string GetName(CivPopKerbalGender gender)
        {
            this.idx++;
            return "kerbal-" + idx;
        }

		public void run(int days)
        {
			now = 1;
            for (int today = 0; today <= days; today++)
            {
                now = today * TimeUnit.DAY + 1;
                growth.Update(now, repo);
                death.Update(now, repo);

                Console.WriteLine(
                    (today / TimeUnit.DAYS_PER_YEARS)
                    + "\t" + today
                    + "\t" + repo.GetLivingRosterForVessel("vessel").Count()
                    + "\t" + repo.GetLivingRosterForVessel("vessel").Where(k => k.GetGender() == CivPopKerbalGender.MALE).Count()
                    + "\t" + repo.GetLivingRosterForVessel("vessel").Where(k => k.GetGender() == CivPopKerbalGender.FEMALE).Count()
                    + "\t" + repo.GetLivingRosterForVessel("vessel").Where(kerbal => kerbal.GetExpectingBirthAt() > 0).Count()
                );
			}
		}
    }
}
