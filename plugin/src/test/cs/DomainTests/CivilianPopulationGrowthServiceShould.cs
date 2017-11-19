using System;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

namespace CivilianPopulation.Domain
{
    public class CivilianPopulationGrowthServiceShould
    {
        public const double DAY_IN_SECONDS = 60 * 60 * 6;
		
        [SetUp]
        public void SetUp()
        {
        }

        [Test()]
        public void make_update_once_a_day()
        {
            CivilianPopulationGrowthServiceCount service = new CivilianPopulationGrowthServiceCount(setPregnant, birth);

            double date = 45;
            CivilianVessel vessel = null;

            service.update(date, vessel);
            Assert.AreEqual(0, service.getNbUpdates());

            date = date + 1;
            service.update(date, vessel);
            Assert.AreEqual(0, service.getNbUpdates());

            date = date + 1;
            service.update(date, vessel);
            Assert.AreEqual(0, service.getNbUpdates());

            date = date + DAY_IN_SECONDS;
            service.update(date, vessel);
            Assert.AreEqual(1, service.getNbUpdates());

            date = date + 1;
            service.update(date, vessel);
            Assert.AreEqual(1, service.getNbUpdates());

            date = date + DAY_IN_SECONDS;
            service.update(date, vessel);
            Assert.AreEqual(2, service.getNbUpdates());
        }

		[Test()]
        public void create_no_couples_when_no_males_nor_females()
        {
			CivilianPopulationGrowthTestService service = new CivilianPopulationGrowthTestService(setPregnant, birth);
            List<CivilianKerbal> males = new List<CivilianKerbal>();
            List<CivilianKerbal> females = new List<CivilianKerbal>();
            IEnumerable<CivilianKerbalCouple> couples = service.testMakeCouples(males, females);
            Assert.AreEqual(0, couples.Count());
		}

		[Test()]
		public void create_no_couples_when_no_females()
		{
			CivilianPopulationGrowthTestService service = new CivilianPopulationGrowthTestService(setPregnant, birth);
			List<CivilianKerbal> males = new List<CivilianKerbal>();
			for (int i = 0; i < 50; i++)
			{
                males.Add(new CivilianKerbal("male", "Civilian", true, false, -1, -1));
			}
			List<CivilianKerbal> females = new List<CivilianKerbal>();
			IEnumerable<CivilianKerbalCouple> couples = service.testMakeCouples(males, females);
			Assert.AreEqual(0, couples.Count());
		}

		[Test()]
		public void create_some_couples_when_many_males_and_females()
		{
			CivilianPopulationGrowthTestService service = new CivilianPopulationGrowthTestService(setPregnant, birth);
			List<CivilianKerbal> males = new List<CivilianKerbal>();
			List<CivilianKerbal> females = new List<CivilianKerbal>();
			for (int i = 0; i < 50; i++)
			{
                males.Add(new CivilianKerbal("male-" + i, "Civilian", true, false, -1, -1));
                females.Add(new CivilianKerbal("female-" + i, "Civilian", false, false, -1, -1));
			}
			IEnumerable<CivilianKerbalCouple> couples = service.testMakeCouples(males, females);
			Assert.AreEqual(14, couples.Count());
		}

		[Test()]
		public void turn_pregnant_no_female_when_no_couples()
		{
			List<CivilianKerbal> males = new List<CivilianKerbal>();
			List<CivilianKerbal> females = new List<CivilianKerbal>();

			CivilianPopulationGrowthTestService service = new CivilianPopulationGrowthTestService(setPregnant, birth);
			IEnumerable<CivilianKerbalCouple> couples = service.testMakeCouples(males, females);
			service.testTurnPregnantSomeFemales(0, couples, true);
			Assert.AreEqual(0, females.Where(female => female.getExpectingBirthAt() > 0).Count());
		}

		[Test()]
		public void turn_pregnant_some_female_when_in_couples()
		{
			CivilianPopulationGrowthTestService service = new CivilianPopulationGrowthTestService(setPregnant, birth);
			List<CivilianKerbal> males = new List<CivilianKerbal>();
			List<CivilianKerbal> females = new List<CivilianKerbal>();
			for (int i = 0; i < 50; i++)
			{
                males.Add(new CivilianKerbal("male-" + i, "Civilian", true, false, -1, -1));
                females.Add(new CivilianKerbal("female-" + i, "Civilian", false, false, -1, -1));
			}
			IEnumerable<CivilianKerbalCouple> couples = service.testMakeCouples(males, females);
            service.testTurnPregnantSomeFemales(25 * TimeUnit.YEAR, couples, true);
			Assert.AreEqual(3, females.Where(female => female.getExpectingBirthAt() > 0).Count());
		}

		[Test()]
		public void turn_pregnant_some_female_when_breeding_is_not_allowed()
		{
			CivilianPopulationGrowthTestService service = new CivilianPopulationGrowthTestService(setPregnant, birth);
			List<CivilianKerbal> males = new List<CivilianKerbal>();
			List<CivilianKerbal> females = new List<CivilianKerbal>();
			for (int i = 0; i < 50; i++)
			{
                males.Add(new CivilianKerbal("male-" + i, "Civilian", true, false, -1, -1));
                females.Add(new CivilianKerbal("female-" + i, "Civilian", false, false, -1, -1));
			}
			IEnumerable<CivilianKerbalCouple> couples = service.testMakeCouples(males, females);
			service.testTurnPregnantSomeFemales(0, couples, false);
			Assert.AreEqual(0, females.Where(female => female.getExpectingBirthAt() > 0).Count());
		}

		[Test()]
        public void make_female_give_birth_after_pregnancy_duration()
        {
            double now = 1;
			CivilianPopulationGrowthTestService service = new CivilianPopulationGrowthTestService(setPregnant, birth);
            CivilianVessel vessel = new CivilianVesselBuilder().build();
			for (int i = 0; i < 50; i++)
			{
                vessel.addCrew(new CivilianKerbal("male-" + i, "Civilian", true, false, -1, -1));
                vessel.addCrew(new CivilianKerbal("female-" + i, "Civilian", false, false, -1, -1));
			}
            CivilianKerbal mother = new CivilianKerbal("mother", "Civilian", false, false, -1, now);
            vessel.addCrew(mother);

            service.update(now, vessel);
			now = now + DAY_IN_SECONDS;
			service.update(now, vessel);

			Assert.AreEqual(-1, mother.getExpectingBirthAt());
		}

		public void setPregnant(CivilianKerbal kerbal, double at)
		{
			kerbal.setExpectingBirthAt(at);
		}
        public void birth(CivilianVessel vessel, CivilianKerbal mother, bool male)
		{
			mother.setExpectingBirthAt(-1);
		}
	}

    class CivilianPopulationGrowthServiceCount : CivilianPopulationGrowthService
    {
        private int nbUpdates;

        public CivilianPopulationGrowthServiceCount(
            Action<CivilianKerbal, double> setPregnant,
            Action<CivilianVessel, CivilianKerbal, bool> birth) : base(setPregnant, birth)
        {
            nbUpdates = 0;
        }

        protected override void doUpdate(double currentDate, CivilianVessel vessel)
        {
            nbUpdates++;
        }

        public int getNbUpdates()
        {
            return this.nbUpdates;
        }
    }

	class CivilianPopulationGrowthTestService : CivilianPopulationGrowthService
	{
		public CivilianPopulationGrowthTestService(
			Action<CivilianKerbal, double> setPregnant,
            Action<CivilianVessel, CivilianKerbal, bool> birth) : base(setPregnant, birth)
		{
            this.rng = new System.Random(42);
		}

		public IEnumerable<CivilianKerbalCouple> testMakeCouples(
            IEnumerable<CivilianKerbal> males, 
            IEnumerable<CivilianKerbal> females)
		{
            return base.makeCouples(25 * TimeUnit.YEAR, males, females);
		}

		public void testTurnPregnantSomeFemales(double date,
                                                IEnumerable<CivilianKerbalCouple> couples,
                                                bool breedingAllowed)
		{
			base.turnPregnantSomeFemales(date, couples, breedingAllowed);
		}

		public void testBirthOfNewCivilans(CivilianVessel vessel, 
                                           double date,
                                           IEnumerable<CivilianKerbal> females)
		{
            base.birthOfNewCivilans(vessel, date, females);
		}

	}

}
