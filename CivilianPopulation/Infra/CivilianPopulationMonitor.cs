using System;
using System.Collections.Generic;
using System.Diagnostics;


namespace CivilianPopulation.Infra
{
    public class CivilianPopulationMonitor
    {
        private Dictionary<string, long> counters;
        private long lastTick;

        public CivilianPopulationMonitor()
        {
            counters = new Dictionary<string, long>();
            lastTick = DateTime.Now.Ticks;
        }

        [ConditionalAttribute("DEBUG")]
        public void add(String counter)
        {
            if (!counters.ContainsKey(counter))
            {
                counters[counter] = 0;
            }
            counters[counter] = counters[counter] + DateTime.Now.Ticks - lastTick;
            lastTick = DateTime.Now.Ticks;
        }

        [ConditionalAttribute("DEBUG")]
        public void show()
        {
            foreach (string key in counters.Keys)
            {
                Log.Info(key + " : " + counters[key]);
            }
        }
    }
}