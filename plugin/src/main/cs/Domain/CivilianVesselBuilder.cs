using System;
namespace CivilianPopulation.Domain
{
    public class CivilianVesselBuilder
    {
		private string name = "unknown";
		private int docksCapacity = 0;
        private bool orbiting = false;
        private CelestialBody body = new CelestialBody("Kerbin", CelestialBodyType.HOMEWORLD);
		private ContractorMission missionInProgress = null;
		
        public CivilianVessel build()
        {
            return new CivilianVessel(
                name,
                docksCapacity,
                orbiting,
                body,
                missionInProgress
            );
        }

		public CivilianVesselBuilder named(string name)
        {
            this.name = name;
            return this;
        }

		public CivilianVesselBuilder withADockCapacityOf(int number)
		{
			this.docksCapacity = number;
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
