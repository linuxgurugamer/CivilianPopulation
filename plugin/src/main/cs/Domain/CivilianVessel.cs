using System;

namespace CivilianPopulation.Domain
{
    public class CivilianVessel
    {
        private readonly string name;
		private readonly int civilianCount;
		private readonly int docksCapacity;
		private readonly bool orbiting;
		private readonly CelestialBodyType body;
		private readonly ContractorMission missionInProgress;

		public CivilianVessel(
            string name, 
            int civilianCount, 
            int docksCapacity,
            bool orbiting,
            CelestialBodyType body,
            ContractorMission missionInProgress
        )
        {
			this.name = name;
			this.civilianCount = civilianCount;
			this.docksCapacity = docksCapacity;
			this.orbiting = orbiting;
			this.body = body;
			this.missionInProgress = missionInProgress;
		}

        public string getName()
		{
			return name;
		}
		public int getCivilianCount()
		{
			return civilianCount;
		}
        public int getDocksCapacity()
        {
            return docksCapacity;
        }
		public bool isOrbiting()
		{
			return this.orbiting;
		}
		public CelestialBodyType getBody()
		{
			return this.body;
		}
		public ContractorMission getMission()
		{
			return this.missionInProgress;
		}
    }
}