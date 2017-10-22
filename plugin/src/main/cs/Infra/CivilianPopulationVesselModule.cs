using System;
using System.Collections;
using System.Collections.Generic;
using CivilianPopulation.Domain;
using UnityEngine;

namespace CivilianPopulation.Infra
{
    public class CivilianPopulationVesselModule : VesselModule
    {
		[KSPField(isPersistant = true, guiActive = false)]
		public string crewJSON;
		[KSPField(isPersistant = true, guiActive = false)]
		public int capacity;

		[KSPField(isPersistant = true, guiActive = false)]
		public bool allowDocking;
		[KSPField(isPersistant = true, guiActive = false)]
		public bool allowBreeding;
		[KSPField(isPersistant = true, guiActive = false)]
		public string missionEndDate;

        [KSPField(isPersistant = true, guiActive = false)]
		public int missionTargetType;

		private CivilianPopulationGrowthService growth;
		private CivilianPopulationContractorService contract;

		private CivilianPopulationAdapter adapter = new CivilianPopulationAdapter();

        private CivilianKerbalRoster roster;

		public void Start()
        {
			if (growth == null)
			{
				growth = new CivilianPopulationGrowthService(setPregnant, birth);
			}
			if (contract == null)
			{
				contract = new CivilianPopulationContractorService(setMission, addCivilian);
			}
		}

        public void Update()
        {
        }

        public void FixedUpdate()
        {
			// log(vessel.GetName() + " crewJSON -> " + crewJSON);
            if (crewJSON != null)
            {
                roster = new CivilianKerbalRoster(crewJSON.Replace(']', '}').Replace('[', '{'));
            }
            else
            {
                roster = new CivilianKerbalRoster();
            }

			if (FlightGlobals.ActiveVessel != null && FlightGlobals.ActiveVessel.id == vessel.id)
			{
                addAllKSPCrewToCrewData();
                createNewKSPCrewMembersFromCrewData();
                cleanUpCrewDataToMatchKSPCrew();
                this.capacity = updateVesselCapacity();
			}

            growth.update(Planetarium.GetUniversalTime(), adapter.asCivilianVessel(vessel));
			contract.update(Planetarium.GetUniversalTime(), adapter.asCivilianVessel(vessel));

            crewJSON = roster.toString();
			// log(vessel.GetName() + " crewJSON <- " + crewJSON);
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
				catch (Exception error)
				{
					log("error : " + error.Message);
					log("error : " + error.StackTrace);
					this.missionEndDate = "-1";
					mission = null;
				}
			}
			return mission;
		}

		public bool isAllowDocking()
		{
			return allowDocking;
		}

		public bool isAllowBreeding()
		{
			return allowBreeding;
		}

		private void addAllKSPCrewToCrewData()
        {
			foreach (ProtoCrewMember crew in vessel.GetVesselCrew())
			{
				if (!roster.exists(crew.name))
				{
                    roster.add(new CivilianKerbal(crew.name, crew.trait, crew.gender == ProtoCrewMember.Gender.Male, -1));
				}
			}
		}

        private void createNewKSPCrewMembersFromCrewData()
        {
			KerbalRoster kspRoster = HighLogic.CurrentGame.CrewRoster;
			foreach (CivilianKerbal kerbal in roster.list())
			{
                if (!kspRoster.Exists(kerbal.getName()))
                {
					List<CivilianPopulationHousingModule> houses = FlightGlobals.ActiveVessel.FindPartModulesImplementing<CivilianPopulationHousingModule>();
					if (houses.Count > 0)
					{
						foreach (CivilianPopulationHousingModule house in houses)
						{
							if (house.part.CrewCapacity > house.part.protoModuleCrew.Count)
							{
								ProtoCrewMember newCivilian = createNewCrewMember(kerbal.getName(), "Civilian", false);
								if (house.part.AddCrewmember(newCivilian))
								{
									vessel.SpawnCrew();
									log(newCivilian.name + " has been placed successfully by placeNewCivilian");
									break;
								}
							}
						}
					}
				}
			}
		}

        private void cleanUpCrewDataToMatchKSPCrew()
        {
			KerbalRoster kspRoster = HighLogic.CurrentGame.CrewRoster;
            foreach (CivilianKerbal kerbal in roster.list())
            {
                if (!kspRoster.Exists(kerbal.getName()))
                {
                    roster.remove(kerbal.getName());
                }
            }
        }

        private int updateVesselCapacity()
        {
			int res = 0;
			List<CivilianPopulationHousingModule> houses = FlightGlobals.ActiveVessel.FindPartModulesImplementing<CivilianPopulationHousingModule>();
			if (houses.Count > 0)
			{
				foreach (CivilianPopulationHousingModule house in houses)
				{
					res += house.part.CrewCapacity;
				}
			}
            return res;
		}

		public void setAllowDocking(bool allow)
		{
			this.allowDocking = allow;
			List<CivilianPopulationDockModule> docks = FlightGlobals.ActiveVessel.FindPartModulesImplementing<CivilianPopulationDockModule>();
			foreach (CivilianPopulationDockModule dock in docks)
			{
				dock.activated = allow;
			}
		}

		public void setAllowBreeding(bool allow)
		{
			this.allowBreeding = allow;
			List<CivilianPopulationHousingModule> houses = FlightGlobals.ActiveVessel.FindPartModulesImplementing<CivilianPopulationHousingModule>();
			foreach (CivilianPopulationHousingModule house in houses)
			{
				house.activated = allow;
			}
		}

		private ProtoCrewMember createNewCrewMember(string kerbalName, string kerbalTraitName, bool male)
		{
			KerbalRoster kspRoster = HighLogic.CurrentGame.CrewRoster;
			ProtoCrewMember newKerbal = kspRoster.GetNewKerbal(ProtoCrewMember.KerbalType.Crew);
            newKerbal.ChangeName(kerbalName);
			KerbalRoster.SetExperienceTrait(newKerbal, kerbalTraitName);
			if (male)
			{
				newKerbal.gender = ProtoCrewMember.Gender.Male;
			}
			else
			{
				newKerbal.gender = ProtoCrewMember.Gender.Female;
			}
			return newKerbal;
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

		private void addCivilian(bool male)
		{
            if (this.capacity > this.roster.count())
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
				CivilianKerbal kerbal = new CivilianKerbal(kerbalName, "Civilian", male, -1);
				roster.add(kerbal);
				log("addNewCivilian : " + kerbalName + " - " + male + " - " + " -1 ");
			}
		}

		private void birth(CivilianKerbal mother, bool male)
		{
            roster.get(mother.getName()).setExpectingBirthAt(-1);
            addCivilian(male);
            log(mother.getName() + " gave birth");

		}

		private void setPregnant(CivilianKerbal mother, double when)
		{
            roster.get(mother.getName()).setExpectingBirthAt(when);
			log(mother.getName() + " is pregnant");
		}

		private void log(string message)
		{
			Debug.Log(this.GetType().Name + " - " + message);
		}
	}
}



