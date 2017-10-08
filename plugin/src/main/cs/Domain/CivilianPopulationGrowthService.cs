using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CivilianPopulation.Domain
{
    public class CivilianPopulationGrowthService
    {
		private const double DAY_IN_SECONDS = 60 * 60 * 6;
		private const int MALE_AVAILABILITY = 3;
		private const int FEMALE_AVAILABILITY = 3;
		private const int CHANCE_OF_PREGNANCY = 10;
		private const int PREGNANCY_DURATION_IN_DAYS = 75;

		private Action<CivilianKerbal, double> setPregnant;
        private Action<CivilianKerbal, bool> birth;

		protected System.Random rng;
		private double lastUpdate;

        public CivilianPopulationGrowthService(
            Action<CivilianKerbal, double> setPregnant,
            Action<CivilianKerbal, bool> birth)
        {
			this.setPregnant = setPregnant;
			this.birth = birth;
			this.lastUpdate = -1;
			this.rng = new System.Random();
		}

        public void update(double currentDate, CivilianVessel vessel)
        {
            if (lastUpdate > -1 && getDay(lastUpdate) < getDay(currentDate))
            {
                doUpdate(currentDate, vessel);
            }
            lastUpdate = currentDate;
        }

        protected virtual void doUpdate(double currentDate, CivilianVessel vessel)
        {
            // select males
            IEnumerable<CivilianKerbal> males = vessel.getMales();
            // select females
            IEnumerable<CivilianKerbal> females = vessel.getFemales();
            // make couples
            IEnumerable<CivilianKerbalCouple> couples = makeCouples(males, females);
            // change females state
            turnPregnantSomeFemales(currentDate, couples, vessel.isBreedingAllowed());
            // birth
            birthOfNewCivilans(currentDate, females);
        }

        protected virtual IEnumerable<CivilianKerbalCouple> makeCouples(
            IEnumerable<CivilianKerbal> males, 
            IEnumerable<CivilianKerbal> females)
        {
            List<CivilianKerbal> availableMales = new List<CivilianKerbal>();
            foreach (CivilianKerbal kerbal in males)
            {
                if (rng.Next() % MALE_AVAILABILITY == 0)
                {
                    availableMales.Add(kerbal);
                }
            }

            List<CivilianKerbal> availableFemales = new List<CivilianKerbal>();
            foreach (CivilianKerbal kerbal in females)
            {
                if (rng.Next() % FEMALE_AVAILABILITY == 0)
                {
                    availableFemales.Add(kerbal);
                }
            }

            List<CivilianKerbalCouple> couples = new List<CivilianKerbalCouple>();
            foreach (CivilianKerbal male in availableMales)
            {
                if (availableFemales.Count > 0)
                {
                    int index = rng.Next() % availableFemales.Count();
                    CivilianKerbal female = availableFemales[index];
                    availableFemales.RemoveAt(index);
                    couples.Add(new CivilianKerbalCouple(male, female));
                }
            }
            return couples;
        }

        protected virtual void turnPregnantSomeFemales(
            double now,
    		IEnumerable<CivilianKerbalCouple> couples,
    		bool breedingAllowed)
		{
            if (breedingAllowed)
            {
                foreach (CivilianKerbalCouple couple in couples)
                {
                    CivilianKerbal female = couple.getFemale();
                    if (female.getExpectingBirthAt() < 0)
                    {
                        if (rng.Next() % CHANCE_OF_PREGNANCY == 0)
                        {
                            this.setPregnant(female, now + PREGNANCY_DURATION_IN_DAYS * DAY_IN_SECONDS);
                        }
                    }
                }
            }
        }

        protected virtual void birthOfNewCivilans(
			double now,
			IEnumerable<CivilianKerbal> females)
        {
            foreach (CivilianKerbal kerbal in females)
            {
                if (kerbal.getExpectingBirthAt() > 0 && kerbal.getExpectingBirthAt() < now)
                {
                    birth(kerbal, rng.Next() % 2 == 0);
                }
            }
        }

        private double getDay(double date)
        {
            return Math.Floor(date / DAY_IN_SECONDS);
        }
    }
}
