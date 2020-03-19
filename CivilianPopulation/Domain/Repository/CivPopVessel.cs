using System;
using System.Collections;

namespace CivilianPopulation.Domain.Repository
{
    public class CivPopVessel
    {
        private bool orbiting;
        private CelestialBody body;

        private string id;
        private double capacity;
        private bool allowDocking;
        private bool allowBreeding;
        private double missionArrival;
        private string missionType;
        private Vessel kspVessel;
        public CivPopVessel(Vessel v)
        {
            this.kspVessel = v;
            this.id = v.id.ToString(); ;
            this.capacity = 0;
            this.allowDocking = false;
            this.allowBreeding = false;
            this.missionArrival = -1;
            this.missionType = null;
        }

        public CivPopVessel(Hashtable value)
        {
            if (!value.Contains("id"))
            {
                throw new Exception("no id");
            }
            this.id = (string)value["id"];

            if (!value.Contains("capacity"))
            {
                this.capacity = 0;
            }
            else
            {
                this.capacity = (double)value["capacity"];
            }

            if (!value.Contains("allowDocking"))
            {
                this.allowDocking = false;
            }
            else
            {
                this.allowDocking = (bool)value["allowDocking"];
            }

            if (!value.Contains("allowBreeding"))
            {
                this.allowBreeding = false;
            }
            else
            {
                this.allowBreeding = (bool)value["allowBreeding"];
            }

            if (!value.Contains("missionArrival"))
            {
                this.missionArrival = -1;
            }
            else
            {
                this.missionArrival = (double)value["missionArrival"];
            }

            if (!value.Contains("missionType"))
            {
                this.missionType = null;
            }
            else
            {
                this.missionType = (string)value["missionType"];
            }
        }

        public Vessel KSPVessel
        { get { return kspVessel; } set { kspVessel = value; } }
        public string GetId()
        {
            return id;
        }

        public double GetCapacity()
        {
            return this.capacity;
        }

        public void SetCapacity(double capacity)
        {
            this.capacity = capacity;
        }

        public bool IsOrbiting()
        {
            return this.orbiting;
        }

        public void SetOrbiting(bool orbiting)
        {
            this.orbiting = orbiting;
        }

        public bool IsAllowDocking()
        {
            return this.allowDocking;
        }

        public void SetAllowDocking(bool allow)
        {
            this.allowDocking = allow;
        }

        public bool IsAllowBreeding()
        {
            return this.allowBreeding;
        }

        public void SetAllowBreeding(bool allow)
        {
            this.allowBreeding = allow;
        }

        public CelestialBody GetBody()
        {
            return this.body;
        }

        public void SetBody(CelestialBody body)
        {
            this.body = body;
        }

        public double GetMissionArrival()
        {
            return this.missionArrival;
        }

        public void SetMissionArrival(double when)
        {
            this.missionArrival = when;
        }

        public string GetMissionType()
        {
            return this.missionType;
        }

        public void SetMissionType(string type)
        {
            this.missionType = type;
        }

        public Hashtable ToTable()
        {
            Hashtable table = new Hashtable();
            table.Add("id", id);
            table.Add("capacity", capacity);
            table.Add("allowDocking", allowDocking);
            table.Add("allowBreeding", allowBreeding);
            table.Add("missionArrival", missionArrival);
            table.Add("missionType", missionType);
            return table;
        }
    }
}
