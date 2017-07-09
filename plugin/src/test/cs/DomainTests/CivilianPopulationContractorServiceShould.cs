using System;
using NUnit.Framework;

namespace CivilianPopulation.Domain
{
    public class CivilianPopulationContractorServiceShould
    {
        private const double DAY_IN_SECONDS = 60 * 60 * 6;

        private double currentDate;

		private ContractorMission mission;
		private int delivered;

		private CivilianVesselBuilder builder;
        private CivilianPopulationContractorService service;

		[SetUp]
		public void SetUp()
		{
			currentDate = 0;

            mission = null;
            delivered = 0;

            builder = new CivilianVesselBuilder().withADockCapacityOf(4).inOrbit(true);
			service = new CivilianPopulationContractorService(setMission, addCivilian);
		}

		[Test()]
		public void launch_civilian_delivery_mission()
		{
            service.update(currentDate, builder.build());
			
            Assert.AreEqual(
                new ContractorMission(DAY_IN_SECONDS * 85, CelestialBodyType.HOMEWORLD),
                mission
            );
		}

		[Test()]
		public void launch_a_mission_for_day_100_at_day_15()
		{
            currentDate = DAY_IN_SECONDS * 15;

			service.update(currentDate, builder.build());

			Assert.AreEqual(
            	new ContractorMission(DAY_IN_SECONDS * 100, CelestialBodyType.HOMEWORLD),
            	mission
            );
		}

        [Test()]
		public void cancel_civilian_delivery_mission_if_vessel_is_not_in_orbit()
		{
			builder.inOrbit(false)
                   .targetedBy(new ContractorMission(DAY_IN_SECONDS * 30, CelestialBodyType.HOMEWORLD));

			service.update(currentDate, builder.build());

			Assert.Null(mission);
		}

        [Test()]
		public void launch_a_mission_for_day_185_at_day_15_when_vessel_is_in_homeworld_moon_orbit()
		{
			currentDate = DAY_IN_SECONDS * 15;
            builder.on(CelestialBodyType.HOMEWORLD_MOON);

			service.update(currentDate, builder.build());

			Assert.AreEqual(
                new ContractorMission(DAY_IN_SECONDS * 185, CelestialBodyType.HOMEWORLD_MOON),
				mission
			);
		}

		[Test()]
		public void cancel_civilian_delivery_mission_if_vessel_is_not_orbiting_a_homeworld_moon()
		{
            mission = new ContractorMission(DAY_IN_SECONDS * 30, CelestialBodyType.HOMEWORLD);
			builder.on(CelestialBodyType.OTHERS)
                   .targetedBy(mission);

			service.update(currentDate, builder.build());

			Assert.Null(mission);
		}

		[Test()]
		public void not_launch_civilian_delivery_mission_if_a_mission_is_in_progress()
		{
			mission = new ContractorMission(DAY_IN_SECONDS * 30, CelestialBodyType.HOMEWORLD);
			builder.targetedBy(mission);

            service.update(currentDate, builder.build());

			Assert.AreEqual(
            	new ContractorMission(DAY_IN_SECONDS * 30, CelestialBodyType.HOMEWORLD),
            	mission
            );
		}

		[Test()]
		public void cancel_mission_to_homeworld_orbit_if_current_situation_is_homeworld_moon()
		{
			mission = new ContractorMission(DAY_IN_SECONDS * 12, CelestialBodyType.HOMEWORLD);

            builder.on(CelestialBodyType.HOMEWORLD_MOON)
			       .targetedBy(mission);

			service.update(currentDate, builder.build());

			Assert.Null(mission);
		}

        [Test()]
		public void cancel_civilian_delivery_mission_if_docks_are_full()
		{
			mission = new ContractorMission(DAY_IN_SECONDS * 12, CelestialBodyType.HOMEWORLD);
            builder.targetedBy(mission)
                   .withADockCapacityOf(0);

			service.update(currentDate, builder.build());

			Assert.Null(mission);
		}

		[Test()]
		public void complete_civilian_delivery_mission()
		{
			mission = new ContractorMission(currentDate, CelestialBodyType.HOMEWORLD);
            builder.targetedBy(mission);

            currentDate = currentDate + 1;
			service.update(currentDate, builder.build());

			Assert.Null(mission);
            Assert.AreEqual(1, delivered);
		}

        private void setMission(ContractorMission newMission)
		{
            this.mission = newMission;
		}

        private void addCivilian()
        {
            this.delivered++;
        }
	}
}
