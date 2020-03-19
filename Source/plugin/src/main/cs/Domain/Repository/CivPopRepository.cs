using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CivilianPopulation.Domain.Repository
{
    [KSPScenario(ScenarioCreationOptions.AddToAllGames, GameScenes.FLIGHT, GameScenes.TRACKSTATION, GameScenes.SPACECENTER)]
    public class CivilianPopulationRosterModule : ScenarioModule
    {

    }
    public class CivPopRepository
    {
        private double funds;

        private Dictionary<string, CivPopKerbal> roster;
        private Dictionary<string, CivPopVessel> fleet;

        public CivPopRepository()
        {
            this.funds = 0;
            this.roster = new Dictionary<string, CivPopKerbal>();
            this.fleet = new Dictionary<string, CivPopVessel>();
        }

        public CivPopRepository(string json)
        {
            this.funds = 0;
            this.roster = new Dictionary<string, CivPopKerbal>();
            this.fleet = new Dictionary<string, CivPopVessel>();

            if (json != null && json != "")
            {
                Hashtable table = JSON.JsonDecode(json) as Hashtable;
                if (table != null)
                {
                    this.funds = (double)table["funds"];
                    this.roster = RosterFromJson(table);
                    this.fleet = FleetFromJson(table);
                }
            }
        }

        public double GetFunds()
        {
            return this.funds;
        }

        public void AddFunds(double amount)
        {
            this.funds = this.funds + amount;
        }

        public void Add(CivPopKerbal kerbal)
        {
            if (roster.ContainsKey(kerbal.GetName()))
            {
                roster[kerbal.GetName()] = kerbal;
            }
            else
            {
                roster.Add(kerbal.GetName(), kerbal);
            }
        }

        public void Kill(CivPopKerbal kerbal)
        {
            kerbal.SetDead(true);
        }

        public void Remove(CivPopKerbal kerbal)
        {
            roster.Remove(kerbal.GetName());
        }

        public bool KerbalExists(string name)
        {
            return roster.ContainsKey(name);
        }

        public IEnumerable<CivPopKerbal> GetRoster()
        {
            return roster.Values;
        }

        public IEnumerable<CivPopKerbal> GetLivingRosterForVessel(string vesselId)
        {
            return roster.Values
                         .Where(k => vesselId.Equals(k.GetVesselId()))
                         .Where(k => !k.IsDead());
        }

        public IEnumerable<CivPopKerbal> GetDeadRosterForVessel(string vesselId)
        {
            List<CivPopKerbal> res = new List<CivPopKerbal>();
            foreach (CivPopKerbal current in roster.Values
                         .Where(k => vesselId.Equals(k.GetVesselId()))
                         .Where(k => k.IsDead()))
            {
                res.Add(current);
            }
            return res;
        }

        public CivPopKerbal GetKerbal(string name)
        {
            CivPopKerbal res = null;
            if (KerbalExists(name))
            {
                res = roster[name];
            }
            return res;
        }

        public void Add(CivPopVessel vessel)
        {
            if (vessel.KSPVessel == null)
            {
                return;
            }
            if (vessel.KSPVessel.vesselType == VesselType.Debris ||
              vessel.KSPVessel.vesselType == VesselType.SpaceObject ||
              vessel.KSPVessel.vesselType == VesselType.EVA ||
              vessel.KSPVessel.vesselType == VesselType.Flag ||
              vessel.KSPVessel.vesselType == VesselType.DeployedScienceController ||
              vessel.KSPVessel.vesselType == VesselType.DeployedSciencePart ||
              vessel.KSPVessel.vesselType == VesselType.Unknown
              )
                return;
            if (vessel.KSPVessel.rootPart == null)
            {
                SimpleLogger.fetch.Info("vessel.KSPVessel.rootPart == null");
                string currentStackTrace = System.Environment.StackTrace;
                SimpleLogger.fetch.Info("currentStackTrace: " + currentStackTrace);

                return;
            }
            if (vessel.KSPVessel.rootPart.Modules == null)
            {
                SimpleLogger.fetch.Info("vessel.KSPVessel.rootPart.Modules == null");
                string currentStackTrace = System.Environment.StackTrace;
                SimpleLogger.fetch.Info("currentStackTrace: " + currentStackTrace);
                return;
            }


            if (vessel.KSPVessel.rootPart.Modules.Contains<ModuleAsteroid>())
            {
                SimpleLogger.fetch.Info("Vessel is asteroid");
                string currentStackTrace = System.Environment.StackTrace;
                SimpleLogger.fetch.Info("currentStackTrace: " + currentStackTrace);
                return;
            }

            if (fleet.ContainsKey(vessel.GetId()))
            {
                fleet[vessel.GetId()] = vessel;
            }
            else
            {
                fleet.Add(vessel.GetId(), vessel);
            }
        }

        public bool VesselExists(string name)
        {
            return fleet.ContainsKey(name);
        }

        public IEnumerable<CivPopVessel> GetVessels()
        {
            List<CivPopVessel> res = new List<CivPopVessel>();
            foreach (CivPopVessel civVessel in this.fleet.Values)
            {
                res.Add(civVessel);
            }
            return res;
        }

        public CivPopVessel GetVessel(string vesselId)
        {
            return fleet[vesselId];
        }

        public void Remove(CivPopVessel civVessel)
        {
            fleet.Remove(civVessel.GetId());
        }

        public string ToJson()
        {
            return "{" +
                "\"funds\":" + funds + "," +
                "\"roster\":" + ToJson(roster.Keys, (key) => roster[key].ToTable()) + "," +
                "\"fleet\":" + ToJson(fleet.Keys, (key) => fleet[key].ToTable()) +
                "}";
        }

        private Dictionary<string, CivPopKerbal> RosterFromJson(Hashtable table)
        {
            return (table["roster"] as Hashtable).Cast<DictionaryEntry>()
                        .ToDictionary(
                            kvp => (string)kvp.Key,
                            kvp => new CivPopKerbal((Hashtable)kvp.Value)
                        );
        }

        private Dictionary<string, CivPopVessel> FleetFromJson(Hashtable table)
        {
            return (table["fleet"] as Hashtable).Cast<DictionaryEntry>()
                        .ToDictionary(
                            kvp => (string)kvp.Key,
                            kvp => new CivPopVessel((Hashtable)kvp.Value)
                        );
        }

        private string ToJson(IEnumerable<string> keys, Func<string, Object> toTable)
        {
            Hashtable table = new Hashtable();
            foreach (string key in keys)
            {
                table.Add(key, toTable(key));
            }
            return JSON.JsonEncode(table);
        }

        public override bool Equals(object obj)
        {
            var that = obj as CivPopRepository;

            if (that == null)
            {
                return false;
            }

            return this.ToJson().Equals(that.ToJson());
        }

        public override int GetHashCode()
        {
            return this.ToJson().GetHashCode();
        }
    }
}
