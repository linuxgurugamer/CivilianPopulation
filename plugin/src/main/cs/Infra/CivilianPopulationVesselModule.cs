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
		public int waiting;
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

            if (FlightGlobals.ActiveVessel.id == vessel.id)
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
                    log(" error : " + error.Message);
					log(" error : " + error.StackTrace);
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
            return this.capacity - this.waiting;
		}

        public int getWaiting()
        {
            return this.waiting;
        }

		private void addCivilian()
		{
            waiting = waiting + 1;
            log(" - addNewCivilian : capacity is now " + this.capacity + ", " + this.waiting + " civilians will spawn.");
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
						while (this.waiting > 0 && dockCapacity > 0)
						{
							log(" - spawning a civilian.");
							ProtoCrewMember newCivilian = createNewCrewMember("Civilian");
							if (dock.part.AddCrewmember(newCivilian))
							{
								vessel.SpawnCrew();
								Debug.Log(this.GetType().Name + newCivilian.name + " has been placed successfully by placeNewCivilian");
							}
							this.waiting = this.waiting - 1;
						}
						capacity += dock.part.CrewCapacity - dock.part.protoModuleCrew.Count;
					}
				}
			}
		}

        private ProtoCrewMember createNewCrewMember(string kerbalTraitName)
		{
			KerbalRoster roster = HighLogic.CurrentGame.CrewRoster;
			ProtoCrewMember newKerbal = roster.GetNewKerbal(ProtoCrewMember.KerbalType.Crew);
			KerbalRoster.SetExperienceTrait(newKerbal, kerbalTraitName);//Set the Kerbal as the specified role (kerbalTraitName)
			Debug.Log(this.GetType().Name + "Created " + newKerbal.name + ", a " + newKerbal.trait);
			return newKerbal;
		}

		private void log(string message)
        {
            Debug.Log(this.GetType().Name + message);
        }
    }
}
