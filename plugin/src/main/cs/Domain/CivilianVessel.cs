using System;
using System.Collections.Generic;
using System.Linq;

namespace CivilianPopulation.Domain
{
    public class CivilianVessel
    {
        private readonly string name;
		private readonly List<CivilianKerbal> crew;
		private readonly int housingCapacity;
		private readonly bool dockingAllowed;
		private readonly bool breedingAllowed;
		private readonly bool orbiting;
		private readonly CelestialBody body;
		private readonly ContractorMission missionInProgress;

		public CivilianVessel(
            string name,
			int housingCapacity,
			bool dockingAllowed,
			bool breedingAllowed,
            bool orbiting,
            CelestialBody body,
            ContractorMission missionInProgress
        )
        {
			this.name = name;
            this.crew = new List<CivilianKerbal>();
			this.housingCapacity = housingCapacity;
			this.dockingAllowed = dockingAllowed;
			this.breedingAllowed = breedingAllowed;
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
            return this.crew.FindAll(k => k.getTrait() == "Civilian").Count();
        }
        public int getCrewCount()
		{
            return this.crew.Count();
		}
		public int getHousingCapacity()
		{
			return housingCapacity;
		}
		public bool isDockingAllowed()
		{
			return dockingAllowed;
		}
		public bool isBreedingAllowed()
		{
			return breedingAllowed;
		}
		public bool isOrbiting()
		{
			return this.orbiting;
		}
		public CelestialBody getBody()
		{
			return this.body;
		}
		public ContractorMission getMission()
		{
			return this.missionInProgress;
		}

		public void addCrew(CivilianKerbal kerbal)
		{
            this.crew.Add((kerbal));
		}

        public IEnumerable<CivilianKerbal> getMales()
		{
            List<CivilianKerbal> res = new List<CivilianKerbal>();
            foreach(CivilianKerbal kerbal in this.crew.Where(kerbal => kerbal.isMale()))
            {
                res.Add(kerbal);
            }
            return res;
		}

		public IEnumerable<CivilianKerbal> getFemales()
		{
			List<CivilianKerbal> res = new List<CivilianKerbal>();
			foreach (CivilianKerbal kerbal in this.crew.Where(kerbal => !kerbal.isMale()))
			{
				res.Add(kerbal);
			}
			return res;
		}
	}
}