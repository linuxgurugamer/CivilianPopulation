using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CivilianPopulation.Domain
{
    public class CivilianPopulationGrowthService
    {
		private const int MALE_AVAILABILITY = 3;
		private const int FEMALE_AVAILABILITY = 3;
		private const int CHANCE_OF_PREGNANCY = 10;
        private const int PREGNANCY_DURATION_IN_DAYS = TimeUnit.DAYS_PER_YEARS * 3 / 4;

		private Action<CivilianKerbal, double> setPregnant;
        private Action<CivilianVessel, CivilianKerbal, bool> birth;

		protected System.Random rng;
		private double lastUpdate;

        public CivilianPopulationGrowthService(
            Action<CivilianKerbal, double> setPregnant,
            Action<CivilianVessel, CivilianKerbal, bool> birth)
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
            IEnumerable<CivilianKerbalCouple> couples = makeCouples(currentDate, males, females);
            // change females state
            turnPregnantSomeFemales(currentDate, couples, vessel.isBreedingAllowed());
            // birth
            birthOfNewCivilans(vessel, currentDate, females);
        }

        protected virtual IEnumerable<CivilianKerbalCouple> makeCouples(
            double currentDate,
            IEnumerable<CivilianKerbal> males, 
            IEnumerable<CivilianKerbal> females)
        {
            List<CivilianKerbal> availableMales = new List<CivilianKerbal>();
            foreach (CivilianKerbal kerbal in males)
            {
                if (kerbal.getAge(currentDate) != CivilianKerbalAge.YOUNG 
                    && rng.Next() % MALE_AVAILABILITY == 0)
                {
                    availableMales.Add(kerbal);
                }
            }

            List<CivilianKerbal> availableFemales = new List<CivilianKerbal>();
            foreach (CivilianKerbal kerbal in females)
            {
                if (kerbal.getAge(currentDate) != CivilianKerbalAge.YOUNG 
                    && rng.Next() % FEMALE_AVAILABILITY == 0)
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
                    if (female.getAge(now) == CivilianKerbalAge.YOUNG_ADULT
                        || female.getAge(now) == CivilianKerbalAge.ADULT)
                    {
                        if (female.getExpectingBirthAt() < 0)
                        {
                            if (rng.Next() % CHANCE_OF_PREGNANCY == 0)
                            {
                                this.setPregnant(female, now + PREGNANCY_DURATION_IN_DAYS * TimeUnit.DAY);
                            }
                        }
                    }
                }
            }
        }

        protected virtual void birthOfNewCivilans(
            CivilianVessel vessel,
			double now,
			IEnumerable<CivilianKerbal> females)
        {
            foreach (CivilianKerbal kerbal in females)
            {
                if (kerbal.getExpectingBirthAt() > 0 && kerbal.getExpectingBirthAt() < now)
                {
                    birth(vessel, kerbal, rng.Next() % 2 == 0);
                }
            }
        }

        private double getDay(double date)
        {
            return Math.Floor(date / TimeUnit.DAY);
        }
    }
}
