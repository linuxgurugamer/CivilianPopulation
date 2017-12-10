using System;
using System.Collections.Generic;
using System.Linq;
using CivilianPopulation.Domain.Repository;
using NUnit.Framework;

namespace CivilianPopulation.Domain.Services
{
    [TestFixture()]
    public class CivPopServiceGrowthShould
    {
        private CivPopKerbalBuilder builder;
        private CivPopRepository repo;
        private CivPopVessel vessel;

        private CivPopServiceGrowth service;

        [SetUp]
        public void SetUp()
        {
            System.Random rng = new System.Random(42);

            builder = new CivPopKerbalBuilder(g => Guid.NewGuid().ToString(), rng);

            repo = new CivPopRepository();
            vessel = new CivPopVessel("vessel");
            vessel.SetAllowBreeding(true);
            repo.Add(vessel);

            service = new CivPopServiceGrowth(builder, rng);
        }

        [Test()]
        public void create_no_couples_when_no_males_nor_females()
        {
            IEnumerable<CivPopCouple> couples = service.makeCouples(0, vessel, repo);
            Assert.AreEqual(0, couples.Count());
        }

        [Test()]
        public void create_no_couples_when_no_females()
        {
            for (int i = 0; i < 100; i++)
            {
                CivPopKerbal kerbal = builder.build(0);
                kerbal.SetVesselId(vessel.GetId());
                if (CivPopKerbalGender.MALE.Equals(kerbal.GetGender()))
                {
                    repo.Add(kerbal);
                }
            }
            IEnumerable<CivPopCouple> couples = service.makeCouples(0, vessel, repo);
            Assert.AreEqual(0, couples.Count());
        }

        [Test()]
        public void create_couples_when_many_males_and_females()
        {
            for (int i = 0; i < 100; i++)
            {
                CivPopKerbal kerbal = builder.build(0);
                kerbal.SetVesselId(vessel.GetId());
                repo.Add(kerbal);
            }
            IEnumerable<CivPopCouple> couples = service.makeCouples(0, vessel, repo);
            Assert.AreEqual(19, couples.Count());
        }

        [Test()]
        public void create_no_couples_when_all_males_are_dead()
        {
            for (int i = 0; i < 100; i++)
            {
                CivPopKerbal kerbal = builder.build(0);
                kerbal.SetVesselId(vessel.GetId());
                repo.Add(kerbal);
                if (kerbal.GetGender() == CivPopKerbalGender.MALE)
                {
                    repo.Kill(kerbal);
                }
            }
            IEnumerable<CivPopCouple> couples = service.makeCouples(0, vessel, repo);
            Assert.AreEqual(0, couples.Count());
        }

        [Test()]
        public void create_no_couples_when_all_females_are_dead()
        {
            for (int i = 0; i < 100; i++)
            {
                CivPopKerbal kerbal = builder.build(0);
                kerbal.SetVesselId(vessel.GetId());
                repo.Add(kerbal);
                if (kerbal.GetGender() == CivPopKerbalGender.FEMALE)
                {
                    repo.Kill(kerbal);
                }
            }
            IEnumerable<CivPopCouple> couples = service.makeCouples(0, vessel, repo);
            Assert.AreEqual(0, couples.Count());
        }

        [Test()]
        public void turn_pregnant_some_female_when_in_couples()
        {
            for (int i = 0; i < 1000; i++)
            {
                CivPopKerbal kerbal = builder.build(0);
                kerbal.SetVesselId(vessel.GetId());
                repo.Add(kerbal);
            }
            IEnumerable<CivPopCouple> couples = service.makeCouples(0, vessel, repo);
            service.turnPregnantSomeFemales(0, couples, vessel.IsAllowBreeding());

            int count = repo.GetLivingRosterForVessel(vessel.GetId())
                .Where(k => k.GetExpectingBirthAt() > 0)
                .Count();
            Assert.AreEqual(14, count);
        }

        [Test()]
        public void not_turn_pregnant_some_female_when_breeding_not_allowed()
        {
            for (int i = 0; i < 1000; i++)
            {
                CivPopKerbal kerbal = builder.build(0);
                kerbal.SetVesselId(vessel.GetId());
                repo.Add(kerbal);
            }
            IEnumerable<CivPopCouple> couples = service.makeCouples(0, vessel, repo);
            service.turnPregnantSomeFemales(0, couples, false);

            int count = repo.GetLivingRosterForVessel(vessel.GetId())
                .Where(k => k.GetExpectingBirthAt() > 0)
                .Count();
            Assert.AreEqual(0, count);
        }

        [Test()]
        public void make_female_give_birth_after_pregnancy_duration()
        {
            vessel.SetCapacity(2);
            CivPopKerbal kerbal = builder.build(0);
            kerbal.SetVesselId(vessel.GetId());
            kerbal.SetExpectingBirthAt(2);
            repo.Add(kerbal);

            service.Update(0, repo);
            service.Update(TimeUnit.DAY, repo);

            Assert.AreEqual(-1, kerbal.GetExpectingBirthAt());
            Assert.AreEqual(2, repo.GetLivingRosterForVessel(vessel.GetId()).Count());
        }

        [Test()]
        public void not_make_dead_female_give_birth_after_pregnancy_duration()
        {
            vessel.SetCapacity(2);
            CivPopKerbal kerbal = builder.build(0);
            kerbal.SetVesselId(vessel.GetId());
            kerbal.SetExpectingBirthAt(2);
            repo.Add(kerbal);
            repo.Kill(kerbal);

            service.Update(0, repo);
            service.Update(TimeUnit.DAY, repo);

            Assert.AreEqual(2, kerbal.GetExpectingBirthAt());
            Assert.AreEqual(0, repo.GetLivingRosterForVessel(vessel.GetId()).Count());
        }

        [Test()]
        public void make_female_give_birth_after_pregnancy_duration_no_room_in_vessel()
        {
            vessel.SetCapacity(1);
            CivPopKerbal kerbal = builder.build(0);
            kerbal.SetVesselId(vessel.GetId());
            kerbal.SetExpectingBirthAt(2);
            repo.Add(kerbal);

            service.Update(0, repo);
            service.Update(TimeUnit.DAY, repo);

            Assert.AreEqual(-1, kerbal.GetExpectingBirthAt());
            Assert.AreEqual(1, repo.GetLivingRosterForVessel(vessel.GetId()).Count());
        }
    }
}
