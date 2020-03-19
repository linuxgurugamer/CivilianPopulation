using CivilianPopulation.Domain.Repository;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
                if (Constants.ValidVessel(vessel.KSPVessel))
                {
                    IEnumerable<CivPopCouple> couples = MakeCouples(date, vessel, repo);
                    turnPregnantSomeFemales(vessel.KSPVessel, date, couples, vessel.IsAllowBreeding());
                }
            }
            birthOfNewCivilans(date, repo);
        }

        public IEnumerable<CivPopCouple> MakeCouples(double date, CivPopVessel vessel, CivPopRepository repo)
        {
            IEnumerable<CivPopKerbal> adults = repo.GetLivingRosterForVessel(vessel.GetId())
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
            Vessel v,
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
                            //if ((rng.Next() % 100) <= CHANCE_OF_PREGNANCY + getTheaterBonus(v))
                            int pregChance = CHANCE_OF_PREGNANCY + getTheaterBonus(v);
                            int r = rng.Next() % 100;

                            if ((rng.Next() % 100) <= pregChance)
                            {
                                female.SetExpectingBirthAt(now + PREGNANCY_DURATION_IN_DAYS * TimeUnit.DAY);
                            }
                        }
                    }
                }
            }
        }
        private int getTheaterBonus(Vessel vessel)
        {
#if true
            int improvedChance = 0;
            if (Constants.ValidVessel(vessel))
            {
                List<MovieTheater> list = vessel.FindPartModulesImplementing<MovieTheater>();
                if (list == null)
                    Log.Error("getTheaterBonus, list is null");
                foreach (MovieTheater item in list)
                {
                    if (item.MovieType == "Romance")
                    {
                        improvedChance += (int)item.RomanceMovieBonus * 100;
                    }
                }
                return improvedChance;
            }
            else
                return 0;
#else
            return 0;
#endif
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

                        ProtoCrewMember pcm = new ProtoCrewMember(ProtoCrewMember.KerbalType.Crew, child.GetName());
                        KerbalRoster.SetExperienceTrait(pcm, "Civilian");//Set the Kerbal as the specified role (kerbalTraitName)
                        var plist = vessel.KSPVessel.parts.FindAll(p => p.CrewCapacity > p.protoModuleCrew.Count);

                        // There may be a better way, but this will look for space in the same part as the female giving birth
                        Part part = null;
                        foreach (var p in plist)
                        {
                            var crew = p.protoModuleCrew.Find(c => c.name == female.GetName());
                            if (crew != null)
                            {
                                part = p;
                                break;
                            }
                        }
                        // If part is null, no room in same part, so just find a part with room
                        if (part == null)
                            part = vessel.KSPVessel.parts.Find(p => p.CrewCapacity > p.protoModuleCrew.Count);
                        if (part != null)
                        {
                            part.AddCrewmember(pcm);
                            vessel.KSPVessel.SpawnCrew();
                        }
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
