using System;
using System.Collections.Generic;
using System.Linq;

namespace CivilianPopulation.Domain
{
    public class CivilianVessel
    {
        private readonly string name;
		private readonly List<CivilianKerbal> civilians;
		private readonly int docksCapacity;
		private readonly bool orbiting;
		private readonly CelestialBody body;
		private readonly ContractorMission missionInProgress;

		public CivilianVessel(
            string name, 
            int docksCapacity,
            bool orbiting,
            CelestialBody body,
            ContractorMission missionInProgress
        )
        {
			this.name = name;
            this.civilians = new List<CivilianKerbal>();
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
            return this.civilians.Count();
		}
        public int getDocksCapacity()
        {
            return docksCapacity;
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

		public void addCivilian(CivilianKerbal kerbal)
		{
            this.civilians.Add((kerbal));
		}

        public IEnumerable<CivilianKerbal> getMales()
		{
            List<CivilianKerbal> res = new List<CivilianKerbal>();
            foreach(CivilianKerbal kerbal in this.civilians.Where(kerbal => kerbal.isMale()))
            {
                res.Add(kerbal);
            }
            return res;
		}

		public IEnumerable<CivilianKerbal> getFemales()
		{
			List<CivilianKerbal> res = new List<CivilianKerbal>();
			foreach (CivilianKerbal kerbal in this.civilians.Where(kerbal => !kerbal.isMale()))
			{
				res.Add(kerbal);
			}
			return res;
		}
	}
}