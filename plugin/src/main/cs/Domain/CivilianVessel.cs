namespace CivilianPopulation.Domain
{
    public class CivilianVessel
    {
        private string name;
        private int civilianCount;

        public CivilianVessel(string name, int civilianCount)
        {
            this.name = name;
            this.civilianCount = civilianCount;
        }

        public string getName()
		{
			return name;
		}
		public int getCivilianCount()
		{
			return civilianCount;
		}
	}
}