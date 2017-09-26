using System;
using System.Collections;

namespace CivilianPopulation.Domain
{
    public class CivilianKerbal
    {
		private string name;
		private string trait;
		private bool male;
		private double expectingBirthAt;

        public CivilianKerbal(string name, string trait, bool male, double expectingBirthAt)
        {
            this.name = name;
            this.trait = trait;
            this.male = male;
            this.expectingBirthAt = expectingBirthAt;
        }

        public CivilianKerbal(Hashtable value)
        {
			this.name = (string)value["name"];
			this.trait = (string)value["trait"];
			this.male = (bool)value["male"];
            this.expectingBirthAt = (double)value["expectingBirthAt"];
		}

        public string getName()
		{
			return this.name;
		}

        public string getTrait()
        {
            return this.trait;
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

        public Hashtable toTable() {
			Hashtable table = new Hashtable();
            table.Add("name", name);
            table.Add("trait", trait);
			table.Add("male", male);
			table.Add("expectingBirthAt", expectingBirthAt);
            return table;
		}
    }
}