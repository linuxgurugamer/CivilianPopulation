using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CivilianPopulation.Domain
{
    public class CivilianKerbalRoster
    {
        private Dictionary<string, CivilianKerbal> data;

		public CivilianKerbalRoster()
		{
			data = new Dictionary<string, CivilianKerbal>();
		}

        public CivilianKerbalRoster(string json)
        {
			data = new Dictionary<string, CivilianKerbal>();
			if (json != null && json != "")
            {
                Hashtable table = JSON.JsonDecode(json) as Hashtable;
                if (table == null)
                {
                    table = new Hashtable();
                }
                data = table
                    .Cast<DictionaryEntry>()
                    .ToDictionary(
                        kvp => (string)kvp.Key,
                        kvp => new CivilianKerbal((Hashtable)kvp.Value)
                    );
            }
		}

		public void add(CivilianKerbal kerbal)
		{
            data.Add(kerbal.getName(), kerbal);
		}


		public string toString()
		{
            Hashtable table = new Hashtable();
            foreach (string key in data.Keys)
            {
                table.Add(key, data[key].toTable());
            }
            return JSON.JsonEncode(table);
		}

		public IEnumerable<CivilianKerbal> list()
		{
            return data.Values;
		}

		public bool exists(string name)
        {
            return data.ContainsKey(name);
        }

        public void remove(string name)
        {
            data.Remove(name);
        }

        public int count()
        {
			int res = 0;
			foreach (CivilianKerbal kerbal in this.list())
			{
				res++;
			}
            return res;
		}

        public CivilianKerbal get(string name)
        {
            return data[name];
        }
    }
}
