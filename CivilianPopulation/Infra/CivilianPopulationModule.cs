using CivilianPopulation.Domain;
using CivilianPopulation.Domain.Repository;
using CivilianPopulation.Domain.Services;
using CivilianPopulation.GUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CivilianPopulation.Infra
{
    [KSPScenario(ScenarioCreationOptions.AddToAllGames, GameScenes.FLIGHT, GameScenes.TRACKSTATION, GameScenes.SPACECENTER)]
    public class CivilianPopulationModule : ScenarioModule

    // [KSPAddon(KSPAddon.Startup.AllGameScenes, true)]
    //public class CivilianPopulationModule: MonoBehaviour
    {
        internal static CivilianPopulationModule Instance;
        private static CivPopKerbalBuilder builder;
        private static CivPopServiceContractors contractors;
        private static CivPopServiceDeath death;
        internal static CivPopServiceGrowth growth;
        private static CivPopServiceRent rent;

        [KSPField(isPersistant = true, guiActive = false)]
        public string repoJSON;

        private static CivilianPopulationGUI gui;

        protected System.Random rng;
        private CivilianPopulationService service;
#if DEBUG
        private CivilianPopulationMonitor monitor;
#endif
        public void Start()
        {
            Instance = this;
            if (builder == null)
            {
                builder = new CivPopKerbalBuilder(this.GenerateKerbalName);
            }
            if (contractors == null)
            {
                contractors = new CivPopServiceContractors(builder);
            }
            if (death == null)
            {
                death = new CivPopServiceDeath();
            }
            if (growth == null)
            {
                growth = new CivPopServiceGrowth(builder);
            }
            if (rent == null)
            {
                rent = new CivPopServiceRent();
            }
            if (gui == null)
            {
                gui = new CivilianPopulationGUI(rent);
            }
            else
                gui.InitButton();


            this.rng = new System.Random();
            this.service = new CivilianPopulationService();
#if DEBUG
            this.monitor = new CivilianPopulationMonitor();
#endif
            DontDestroyOnLoad(this);

            StartCoroutine(SlowUpdate());
        }

        public void OnGUI()
        {
            
#if DEBUG
            //monitor.show();
            monitor.add("Trash");
#endif
            gui.update(Planetarium.GetUniversalTime(), GetRepository());
#if DEBUG
            monitor.add("OnGUI");
#endif
        }

        internal void DoGrowthUpdateNow()
        {
            var currentDate = Planetarium.GetUniversalTime();
            CivPopRepository repo = GetRepository();
            foreach (var crew in repo.GetRoster())
            {
                if (crew.GetExpectingBirthAt() <= currentDate)
                {
                    growth.Update(currentDate, repo, true);
#if DEBUG
                    monitor.add("growth");
#endif
                    return;
                }
            }
        }
        IEnumerator SlowUpdate()
        {
            while (true)
            {
                yield return new WaitForSecondsRealtime(1f);

                DoGrowthUpdateNow();

#if false
            }
        }
        public void FixedUpdate()
        {
#endif
#if DEBUG
                monitor.add("Trash");
#endif
                double now = Planetarium.GetUniversalTime();
#if DEBUG
                monitor.add("GetUniversalTime");
#endif

                CivPopRepository repo = GetRepository();
                UpdateRepository(repo);
#if DEBUG
                monitor.add("UpdateRepository");
#endif
                contractors.Update(now, repo);
#if DEBUG
                monitor.add("contractors");
#endif
                death.Update(now, repo);
#if DEBUG
                monitor.add("death");
#endif
                growth.Update(now, repo);
#if DEBUG
                monitor.add("growth");
#endif

                if (HighLogic.CurrentGame.Mode == Game.Modes.CAREER)
                {
                    rent.Update(now, repo);
                }
#if DEBUG
                monitor.add("rent");
#endif
                Vessel vessel = FlightGlobals.ActiveVessel;
                if (vessel != null)
                {
                    service.KillKerbals(repo, vessel);
                    service.CreateKerbals(repo, vessel);
                }
#if DEBUG
                monitor.add("Vessels");
#endif

                repoJSON = repo.ToJson();
#if DEBUG
                monitor.add("ToJson");
#endif
            }
        }

        private string GenerateKerbalName(CivPopKerbalGender gender)
        {
            string res;
            if (CivPopKerbalGender.MALE.Equals(gender))
            {
                res = CrewGenerator.GetRandomName(ProtoCrewMember.Gender.Male);
            }
            else
            {
                res = CrewGenerator.GetRandomName(ProtoCrewMember.Gender.Female);
            }
            return res;
        }

        private CivPopRepository repository = null;
        private CivPopRepository GetRepository()
        {
#if DEBUG
            monitor.add("Trash");
#endif
            if (repository == null)
            {
                repository = new CivPopRepository();
                if (repoJSON != null)
                {
                    repository = new CivPopRepository(repoJSON.Replace('[', '{').Replace(']', '}'));
                }
            }
#if DEBUG
            monitor.add("GetRepository");
#endif
            return repository;
        }

        private void UpdateRepository(CivPopRepository repo)
        {
            if (Funding.Instance != null && repo.GetFunds() > 0)
            {
                Funding.Instance.AddFunds(repo.GetFunds(), TransactionReasons.Progression);
                repo.AddFunds(repo.GetFunds() * -1);
            }

            ProtoCrewMember.KerbalType type = ProtoCrewMember.KerbalType.Crew;
            //ProtoCrewMember.KerbalType.Applicant;
            //ProtoCrewMember.KerbalType.Crew;
            //ProtoCrewMember.KerbalType.Tourist;
            //ProtoCrewMember.KerbalType.Unowned;

            ProtoCrewMember.RosterStatus[] statuses = {
                ProtoCrewMember.RosterStatus.Assigned,
                ProtoCrewMember.RosterStatus.Available,
                ProtoCrewMember.RosterStatus.Dead,
                ProtoCrewMember.RosterStatus.Missing
            };
            IEnumerable<ProtoCrewMember> kerbals = HighLogic.CurrentGame.CrewRoster.Kerbals(type, statuses);
            foreach (ProtoCrewMember kerbal in kerbals)
            {
                CivPopKerbal civKerbal = repo.GetKerbal(kerbal.name);
                if (civKerbal == null)
                {
                    string kerbalName = kerbal.name;
                    CivPopKerbalGender gender = CivPopKerbalGender.FEMALE;
                    if (ProtoCrewMember.Gender.Male.Equals(kerbal.gender))
                    {
                        gender = CivPopKerbalGender.MALE;
                    }
                    double birthdate = Planetarium.GetUniversalTime() - 15 * TimeUnit.YEAR - rng.Next(15 * TimeUnit.YEAR);
                    civKerbal = new CivPopKerbal(kerbalName, gender, birthdate, false);
                }
                bool civilian = "Civilian".Equals(kerbal.trait);
                civKerbal.SetCivilian(civilian);
                if (ProtoCrewMember.RosterStatus.Assigned.Equals(kerbal.rosterStatus))
                {
                    repo.Add(civKerbal);
                }
                else
                {
                    repo.Remove(civKerbal);
                }
            }

            foreach (Vessel vessel in FlightGlobals.Vessels)
            {
                CivPopVessel civVessel;
                if (Constants.ValidVessel(vessel))
                {
                    if (/* vessel != null && */ !repo.VesselExists(vessel.id.ToString()))
                    {
                        civVessel = new CivPopVessel(vessel);
                    }
                    else
                    {
                        civVessel = repo.GetVessel(vessel.id.ToString());
                    }
                    civVessel.SetOrbiting(!vessel.LandedOrSplashed);
                    civVessel.SetBody(new Domain.CelestialBody(vessel.mainBody.name, GetBodyType(vessel.mainBody)));

                    foreach (VesselModule module in vessel.vesselModules)
                    {
                        if (module.GetType() == typeof(CivilianPopulationVesselModule))
                        {
                            CivilianPopulationVesselModule civModule = (CivilianPopulationVesselModule)module;
                            civVessel.SetCapacity(civModule.capacity);
                            civVessel.SetAllowDocking(civModule.allowDocking);
                            civVessel.SetAllowBreeding(civModule.allowBreeding);
                        }
                    }

                    foreach (ProtoCrewMember kerbal in vessel.GetVesselCrew())
                    {
                        CivPopKerbal civKerbal = repo.GetKerbal(kerbal.name);
                        if (civKerbal != null)
                        {
                            civKerbal.SetVesselId(vessel.id.ToString());
                        }
                    }
                    repo.Add(civVessel);
                }
            }

            foreach (CivPopVessel civVessel in repo.GetVessels())
            {
                bool found = false;
                foreach (Vessel vessel in FlightGlobals.Vessels)
                {
                    if (vessel != null && vessel.id.ToString().Equals(civVessel.GetId()))
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    repo.Remove(civVessel);
                }
            }
        }

        private Domain.CelestialBodyType GetBodyType(CelestialBody body)
        {
            Domain.CelestialBodyType type = Domain.CelestialBodyType.OTHERS;
            if (body.isHomeWorld)
            {
                type = Domain.CelestialBodyType.HOMEWORLD;
            }
            else if (body.orbit != null
                   && body.orbit.referenceBody != null
                   && body.orbit.referenceBody.isHomeWorld)
            {
                type = Domain.CelestialBodyType.HOMEWORLD_MOON;
            }
            return type;
        }

#if false
        private void log(string message)
        {
            Debug.Log(this.GetType().Name + " - " + message);
        }
#endif
    }
}
