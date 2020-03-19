using System;
using System.Collections;

namespace CivilianPopulation.Domain.Repository
{
    public class CivPopKerbal
    {
        private string name;
        private CivPopKerbalGender gender;
        private double birthdate;
        private bool dead;
        private double expectingBirthAt;
        private bool civilian;
        private string vesselId;

        public CivPopKerbal(string name, CivPopKerbalGender gender, double birthdate, bool civilian)
        {
            this.name = name;
            this.gender = gender;
            this.birthdate = birthdate;
            this.dead = false;
            this.expectingBirthAt = -1;
            this.civilian = civilian;
            this.vesselId = null;
        }

        public CivPopKerbal(Hashtable value)
        {
            if (!value.Contains("name"))
            {
                throw new Exception("no name");
            }
            if (!value.Contains("civilian"))
            {
                throw new Exception("no civilian");
            }
            this.name = (string)value["name"];
            if ((bool)value["male"])
            {
                this.gender = CivPopKerbalGender.MALE;
            }
            else
            {
                this.gender = CivPopKerbalGender.FEMALE;
            }
            this.birthdate = (double)value["birthdate"];
            this.dead = (bool)value["dead"];
            this.expectingBirthAt = (double)value["expectingBirthAt"];
            this.civilian = (bool)value["civilian"];
            this.vesselId = (string)value["vesselId"];
        }

        public string GetName()
        {
            return this.name;
        }

        public CivPopKerbalGender GetGender()
        {
            return this.gender;
        }

        public void SetCivilian(bool civilian)
        {
            this.civilian = civilian;
        }

        public bool IsCivilian()
        {
            return this.civilian;
        }

        public void SetDead(bool dead)
        {
            this.dead = dead;
        }

        public bool IsDead()
        {
            return this.dead;
        }

        public void SetBirthdate(double date)
        {
            this.birthdate = date;
        }

        public double GetBirthdate()
        {
            return this.birthdate;
        }

        public void SetExpectingBirthAt(double date)
        {
            this.expectingBirthAt = date;
        }

        public double GetExpectingBirthAt()
        {
            return this.expectingBirthAt;
        }

        public string GetVesselId()
        {
            return this.vesselId;
        }

        public void SetVesselId(string vesselId)
        {
            this.vesselId = vesselId;
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

        public Hashtable ToTable()
        {
            Hashtable table = new Hashtable();
            table.Add("name", name);
            table.Add("male", CivPopKerbalGender.MALE == this.gender);
            table.Add("birthdate", birthdate);
            table.Add("dead", dead);
            table.Add("expectingBirthAt", expectingBirthAt);
            table.Add("civilian", civilian);
            table.Add("vesselId", vesselId);
            return table;
        }

    }
}
