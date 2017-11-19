using System;
using System.Collections;

namespace CivilianPopulation.Domain
{
    public class CivilianKerbal
    {
		private string name;
		private string trait;
        private bool male;
        private bool dead;
        private double birthdate;
		private double expectingBirthAt;
        private string vesselId;

        public CivilianKerbal(string name, string trait, bool male, bool dead, double birthdate, double expectingBirthAt)
        {
            this.setup(name, trait, male, dead, birthdate, expectingBirthAt, null);
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
            if (!value.Contains("dead"))
            {
                throw new Exception("dead");
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
            vesselId = null;
            if (value.Contains("vesselId"))
            {
                vesselId = (string)value["vesselId"];
            }

            setup((string)value["name"], (string)value["trait"], (bool)value["male"], (bool)value["dead"], birth, expecting, vesselId);
		}

        private void setup(string name, string trait, bool male, bool dead, double birthdate, double expectingBirthAt, string vesselId)
        {
            this.name = name;
            this.trait = trait;
            this.male = male;
            this.dead = dead;
            this.birthdate = birthdate;
            this.expectingBirthAt = expectingBirthAt;
            this.vesselId = vesselId;
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

        public bool isDead()
        {
            return this.dead;
        }

        public void setDead(bool value)
        {
            this.dead = value;
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

        public string getVesselId()
        {
            return this.vesselId;
        }

        public void setVesselId(string id)
        {
            this.vesselId = id;
        }

        public CivilianKerbalAge getAge(double now)
        {
            CivilianKerbalAge res = CivilianKerbalAge.YOUNG;
            if (now - this.birthdate < 15 * TimeUnit.YEAR)
            {
                res = CivilianKerbalAge.YOUNG;
            }
            else if (now - this.birthdate < 30 * TimeUnit.YEAR)
            {
                res = CivilianKerbalAge.YOUNG_ADULT;
            }
            else if (now - this.birthdate < 45 * TimeUnit.YEAR)
            {
                res = CivilianKerbalAge.ADULT;
            }
            else if (now - this.birthdate < 60 * TimeUnit.YEAR)
            {
                res = CivilianKerbalAge.SENIOR;
            }
            else
            {
                res = CivilianKerbalAge.ANCIENT;
            }
            return res;
        }

        public Hashtable toTable() {
			Hashtable table = new Hashtable();
            table.Add("name", name);
            table.Add("trait", trait);
            table.Add("male", male);
            table.Add("dead", dead);
            table.Add("birthdate", birthdate);
            table.Add("expectingBirthAt", expectingBirthAt);
            table.Add("vesselId", vesselId);
            return table;
		}
    }
}
