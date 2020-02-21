using CivilianPopulation.Domain.Repository;
using System.Collections.Generic;
using System.Linq;

namespace CivilianPopulation.Domain.Services
{
    public class CivPopServiceGrowth : CivPopOncePerDayService
    {
        private const int MALE_AVAILABILITY = 3;
        private const int FEMALE_AVAILABILITY = 3;
        private const int CHANCE_OF_PREGNANCY = 10;
        private const int PREGNANCY_DURATION_IN_DAYS = TimeUnit.DAYS_PER_YEARS * 3 / 4;

        private CivPopKerbalBuilder builder;

        protected System.Random rng;

        public CivPopServiceGrowth(CivPopKerbalBuilder builder)
        {
            this.builder = builder;
            this.rng = new System.Random();
        }

        public CivPopServiceGrowth(CivPopKerbalBuilder builder, System.Random rng)
        {
            this.builder = builder;
            this.rng = rng;
        }

        protected override void DoUpdate(double date, CivPopRepository repo)
        {
            foreach (CivPopVessel vessel in repo.GetVessels())
            {
                IEnumerable<CivPopCouple> couples = makeCouples(date, vessel, repo);
                turnPregnantSomeFemales(date, couples, vessel.IsAllowBreeding());
            }
            birthOfNewCivilans(date, repo);
        }

        public IEnumerable<CivPopCouple> makeCouples(double date, CivPopVessel vessel, CivPopRepository repo)
        {
            IEnumerable<CivPopKerbal> adults
            = repo.GetLivingRosterForVessel(vessel.GetId())
                   .Where(kerbal => kerbal.getAge(date) != CivilianKerbalAge.YOUNG);

            IEnumerable<CivPopKerbal> males
                = adults.Where(kerbal => kerbal.GetGender() == CivPopKerbalGender.MALE);
            IEnumerable<CivPopKerbal> females
                = adults.Where(kerbal => kerbal.GetGender() == CivPopKerbalGender.FEMALE);


            List<CivPopKerbal> availableMales = new List<CivPopKerbal>();
            foreach (CivPopKerbal kerbal in males)
            {
                if (rng.Next() % MALE_AVAILABILITY == 0)
                {
                    availableMales.Add(kerbal);
                }
            }

            List<CivPopKerbal> availableFemales = new List<CivPopKerbal>();
            foreach (CivPopKerbal kerbal in females)
            {
                if (rng.Next() % FEMALE_AVAILABILITY == 0)
                {
                    availableFemales.Add(kerbal);
                }
            }

            List<CivPopCouple> couples = new List<CivPopCouple>();
            foreach (CivPopKerbal male in availableMales)
            {
                if (availableFemales.Count > 0)
                {
                    int index = rng.Next() % availableFemales.Count();
                    CivPopKerbal female = availableFemales[index];
                    availableFemales.RemoveAt(index);
                    couples.Add(new CivPopCouple(male, female));
                }
            }
            return couples;
        }

        public void turnPregnantSomeFemales(
            double now,
            IEnumerable<CivPopCouple> couples,
            bool breedingAllowed)
        {
            if (breedingAllowed)
            {
                foreach (CivPopCouple couple in couples)
                {
                    CivPopKerbal female = couple.GetFemale();
                    if (female.getAge(now) == CivilianKerbalAge.YOUNG_ADULT
                        || female.getAge(now) == CivilianKerbalAge.ADULT)
                    {
                        if (female.GetExpectingBirthAt() < 0)
                        {
                            if (rng.Next() % CHANCE_OF_PREGNANCY == 0)
                            {
                                female.SetExpectingBirthAt(now + PREGNANCY_DURATION_IN_DAYS * TimeUnit.DAY);
                            }
                        }
                    }
                }
            }
        }

        public void birthOfNewCivilans(double date, CivPopRepository repo)
        {
            List<CivPopKerbal> childs = new List<CivPopKerbal>();

            IEnumerable<CivPopKerbal> females = repo.GetRoster()
                .Where(kerbal => kerbal.GetExpectingBirthAt() > 0)
                .Where(kerbal => !kerbal.IsDead())
                .Where(kerbal => kerbal.GetExpectingBirthAt() < date)
                ;
            foreach (CivPopKerbal female in females)
            {
                female.SetExpectingBirthAt(-1);
                if (female.GetVesselId() != null)
                {
                    CivPopVessel vessel = repo.GetVessel(female.GetVesselId());
                    if (vessel.GetCapacity() > repo.GetLivingRosterForVessel(vessel.GetId()).Count())
                    {
                        CivPopKerbal child = builder.build(date);
                        child.SetBirthdate(date);
                        child.SetVesselId(female.GetVesselId());
                        childs.Add(child);
                    }
                }
            }

            foreach (CivPopKerbal child in childs)
            {
                repo.Add(child);
            }
        }
    }
}
