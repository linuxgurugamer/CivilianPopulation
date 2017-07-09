using System;
namespace CivilianPopulation.Domain
{
    public class CivilianVesselBuilder
    {
		private string name = "unknown";
		private int civilianCount = 0;
		private int docksCapacity = 0;
        private bool orbiting = false;
        private CelestialBodyType body = CelestialBodyType.HOMEWORLD;
		private ContractorMission missionInProgress = null;
		
        public CivilianVesselBuilder()
        {
        }
    		
        public CivilianVessel build()
        {
            return new CivilianVessel(
                name,
                civilianCount,
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

		public CivilianVesselBuilder countingCivilian(int count)
		{
			this.civilianCount = count;
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

		public CivilianVesselBuilder on(CelestialBodyType body)
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
