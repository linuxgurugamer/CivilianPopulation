using System;
using System.Collections;

namespace CivilianPopulation.Domain.Repository
{
    public class CivPopKerbal
    {
        private string name;
        private bool civilian;
        private string vesselId;

        public CivPopKerbal(string name, bool civilian)
        {
            this.name = name;
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
            this.civilian = (bool)value["civilian"];
            this.vesselId = (string)value["vesselId"];
        }

        public string GetName()
        {
            return this.name;
        }

        public bool IsCivilian()
        {
            return this.civilian;
        }

        public void SetVesselId(string vesselId)
        {
            this.vesselId = vesselId;
        }

        public string GetVesselId()
        {
            return this.vesselId;
        }


        public Hashtable ToTable()
        {
            Hashtable table = new Hashtable();
            table.Add("name", name);
            table.Add("civilian", civilian);
            return table;
        }

    }
}
