using System;
using System.Linq;
using CivilianPopulation.Domain.Repository;
using NUnit.Framework;

namespace CivilianPopulation.Domain.Services
{
    [TestFixture()]
    public class CivPopServiceContractorsShould
    {

        private static CelestialBody KERBIN = new CelestialBody("Kerbin", CelestialBodyType.HOMEWORLD);
        private static CelestialBody MUN = new CelestialBody("Mun", CelestialBodyType.HOMEWORLD_MOON);
        private static CelestialBody MINMUS = new CelestialBody("Minmus", CelestialBodyType.HOMEWORLD_MOON);
        private static CelestialBody DUNA = new CelestialBody("Duna", CelestialBodyType.OTHERS);
        private static CelestialBody EVE = new CelestialBody("Eve", CelestialBodyType.OTHERS);
        private static CelestialBody KERBOL = new CelestialBody("Kerbol", CelestialBodyType.OTHERS);

        private CivPopRepository repo;
        private CivPopVessel vessel;
        private CivPopServiceContractors service;

        [SetUp]
        public void SetUp()
        {
            CivPopKerbalBuilder builder = new CivPopKerbalBuilder(g => Guid.NewGuid().ToString());

            repo = new CivPopRepository();

            vessel = new CivPopVessel("vessel");
            repo.Add(vessel);
            vessel.SetCapacity(4);

            service = new CivPopServiceContractors(builder);
        }

        private string generateName(CivPopKerbalGender gender)
        {
            return Guid.NewGuid().ToString();
        }

        [Test()]
        public void launch_civilian_delivery_mission()
        {
            vessel.SetOrbiting(true);
            vessel.SetBody(KERBIN);
            vessel.SetMissionType(null);
            vessel.SetMissionArrival(-1);
            vessel.SetAllowDocking(true);

            service.Update(0, repo);

            Assert.AreEqual(TimeUnit.DAY * 85, repo.GetVessel("vessel").GetMissionArrival());
            Assert.AreEqual("HOMEWORLD", repo.GetVessel("vessel").GetMissionType());
        }

        [Test()]
        public void launch_a_mission_to_replace_a_dead_crew()
        {
            vessel.SetOrbiting(true);
            vessel.SetBody(KERBIN);
            vessel.SetMissionType(null);
            vessel.SetMissionArrival(-1);
            vessel.SetAllowDocking(true);

            for (int i = 0; i < 4; i++)
            {
                CivPopKerbal kerbal = new CivPopKerbal("kerbal" + i, CivPopKerbalGender.FEMALE, 0, true);
                repo.Add(kerbal);
                kerbal.SetVesselId(vessel.GetId());
                repo.Kill(kerbal);
            }

            service.Update(0, repo);

            Assert.AreEqual(TimeUnit.DAY * 85, repo.GetVessel("vessel").GetMissionArrival());
            Assert.AreEqual("HOMEWORLD", repo.GetVessel("vessel").GetMissionType());
        }

        [Test()]
        public void launch_a_mission_for_day_100_at_day_15()
        {
            vessel.SetOrbiting(true);
            vessel.SetBody(KERBIN);
            vessel.SetMissionType(null);
            vessel.SetMissionArrival(-1);
            vessel.SetAllowDocking(true);

            service.Update(TimeUnit.DAY * 15, repo);

            Assert.AreEqual(TimeUnit.DAY * 100, repo.GetVessel("vessel").GetMissionArrival());
            Assert.AreEqual("HOMEWORLD", repo.GetVessel("vessel").GetMissionType());
        }

        [Test()]
        public void cancel_civilian_delivery_mission_if_vessel_is_not_in_orbit()
        {
            vessel.SetOrbiting(false);
            vessel.SetBody(KERBIN);
            vessel.SetMissionType("HOMEWORLD");
            vessel.SetMissionArrival(TimeUnit.DAY * 30);
            vessel.SetAllowDocking(true);

            service.Update(0, repo);

            Assert.AreEqual(-1, repo.GetVessel("vessel").GetMissionArrival());
            Assert.AreEqual(null, repo.GetVessel("vessel").GetMissionType());
        }

        [Test()]
        public void launch_a_mission_for_day_185_at_day_15_when_vessel_is_in_homeworld_moon_orbit()
        {
            vessel.SetOrbiting(true);
            vessel.SetBody(MINMUS);
            vessel.SetMissionType(null);
            vessel.SetMissionArrival(-1);
            vessel.SetAllowDocking(true);

            service.Update(TimeUnit.DAY * 15, repo);

            Assert.AreEqual(TimeUnit.DAY * 185, repo.GetVessel("vessel").GetMissionArrival());
            Assert.AreEqual("MOON", repo.GetVessel("vessel").GetMissionType());
        }

        [Test()]
        public void cancel_civilian_delivery_mission_if_vessel_is_not_orbiting_a_homeworld_moon()
        {
            vessel.SetOrbiting(true);
            vessel.SetBody(EVE);
            vessel.SetMissionType("HOMEWORLD");
            vessel.SetMissionArrival(TimeUnit.DAY * 30);
            vessel.SetAllowDocking(true);

            service.Update(0, repo);

            Assert.AreEqual(-1, repo.GetVessel("vessel").GetMissionArrival());
            Assert.AreEqual(null, repo.GetVessel("vessel").GetMissionType());
        }

        [Test()]
        public void not_launch_civilian_delivery_mission_if_a_mission_is_in_progress()
        {
            vessel.SetOrbiting(true);
            vessel.SetBody(KERBIN);
            vessel.SetMissionType("HOMEWORLD");
            vessel.SetMissionArrival(TimeUnit.DAY * 30);
            vessel.SetAllowDocking(true);

            service.Update(0, repo);

            Assert.AreEqual(TimeUnit.DAY * 30, repo.GetVessel("vessel").GetMissionArrival());
            Assert.AreEqual("HOMEWORLD", repo.GetVessel("vessel").GetMissionType());
        }

        [Test()]
        public void cancel_mission_to_homeworld_orbit_if_current_situation_is_homeworld_moon()
        {
            vessel.SetOrbiting(true);
            vessel.SetBody(MUN);
            vessel.SetMissionType("HOMEWORLD");
            vessel.SetMissionArrival(TimeUnit.DAY * 12);
            vessel.SetAllowDocking(true);

            service.Update(0, repo);

            Assert.AreEqual(-1, repo.GetVessel("vessel").GetMissionArrival());
            Assert.AreEqual(null, repo.GetVessel("vessel").GetMissionType());
        }

        [Test()]
        public void cancel_civilian_delivery_mission_if_docks_are_full()
        {
            vessel.SetOrbiting(true);
            vessel.SetBody(KERBIN);
            vessel.SetMissionType("HOMEWORLD");
            vessel.SetMissionArrival(TimeUnit.DAY * 12);
            vessel.SetAllowDocking(true);

            for (int i = 0; i < 4; i++)
            {
                CivPopKerbal kerbal = new CivPopKerbal("kerbal" + i, CivPopKerbalGender.FEMALE, 0, true);
                repo.Add(kerbal);
                kerbal.SetVesselId(vessel.GetId());
            }

            service.Update(0, repo);

            Assert.AreEqual(-1, repo.GetVessel("vessel").GetMissionArrival());
            Assert.AreEqual(null, repo.GetVessel("vessel").GetMissionType());
        }

        [Test()]
        public void complete_civilian_delivery_mission()
        {
            vessel.SetOrbiting(true);
            vessel.SetBody(KERBIN);
            vessel.SetMissionType("HOMEWORLD");
            vessel.SetMissionArrival(0);
            vessel.SetAllowDocking(true);

            service.Update(1, repo);

            Assert.AreEqual(-1, repo.GetVessel("vessel").GetMissionArrival());
            Assert.AreEqual(null, repo.GetVessel("vessel").GetMissionType());
            int count = repo.GetRoster().Where(kerbal => kerbal.GetVesselId().Equals(vessel.GetId())).Count();
            Assert.AreEqual(1, count);
        }

        [Test()]
        public void cancel_civilian_delivery_mission_if_vessel_is_not_allowing_docking()
        {
            vessel.SetOrbiting(true);
            vessel.SetBody(KERBIN);
            vessel.SetMissionType("HOMEWORLD");
            vessel.SetMissionArrival(TimeUnit.DAY * 12);
            vessel.SetAllowDocking(false);

            service.Update(0, repo);

            Assert.AreEqual(-1, repo.GetVessel("vessel").GetMissionArrival());
            Assert.AreEqual(null, repo.GetVessel("vessel").GetMissionType());
        }

        [Test()]
        public void not_launch_civilian_delivery_mission_if_vessel_is_not_allowing_docking()
        {
            vessel.SetOrbiting(true);
            vessel.SetBody(KERBIN);
            vessel.SetMissionType(null);
            vessel.SetMissionArrival(-1);
            vessel.SetAllowDocking(false);

            service.Update(0, repo);

            Assert.AreEqual(-1, repo.GetVessel("vessel").GetMissionArrival());
            Assert.AreEqual(null, repo.GetVessel("vessel").GetMissionType());
        }

    }
}
