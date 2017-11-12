using System;
using System.Collections;

namespace CivilianPopulation.Domain
{
    public class CivilianKerbal
    {
		private string name;
		private string trait;
		private bool male;
        private double birthdate;
		private double expectingBirthAt;

        public CivilianKerbal(string name, string trait, bool male, double birthdate, double expectingBirthAt)
        {
            this.setup(name, trait, male, birthdate, expectingBirthAt);
        }

        public CivilianKerbal(Hashtable value)
        {
            if (!value.Contains("name"))
            {
                throw new Exception("no name");
            }
            if (!value.Contains("trait"))
            {
                throw new Exception("no trait");
            }
            if (!value.Contains("male"))
            {
                throw new Exception("no male");
            }
            double birth = -1;
            if (value.Contains("birthdate"))
            {
                birth = (double)value["birthdate"];
            }

            double expecting = -1;
            if (value.Contains("expectingBirthAt"))
            {
                expecting = (double)value["expectingBirthAt"];
            }
            setup((string)value["name"], (string)value["trait"], (bool)value["male"], birth, expecting);
		}

        private void setup(string name, string trait, bool male, double birthdate, double expectingBirthAt)
        {
            this.name = name;
            this.trait = trait;
            this.male = male;
            this.birthdate = birthdate;
            this.expectingBirthAt = expectingBirthAt;
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

        public double getBirthDate()
        {
            return this.birthdate;
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
            table.Add("birthdate", birthdate);
			table.Add("expectingBirthAt", expectingBirthAt);
            return table;
		}
    }
}
