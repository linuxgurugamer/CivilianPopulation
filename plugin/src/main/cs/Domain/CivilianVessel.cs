using System;

namespace CivilianPopulation.Domain
{
    public class CivilianVessel
    {
        private Guid id;
        private string name;
		private int civilianCount;
		private int docksCapacity;
		private double deliveryDate;

		public CivilianVessel(Guid id, string name, int civilianCount, int docksCapacity, double deliveryDate)
        {
            this.id = id;
            this.name = name;
            this.civilianCount = civilianCount;
			this.docksCapacity = docksCapacity;
            this.deliveryDate = deliveryDate;
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
        public double getDeliveryDate()
        {
            return deliveryDate;
        }
    }
}