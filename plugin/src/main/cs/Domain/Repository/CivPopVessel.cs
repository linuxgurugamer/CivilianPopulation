using System;
using System.Collections;

namespace CivilianPopulation.Domain.Repository
{
    public class CivPopVessel 
    {
        private string id;

        public CivPopVessel(string id)
        {
            this.id = id;
        }

        public CivPopVessel(Hashtable value)
        {
            if (!value.Contains("id"))
            {
                throw new Exception("no id");
            }
            this.id = (string)value["id"];
        }

        public string GetId()
        {
            return id;
        }

        public Hashtable ToTable()
        {
            Hashtable table = new Hashtable();
            table.Add("id", id);
            return table;
        }
    }
}
