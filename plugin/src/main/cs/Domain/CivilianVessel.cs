using System;

namespace CivilianPopulation.Domain
{
    public class CivilianVessel
    {
        private Guid id;
        private string name;
		private int civilianCount;
		private int docksCapacity;

        public CivilianVessel(Guid id, string name, int civilianCount, int docksCapacity)
        {
            this.id = id;
            this.name = name;
            this.civilianCount = civilianCount;
			this.docksCapacity = docksCapacity;
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
        public int getDocksCapacity()
        {
            return docksCapacity;
        }
	}
}