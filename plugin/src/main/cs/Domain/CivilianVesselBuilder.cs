using System;
namespace CivilianPopulation.Domain
{
    public class CivilianVesselBuilder
    {
        private Guid id;
        private string name = "unknown";
		private int housingCapacity = 0;
		private bool dockingAllowed = false;
		private bool breedingAllowed = false;
		private bool orbiting = false;
		private CelestialBody body = new CelestialBody("Kerbin", CelestialBodyType.HOMEWORLD);
		private ContractorMission missionInProgress = null;

		public CivilianVessel build()
        {
            return new CivilianVessel(
                id,
                name,
                housingCapacity,
                dockingAllowed,
                breedingAllowed,
                orbiting,
                body,
                missionInProgress
            );
        }

        public CivilianVesselBuilder identified(Guid id)
        {
            this.id = id;
            return this;
        }

        public CivilianVesselBuilder named(string name)
        {
            this.name = name;
            return this;
        }

		public CivilianVesselBuilder withAHousingCapacityOf(int number)
		{
			this.housingCapacity = number;
			return this;
		}

		public CivilianVesselBuilder allowingDocking(bool allow)
		{
			this.dockingAllowed = allow;
			return this;
		}

		public CivilianVesselBuilder allowingBreeding(bool allow)
		{
			this.breedingAllowed = allow;
			return this;
		}

		public CivilianVesselBuilder inOrbit(bool isOrbiting)
		{
			this.orbiting = isOrbiting;
			return this;
		}

		public CivilianVesselBuilder on(CelestialBody body)
		{
			this.body = body;
			return this;
		}

        public CivilianVesselBuilder targetedBy(ContractorMission mission)
		{
			this.missionInProgress = mission;
			return this;
		}

	}
}
