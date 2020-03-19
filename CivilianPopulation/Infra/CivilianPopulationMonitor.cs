using System;
using System.Collections.Generic;
using UnityEngine;

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

        public void add(String counter)
        {
            if (!counters.ContainsKey(counter))
            {
                counters[counter] = 0;
            }
            counters[counter] = counters[counter] + DateTime.Now.Ticks - lastTick;
            lastTick = DateTime.Now.Ticks;
        }

        public void show()
        {
            foreach (string key in counters.Keys)
            {
                log(key + " : " + counters[key]);
            }
        }

        private void log(string message)
        {
            Debug.Log(this.GetType().Name + " - " + message);
        }
    }
}