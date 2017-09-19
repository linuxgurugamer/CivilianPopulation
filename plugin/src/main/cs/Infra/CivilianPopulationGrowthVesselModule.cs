using System;
using System.Collections.Generic;
using CivilianPopulation.Domain;
using UnityEngine;

namespace CivilianPopulation.Infra
{
    public class CivilianPopulationGrowthVesselModule : VesselModule
    {
		[KSPField(isPersistant = true, guiActive = false)]
		public string expectations;
		[KSPField(isPersistant = true, guiActive = false)]
		public int waitingMales;
		[KSPField(isPersistant = true, guiActive = false)]
		public int waitingFemales;
		[KSPField(isPersistant = true, guiActive = false)]
		public int capacity;

		private CivilianPopulationGrowthService service;
		private CivilianPopulationAdapter adapter = new CivilianPopulationAdapter();

		public void Start()
		{
			if (service == null)
			{
				service = new CivilianPopulationGrowthService(setPregnant, birth);
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
				List<CivilianPopulationHousingModule> houses = FlightGlobals.ActiveVessel.FindPartModulesImplementing<CivilianPopulationHousingModule>();
				if (houses.Count == 0)
				{
					this.capacity = 0;
				}
			}
		}

		public int getWaitingMales()
		{
			return this.waitingMales;
		}

		public int getWaitingFemales()
		{
			return this.waitingFemales;
		}

        private void birth(CivilianKerbal mother, bool male)
		{
			log(mother.getName() + " is giving birth !");

			Dictionary<string, double> data = getExpectations();
			foreach (ProtoCrewMember crew in vessel.GetVesselCrew())
			{
				if (crew.name.Equals(mother.getName()))
				{
					data[crew.name] = -1;
					if (this.capacity - this.waitingMales - this.waitingFemales > 0)
                    {
                        if (male)
                        {
                            waitingMales = waitingMales + 1;
                        }
                        else
                        {
                            waitingFemales = waitingFemales + 1;
                        }
					}
				}
			}
			save(data);
		}

		private void setPregnant(CivilianKerbal mother, double when)
		{
            log(mother.getName() + " is pregnant !");

			Dictionary<string, double> data = getExpectations();
			foreach (ProtoCrewMember crew in vessel.GetVesselCrew())
            {
                if (crew.name.Equals(mother.getName()))
                {
                    data[crew.name] = when;
                }
            }
            save(data);
		}

		public void updateCapacity()
		{
			List<CivilianPopulationHousingModule> houses = vessel.FindPartModulesImplementing<CivilianPopulationHousingModule>();
			if (houses.Count > 0)
			{
				this.capacity = 0;
				foreach (CivilianPopulationHousingModule house in houses)
				{
					int dockCapacity = house.part.CrewCapacity - house.part.protoModuleCrew.Count;
					while (this.waitingMales > 0 && dockCapacity > 0)
					{
						log("spawning a civilian.");
						ProtoCrewMember newCivilian = createNewCrewMember("Civilian", true);
						if (house.part.AddCrewmember(newCivilian))
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
						if (house.part.AddCrewmember(newCivilian))
						{
							vessel.SpawnCrew();
							log(newCivilian.name + " has been placed successfully by placeNewCivilian");
						}
						this.waitingFemales = this.waitingFemales - 1;
					}
					capacity += house.part.CrewCapacity - house.part.protoModuleCrew.Count;
				}
			}
		}

		private ProtoCrewMember createNewCrewMember(string kerbalTraitName, bool male)
		{
			KerbalRoster roster = HighLogic.CurrentGame.CrewRoster;
			ProtoCrewMember newKerbal = roster.GetNewKerbal(ProtoCrewMember.KerbalType.Crew);
			KerbalRoster.SetExperienceTrait(newKerbal, kerbalTraitName);
			if (male)
			{
				newKerbal.gender = ProtoCrewMember.Gender.Male;
				newKerbal.ChangeName(CrewGenerator.GetRandomName(ProtoCrewMember.Gender.Male));
				log("Created " + newKerbal.name + ", a male " + newKerbal.trait);
			}
			else
			{
				newKerbal.gender = ProtoCrewMember.Gender.Female;
				newKerbal.ChangeName(CrewGenerator.GetRandomName(ProtoCrewMember.Gender.Female));
				log("Created " + newKerbal.name + ", a female " + newKerbal.trait);
			}
			return newKerbal;
		}

        public Dictionary<string, double> getExpectations()
        {
            string saved = expectations;
            Dictionary<string, double> res = new Dictionary<string, double>();
            if (saved != null)
            {
                foreach (string token in saved.Split(','))
                {
                    if (token != null && token.Contains("@"))
                    {
                        string[] data = token.Split('@');
                        res.Add(data[0], Convert.ToDouble(data[1]));
                    }
                }
            }
            return res;
        }

		private void save(Dictionary<string, double> data)
		{
            string res = "";
            if (data != null && data.Count > 0)
            {
				foreach (KeyValuePair<string, double> entry in data)
				{
                    res += "," + entry.Key + "@" + entry.Value.ToString();
				}
            } else
            {
                res = "-";
            }
            expectations = res.Substring(1);
		}

		private void log(string message)
		{
			Debug.Log(this.GetType().Name + " - " + message);
		}
	}
}
