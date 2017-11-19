using System;
using System.Collections.Generic;
using System.Linq;
using CivilianPopulation.Domain;
using CivilianPopulation.GUI;
using UnityEngine;

namespace CivilianPopulation.Infra
{
	[KSPScenario(ScenarioCreationOptions.AddToAllGames, GameScenes.FLIGHT, GameScenes.TRACKSTATION, GameScenes.SPACECENTER, GameScenes.EDITOR)]
	public class CivilianPopulationModule : ScenarioModule
    {

        private static CivilianPopulationRentService rentService;
        private static CivilianPopulationGrowthService growth;
        private static CivilianPopulationContractorService contract;
        private static CivilianPopulationDeathService death;
        private static CivilianPopulationAdapter adapter;
		private static CivilianPopulationGUI gui;

        protected System.Random rng;

        [KSPField(isPersistant = true, guiActive = false)]
        public string rosterJSON;

        private CivilianKerbalRoster roster;
        private List<CivilianVessel> vessels;

        public void Start()
        {
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
        }

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

        private IEnumerable<ProtoCrewMember> getKerbals()
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
            return HighLogic.CurrentGame.CrewRoster.Kerbals(type, statuses);
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

		private void log(string message)
		{
			Debug.Log(this.GetType().Name + " - " + message);
		}
    }
}
