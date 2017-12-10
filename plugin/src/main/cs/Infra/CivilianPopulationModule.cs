using System;
using System.Collections.Generic;
using System.Linq;
using CivilianPopulation.Domain;
using CivilianPopulation.Domain.Repository;
using CivilianPopulation.Domain.Services;
using CivilianPopulation.GUI;
using UnityEngine;

namespace CivilianPopulation.Infra
{
    [KSPScenario(ScenarioCreationOptions.AddToAllGames, GameScenes.FLIGHT, GameScenes.TRACKSTATION, GameScenes.SPACECENTER, GameScenes.EDITOR)]
    public class CivilianPopulationModule : ScenarioModule
    {
        private static CivPopKerbalBuilder builder;
        private static CivPopServiceContractors contractors;
        private static CivPopServiceDeath death;
        private static CivPopServiceGrowth growth;
        private static CivPopServiceRent rent;

        [KSPField(isPersistant = true, guiActive = false)]
        public string repoJSON;

        //        private static CivilianPopulationRentService rentService;
        //        private static CivilianPopulationGrowthService growth;
        //        private static CivilianPopulationContractorService contract;
        //        private static CivilianPopulationDeathService death;
        private static CivilianPopulationAdapter adapter;
        private static CivilianPopulationGUI gui;

        protected System.Random rng;

        [KSPField(isPersistant = true, guiActive = false)]
        public string rosterJSON;

        private CivilianKerbalRoster roster;
        private List<CivilianVessel> vessels;

        public void Start()
        {
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
            /*
            if (rentService == null)
            {
                rentService = new CivilianPopulationRentService(this.addFunds);
            }
            if (contract == null)
            {
                contract = new CivilianPopulationContractorService(setMission, addCivilian);
            }
            if (growth == null)
            {
                growth = new CivilianPopulationGrowthService(setPregnant, birth);
            }
            if (growth == null)
            {
                death = new CivilianPopulationDeathService(kill);
            }
            */
            if (adapter == null)
            {
                adapter = new CivilianPopulationAdapter();
            }
            if (gui == null)
            {
                gui = new CivilianPopulationGUI();
            }
            this.rng = new System.Random();
        }

        public void OnGUI()
        {
            gui.update(Planetarium.GetUniversalTime(), this.vessels, this.roster);
        }

        public void FixedUpdate()
        {
            double now = Planetarium.GetUniversalTime();

            CivPopRepository repo = GetRepository();
            this.UpdateRepository(repo);

            contractors.Update(now, repo);
            death.Update(now, repo);
            growth.Update(now, repo);

            if (HighLogic.CurrentGame.Mode == Game.Modes.CAREER)
            {
                rent.Update(now, repo);
            }

            Vessel vessel = FlightGlobals.ActiveVessel;
            if (vessel != null)
            {
                KillKerbals(repo, vessel);
                CreateKerbals(repo, vessel);
            }
            this.repoJSON = repo.ToJson();

            /* DEPRECATED BELOW !
            this.roster = getRoster();
            updateRoster();
            this.vessels = getVessels();

            if (HighLogic.CurrentGame.Mode == Game.Modes.CAREER)
            {
                rentService.update(Planetarium.GetUniversalTime(), this.vessels);
            }

            foreach (CivilianVessel vessel in this.vessels)
            {
                growth.update(Planetarium.GetUniversalTime(), vessel);
                contract.update(Planetarium.GetUniversalTime(), vessel);
            }
            death.update(Planetarium.GetUniversalTime(), this.roster);

            if (FlightGlobals.ActiveVessel != null)
            {
                spawnKerbals(HighLogic.CurrentGame.CrewRoster, FlightGlobals.ActiveVessel);
                killKerbals(HighLogic.CurrentGame.CrewRoster, FlightGlobals.ActiveVessel);
            }
            rosterJSON = this.roster.toString();
            /**********************/

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

        private CivPopRepository GetRepository()
        {
            CivPopRepository repo = new CivPopRepository();
            if (this.repoJSON != null)
            {
                repo = new CivPopRepository(this.repoJSON);
            }
            return repo;
        }

        private void UpdateRepository(CivPopRepository repo)
        {
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
                if (!repo.KerbalExists(kerbal.name))
                {
                    string kerbalName = kerbal.name;
                    CivPopKerbalGender gender = CivPopKerbalGender.FEMALE;
                    if (ProtoCrewMember.Gender.Male.Equals(kerbal.gender))
                    {
                        gender = CivPopKerbalGender.MALE;
                    }
                    double birthdate = Planetarium.GetUniversalTime() - 15 * TimeUnit.YEAR - rng.Next(15 * TimeUnit.YEAR);
                    bool civilian = false;
                    if ("Civilian".Equals(kerbal.trait))
                    {
                        civilian = true;
                    }
                    CivPopKerbal civKerbal = new CivPopKerbal(kerbalName, gender, birthdate, civilian);
                    repo.Add(civKerbal);
                }
            }

            foreach (Vessel vessel in FlightGlobals.Vessels)
            {
                CivPopVessel civVessel;
                if (!repo.VesselExists(vessel.id.ToString()))
                {
                    civVessel = new CivPopVessel(vessel.id.ToString());
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
                        //log("vessel has civ pop module");
                        CivilianPopulationVesselModule civModule = (CivilianPopulationVesselModule)module;
                        civVessel.SetCapacity(civModule.capacity);
                        civVessel.SetAllowDocking(civModule.allowDocking);
                        civVessel.SetAllowBreeding(civModule.allowBreeding);
                    }
                }

                foreach (ProtoCrewMember kerbal in vessel.GetVesselCrew())
                {
                    CivPopKerbal civKerbal = repo.GetKerbal(kerbal.name);
                    civKerbal.SetVesselId(vessel.id.ToString());
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

        private void KillKerbals(CivPopRepository repo, Vessel vessel)
        {
            foreach (CivPopKerbal current in repo.GetDeadRosterForVessel(vessel.id.ToString()))
            {
                Part part = null;
                foreach (Part p in vessel.parts)
                {
                    foreach (ProtoCrewMember crew in p.protoModuleCrew)
                    {
                        if (crew.name.Equals(current.GetName()))
                        {
                            part.RemoveCrewmember(crew);
                            vessel.RemoveCrew(crew);
                            crew.Die();
                        }
                    }
                }
                repo.Remove(current);
            }
        }

        private void CreateKerbals(CivPopRepository repo, Vessel vessel)
        {
            foreach (CivPopKerbal current in repo.GetLivingRosterForVessel(vessel.id.ToString()))
            {
                ProtoCrewMember crew = vessel.GetVesselCrew().Find(c => c.name.Equals(current.GetName()));
                if (crew == null)
                {
                    List<CivilianPopulationHousingModule> houses = vessel.FindPartModulesImplementing<CivilianPopulationHousingModule>();
                    if (houses.Count > 0)
                    {
                        foreach (CivilianPopulationHousingModule house in houses)
                        {
                            if (house.part.CrewCapacity > house.part.protoModuleCrew.Count)
                            {
                                KerbalRoster kspRoster = HighLogic.CurrentGame.CrewRoster;
                                ProtoCrewMember newKerbal = kspRoster.GetNewKerbal(ProtoCrewMember.KerbalType.Crew);

                                ProtoCrewMember.Gender gender = ProtoCrewMember.Gender.Male;
                                if (current.GetGender().Equals(CivPopKerbalGender.FEMALE))
                                {
                                    gender = ProtoCrewMember.Gender.Female;
                                }

                                while (newKerbal.gender != gender)
                                {
                                    kspRoster.Remove(newKerbal);
                                    kspRoster.GetNewKerbal(ProtoCrewMember.KerbalType.Crew);
                                }

                                if (house.part.AddCrewmember(newKerbal))
                                {
                                    vessel.SpawnCrew();
                                    log("CreateKerbals : " + newKerbal.name + " has been placed successfully");
                                    break;
                                }
                            }
                        }
                    }
                }
                crew = vessel.GetVesselCrew().Find(c => c.name.Equals(current.GetName()));
                if (crew == null)
                {
                    current.SetDead(true);
                    log("CreateKerbals : " + current.GetName() + " died because of a lack of room");
                }
            }
        }

        /* DEPRECATED BELOW ! 

        private CivilianKerbalRoster getRoster()
        {
            CivilianKerbalRoster res = new CivilianKerbalRoster();
            if (rosterJSON != null)
            {
                res = new CivilianKerbalRoster(rosterJSON.Replace(']', '}').Replace('[', '{'));
            }
            return res;
        }

        private void updateRoster()
        {
            foreach (ProtoCrewMember kerbal in getKerbals())
            {
                if (!roster.exists(kerbal.name))
                {
                    bool isMale = true;
                    if (kerbal.gender.Equals(ProtoCrewMember.Gender.Female))
                    {
                        isMale = false;
                    }
                    double birthdate = Planetarium.GetUniversalTime() - 15 * TimeUnit.YEAR - rng.Next(15 * TimeUnit.YEAR);
                    CivilianKerbal civKerbal = new CivilianKerbal(kerbal.name, kerbal.trait, isMale, false, birthdate, -1);
                    roster.add(civKerbal);
                }
            }
        }

        private List<CivilianVessel> getVessels()
        {
            List<CivilianVessel> res = new List<CivilianVessel>();
            foreach (Vessel vessel in FlightGlobals.Vessels)
            {
                res.Add(adapter.asCivilianVessel(vessel, roster));
            }
            return res;
        }

        private void spawnKerbals(KerbalRoster kspRoster, Vessel vessel)
        {
            foreach (CivilianKerbal kerbal in roster.list())
            {
                if (vessel.id.ToString().Equals(kerbal.getVesselId()))
                {
                    ProtoCrewMember.Gender gender = ProtoCrewMember.Gender.Male;
                    if (!kerbal.isMale())
                    {
                        gender = ProtoCrewMember.Gender.Female;
                    }

                    List<CivilianPopulationHousingModule> houses = vessel.FindPartModulesImplementing<CivilianPopulationHousingModule>();
                    if (houses.Count > 0)
                    {
                        foreach (CivilianPopulationHousingModule house in houses)
                        {
                            if (house.part.CrewCapacity > house.part.protoModuleCrew.Count)
                            {
                                ProtoCrewMember newKerbal = kspRoster.GetNewKerbal(ProtoCrewMember.KerbalType.Crew);
                                while (newKerbal.gender != gender)
                                {
                                    kspRoster.Remove(newKerbal);
                                    kspRoster.GetNewKerbal(ProtoCrewMember.KerbalType.Crew);
                                }
                                newKerbal.ChangeName(kerbal.getName());
                                KerbalRoster.SetExperienceTrait(newKerbal, "Civilian");

                                if (house.part.AddCrewmember(newKerbal))
                                {
                                    vessel.SpawnCrew();
                                    log(newKerbal.name + " has been placed successfully");
                                    break;
                                }
                            }
                        }
                    }
                    kerbal.setVesselId(null);
                }
            }
        }

        private void killKerbals(KerbalRoster kspRoster, Vessel vessel)
        {
            foreach (CivilianKerbal kerbal in roster.listDead())
            {
                Part part = null;
                foreach (Part p in vessel.parts)
                {
                    foreach (ProtoCrewMember crew in p.protoModuleCrew)
                    {
                        if (crew.name.Equals(kerbal.getName()))
                        {
                            part.RemoveCrewmember(crew);
                            vessel.RemoveCrew(crew);
                            crew.Die();
                            roster.remove(kerbal.getName());
                        }
                    }
                }
            }
        }

        private void addFunds(int amount) {
			Funding.Instance.AddFunds(amount, TransactionReasons.Progression);
		}

        private void setMission(CivilianVessel vessel, ContractorMission newMission)
        {
            CivilianPopulationVesselModule module = getVesselModule(vessel);
            if (newMission != null)
            {
                module.missionEndDate = newMission.getEndDate().ToString();
                module.missionTargetType = adapter.bodyTypeToInt(newMission.getBody());
            }
            else
            {
                module.missionEndDate = "-1";
            }
        }

        private void addCivilian(CivilianVessel vessel, bool male)
        {
            addCivilian(vessel, male, Planetarium.GetUniversalTime() - 15 * TimeUnit.YEAR - rng.Next(15 * TimeUnit.YEAR));
        }

        private void addCivilian(CivilianVessel vessel, bool male, double birthdate)
        {
            CivilianPopulationVesselModule module = getVesselModule(vessel);
            if (module.capacity > getCrewCount(vessel))
            {
                string kerbalName;
                if (male)
                {
                    kerbalName = CrewGenerator.GetRandomName(ProtoCrewMember.Gender.Male);
                }
                else
                {
                    kerbalName = CrewGenerator.GetRandomName(ProtoCrewMember.Gender.Female);
                }
                CivilianKerbal kerbal = new CivilianKerbal(kerbalName, "Civilian", male, false, birthdate, -1);
                kerbal.setVesselId(vessel.getId().ToString());
                roster.add(kerbal);

                log("addNewCivilian : " + kerbalName + " - " + male + " - " + " -1 ");
            }
        }

        private int getCrewCount(CivilianVessel vessel)
        {
            int res = 0;

            foreach (Vessel current in FlightGlobals.Vessels)
            {
                if (current.id == vessel.getId())
                {
                    res = current.GetCrewCount();
                    break;
                }
            }

            foreach (CivilianKerbal kerbal in this.roster.list())
            {
                if (vessel.getId().ToString().Equals(kerbal.getVesselId()))
                {
                    res++;
                }
            }

            return res;
        }

        private CivilianPopulationVesselModule getVesselModule(CivilianVessel civVessel)
        {
            Vessel vessel = null;
            foreach (Vessel current in FlightGlobals.Vessels)
            {
                if (current.id == civVessel.getId())
                {
                    vessel = current;
                    break;
                }
            }

            CivilianPopulationVesselModule res = null;
            if (vessel != null)
            {
                foreach (VesselModule module in vessel.vesselModules)
                {
                    if (module is CivilianPopulationVesselModule)
                    {
                        res = (CivilianPopulationVesselModule)module;
                    }
                }
            }
            return res;
        }

        private void birth(CivilianVessel vessel, CivilianKerbal mother, bool male)
        {
            roster.get(mother.getName()).setExpectingBirthAt(-1);
            addCivilian(vessel, male, Planetarium.GetUniversalTime());
            log(mother.getName() + " gave birth");

        }

        private void setPregnant(CivilianKerbal mother, double when)
        {
            roster.get(mother.getName()).setExpectingBirthAt(when);
            log(mother.getName() + " is pregnant");
        }

        private void kill(CivilianKerbal kerbal)
        {
            kerbal.setDead(true);
        }
        */

		private void log(string message)
		{
			Debug.Log(this.GetType().Name + " - " + message);
		}
    }
}
