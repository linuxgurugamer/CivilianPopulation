using System;
using System.Collections.Generic;
using CivilianPopulation.Domain;
using UnityEngine;

namespace CivilianPopulation.Infra
{
    public class CivilianPopulationVesselModule : VesselModule
    {
        [KSPField(isPersistant = true, guiActive = false)]
        public string missionEndDate;
        [KSPField(isPersistant = true, guiActive = false)]
        public int missionTargetType;
		[KSPField(isPersistant = true, guiActive = false)]
		public int waitingMales;
		[KSPField(isPersistant = true, guiActive = false)]
		public int waitingFemales;
		[KSPField(isPersistant = true, guiActive = false)]
        public int capacity;

        private CivilianPopulationContractorService service;
        private CivilianPopulationAdapter adapter = new CivilianPopulationAdapter();

		public void Start()
        {
            if (service == null)
            {
                service = new CivilianPopulationContractorService(setMission, addCivilian);
            }
		}

        public void Update()
        {
        }

        public void FixedUpdate()
        {
            service.update(Planetarium.GetUniversalTime(), adapter.asCivilianVessel(vessel));
            if (FlightGlobals.ActiveVessel != null && FlightGlobals.ActiveVessel.id == vessel.id)
            {
                List<CivilianPopulationDockModule> docks = FlightGlobals.ActiveVessel.FindPartModulesImplementing<CivilianPopulationDockModule>();
                if (docks.Count == 0)
                {
                    this.capacity = 0;
                }
            }
        }

        public ContractorMission getMission()
        {
            ContractorMission mission = null;
            if (missionEndDate != "-1")
            {
                try
                {
                    mission = new ContractorMission(Convert.ToDouble(missionEndDate), adapter.intToBodyType(missionTargetType));
                }
                catch(Exception error)
                {
                    log("error : " + error.Message);
                    log("error : " + error.StackTrace);
                    this.missionEndDate = "-1";
                    mission = null;
                }
            }
            return mission;
        }

        private void setMission(ContractorMission newMission)
        {
            if (newMission != null)
            {
                this.missionEndDate = newMission.getEndDate().ToString();
                this.missionTargetType = adapter.bodyTypeToInt(newMission.getBody());
            }
            else
            {
                this.missionEndDate = "-1";
            }
        }

        public int getCapacity()
        {
            return this.capacity - this.waitingMales - this.getWaitingFemales();
        }

		public int getWaitingMales()
		{
            return this.waitingMales;
		}

		public int getWaitingFemales()
		{
            return this.waitingFemales;
		}

		private void addCivilian(Boolean male)
        {
            if (male) 
            {
                waitingMales = waitingMales + 1;
            } else 
            {
                waitingFemales = waitingFemales + 1;
            }
            log("addNewCivilian : capacity is now " + this.capacity + ", "
				+ this.waitingMales + " male civilians will spawn,"
				+ this.waitingFemales + " female civilians will spawn."
               );
        }

        public void updateCapacity()
        {
            List<CivilianPopulationDockModule> docks = vessel.FindPartModulesImplementing<CivilianPopulationDockModule>();
            if (docks.Count > 0)
            {
                this.capacity = 0;
                foreach (CivilianPopulationDockModule dock in docks)
                {
                    if (dock.isActivated())
                    {
                        int dockCapacity = dock.part.CrewCapacity - dock.part.protoModuleCrew.Count;
						while (this.waitingMales > 0 && dockCapacity > 0)
						{
							log("spawning a civilian.");
							ProtoCrewMember newCivilian = createNewCrewMember("Civilian", true);
							if (dock.part.AddCrewmember(newCivilian))
							{
								vessel.SpawnCrew();
								log(newCivilian.name + " has been placed successfully by placeNewCivilian");
							}
							this.waitingMales = this.waitingMales - 1;
						}
						while (this.waitingFemales > 0 && dockCapacity > 0)
						{
							log("spawning a civilian.");
							ProtoCrewMember newCivilian = createNewCrewMember("Civilian", false);
							if (dock.part.AddCrewmember(newCivilian))
							{
								vessel.SpawnCrew();
								log(newCivilian.name + " has been placed successfully by placeNewCivilian");
							}
							this.waitingFemales = this.waitingFemales - 1;
						}
						capacity += dock.part.CrewCapacity - dock.part.protoModuleCrew.Count;
                    }
                }
            }
        }

        private ProtoCrewMember createNewCrewMember(string kerbalTraitName, Boolean male)
        {
            KerbalRoster roster = HighLogic.CurrentGame.CrewRoster;
            ProtoCrewMember newKerbal = roster.GetNewKerbal(ProtoCrewMember.KerbalType.Crew);
            KerbalRoster.SetExperienceTrait(newKerbal, kerbalTraitName);
            if (male)
            {
                newKerbal.gender = ProtoCrewMember.Gender.Male;
				newKerbal.ChangeName(CrewGenerator.GetRandomName(ProtoCrewMember.Gender.Male));
				log("Created " + newKerbal.name + ", a male " + newKerbal.trait);
			} else
            {
				newKerbal.gender = ProtoCrewMember.Gender.Female;
                newKerbal.ChangeName(CrewGenerator.GetRandomName(ProtoCrewMember.Gender.Female));
				log("Created " + newKerbal.name + ", a female " + newKerbal.trait);
			}
            return newKerbal;
        }

        private void log(string message)
        {
            Debug.Log(this.GetType().Name + " - " + message);
        }
    }
}
