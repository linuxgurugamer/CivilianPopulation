using System;

namespace CivilianPopulation.Domain
{
    public class CivilianKerbal
    {
        private string name;
        private bool male;
		private double expectingBirthAt;

		public CivilianKerbal(string name, bool male, double expectingBirthAt)
        {
            this.name = name;
            this.male = male;
            this.expectingBirthAt = expectingBirthAt;
        }

		public string getName()
		{
			return this.name;
		}
		
        public bool isMale() 
        {
            return this.male;
        }

		public double getExpectingBirthAt()
		{
			return expectingBirthAt;
		}

        public void setExpectingBirthAt(double at)
        {
			this.expectingBirthAt = at;
		}
    }
}