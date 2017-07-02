namespace CivilianPopulation.Domain
{
    public class CivilianVessel
    {
        private string name;
		private int civilianCount;
		private bool civilianDocks;

		public CivilianVessel(string name, int civilianCount, bool civilianDocks)
        {
            this.name = name;
            this.civilianCount = civilianCount;
            this.civilianDocks = civilianDocks;
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