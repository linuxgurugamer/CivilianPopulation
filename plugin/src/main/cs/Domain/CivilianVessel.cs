using System;

namespace CivilianPopulation.Domain
{
    public class CivilianVessel
    {
        private Guid id;
        private string name;
		private int civilianCount;
		private bool civilianDocks;

        public CivilianVessel(Guid id, string name, int civilianCount, bool civilianDocks)
        {
            this.id = id;
            this.name = name;
            this.civilianCount = civilianCount;
			this.civilianDocks = civilianDocks;
		}

        public Guid getId()
        {
            return id;
        }
        public string getName()
		{
			return name;
		}
		public int getCivilianCount()
		{
			return civilianCount;
		}
        public bool hasCivilianDock()
        {
            return civilianDocks;
        }
	}
}