using System;
using System.Collections.Generic;
using CivilianPopulation.Domain;
using UnityEngine;

namespace CivilianPopulation.Infra
{
    public class CivilianPopulationAdapter
    {
        public CivilianPopulationAdapter()
        {
        }

		/*
[LOG 08:53:09.367] CivilianPopulationAdapter - -> asCivilianVessel(University of Duna (unloaded) (Vessel))
[LOG 08:53:09.367] CivilianPopulationAdapter - GetName is : University of Duna
[LOG 08:53:09.367] CivilianPopulationAdapter - GetDisplayName is : University of Duna
[LOG 08:53:09.367] CivilianPopulationAdapter - id is : b544f275-f2de-4150-8e50-aa0eb1542973
[LOG 08:53:09.367] CivilianPopulationAdapter - vessel has civ pop module
[LOG 08:53:09.367] CivilianPopulationAdapter - crewJSON is : null
[LOG 08:53:09.367] CivilianPopulationAdapter - mission is : 
[LOG 08:53:09.367] CivilianPopulationAdapter - crewData is : 
[LOG 08:53:09.367] CivilianPopulationAdapter - capacity is : 0
[LOG 08:53:09.367] CivilianPopulationAdapter - GetName is : University of Duna
[LOG 08:53:09.367] CivilianPopulationAdapter - dockCapacity is : 0
[LOG 08:53:09.367] CivilianPopulationAdapter - LandedOrSplashed is : True
[LOG 08:53:09.367] CivilianPopulationAdapter - LandedOrSplashed is : True
[LOG 08:53:09.367] CivilianPopulationAdapter - vessel.mainBody.name is : Duna
[LOG 08:53:09.367] CivilianPopulationAdapter - mission is : 
[LOG 08:53:09.367] CivilianPopulationAdapter - crew is : System.Collections.Generic.List`1[CivilianPopulation.Domain.CivilianKerbal]
[LOG 08:53:09.367] CivilianPopulationAdapter - <- asCivilianVessel(University of Duna (unloaded) (Vessel))
*/
		internal CivilianVessel asCivilianVessel(Vessel vessel)
        {
			//log("-> asCivilianVessel(" + vessel + ")");
			//log("GetName is : " + vessel.GetName());
            //log("GetDisplayName is : " + vessel.GetDisplayName());
			//log("id is : " + vessel.id);
			ContractorMission mission = null;
            List<CivilianKerbal> crew = new List<CivilianKerbal>();
            int capacity = 0;
			int dockCapacity = 0;

			foreach (VesselModule module in vessel.vesselModules)
            {
				if (module.GetType() == typeof(CivilianPopulationVesselModule))
                {
					//log("vessel has civ pop module");
					CivilianPopulationVesselModule civModule = (CivilianPopulationVesselModule)module;

                    CivilianKerbalRoster roster = new CivilianKerbalRoster(civModule.crewJSON);
                    mission = civModule.getMission();
					//log("mission is : " + mission);
					//log("crewData is : " + crewData);
                    if (roster.count() > 0)
                    {
						foreach (CivilianKerbal kerbal in roster.list())
                        {
							//log("kerbal is : " + kerbal);
							crew.Add(kerbal);
                        }
                        capacity = civModule.capacity - crew.Count;
                    }
					//log("capacity is : " + capacity);
					if (civModule.dockActivated)
                    {
						//log("capacity is : " + capacity);
						dockCapacity = capacity;
                    }
				}
			}

			//log("GetName is : " + vessel.GetName());
			//log("dockCapacity is : " + dockCapacity);
			//log("LandedOrSplashed is : " + vessel.LandedOrSplashed);
			//log("LandedOrSplashed is : " + vessel.LandedOrSplashed);
			//log("vessel.mainBody.name is : " + vessel.mainBody.name);
			//log("mission is : " + mission);
			//log("crew is : " + crew);

			CivilianVesselBuilder builder = new CivilianVesselBuilder();
			builder.named(vessel.GetName())
				   .withADockCapacityOf(dockCapacity)
				   .inOrbit(!vessel.LandedOrSplashed)
				   .on(new Domain.CelestialBody(vessel.mainBody.name, getBodyType(vessel.mainBody)))
				   .targetedBy(mission);
			CivilianVessel civVessel = builder.build();
            foreach (CivilianKerbal kerbal in crew)
            {
                if (kerbal.getTrait() == "Civilian")
                {
                    civVessel.addCivilian(kerbal);
                }
			}
			//log("<- asCivilianVessel(" + vessel + ")");
            return civVessel;
		}
		/*
        internal CivilianVessel asCivilianVesselOld(Vessel vessel)
        {
            List<CivilianKerbal> civilians = new List<CivilianKerbal>();
			foreach (ProtoCrewMember crew in vessel.GetVesselCrew())
			{
				if (crew.trait == "Civilian")
				{
                    bool male = crew.gender == ProtoCrewMember.Gender.Male;
                    CivilianKerbal kerbal = new CivilianKerbal(crew.name, male, -1);
                    civilians.Add(kerbal);
				}
			}

			ContractorMission mission = null;

			int dockCapacity = 0;
            int maleIdx = 0;
            int femaleIdx = 0;
            foreach (VesselModule module in vessel.vesselModules)
            {
                if (module.GetType() == typeof(CivilianPopulationContractorVesselModule))
                {
                    CivilianPopulationContractorVesselModule civPopModule = (CivilianPopulationContractorVesselModule)module;
                    dockCapacity += civPopModule.getCapacity();
                    for (int i = 0; i < civPopModule.getWaitingMales(); i++)
                    {
                        CivilianKerbal kerbal = new CivilianKerbal("male-" + maleIdx, true, -1);
                        maleIdx++;
                        civilians.Add(kerbal);
                    }
                    for (int i = 0; i < civPopModule.getWaitingFemales(); i++)
                    {
                        CivilianKerbal kerbal = new CivilianKerbal("female-" + femaleIdx, false, -1);
                        femaleIdx++;
                        civilians.Add(kerbal);
                    }
                    mission = civPopModule.getMission();
                }

                if (module.GetType() == typeof(CivilianPopulationGrowthVesselModule))
                {
                    CivilianPopulationGrowthVesselModule civPopModule = (CivilianPopulationGrowthVesselModule)module;
                    for (int i = 0; i < civPopModule.getWaitingMales(); i++)
                    {
                        CivilianKerbal kerbal = new CivilianKerbal("male-" + maleIdx, true, -1);
                        maleIdx++;
                        civilians.Add(kerbal);
                    }
                    for (int i = 0; i < civPopModule.getWaitingFemales(); i++)
                    {
                        CivilianKerbal kerbal = new CivilianKerbal("female-" + femaleIdx, false, -1);
                        femaleIdx++;
                        civilians.Add(kerbal);
                    }

                    Dictionary<string, double> data = civPopModule.getExpectations();
                    if (data != null && data.Count > 0)
                    {
                        foreach (CivilianKerbal kerbal in civilians)
                        {
                            if (data.ContainsKey(kerbal.getName()))
                            {
                                kerbal.setExpectingBirthAt(data[kerbal.getName()]);
                            }
                        }
                    }
                }
            }
			CivilianVesselBuilder builder = new CivilianVesselBuilder();
			builder.named(vessel.GetName())
				   .withADockCapacityOf(dockCapacity)
				   .inOrbit(!vessel.LandedOrSplashed)
                   .on(new Domain.CelestialBody(vessel.mainBody.name, getBodyType(vessel.mainBody)))
				   .targetedBy(mission);
			CivilianVessel civVessel = builder.build();
            foreach (CivilianKerbal kerbal in civilians)
            {
				civVessel.addCivilian(kerbal);
            }
            return civVessel;
		}
*/

		private CivilianPopulation.Domain.CelestialBodyType getBodyType(CelestialBody body)
		{
			CivilianPopulation.Domain.CelestialBodyType type = CivilianPopulation.Domain.CelestialBodyType.OTHERS;
			if (body.isHomeWorld)
			{
				type = CivilianPopulation.Domain.CelestialBodyType.HOMEWORLD;
			}
			else if (body.orbit != null
				   && body.orbit.referenceBody != null
				   && body.orbit.referenceBody.isHomeWorld)
			{
				type = CivilianPopulation.Domain.CelestialBodyType.HOMEWORLD_MOON;
			}
			return type;
		}

        public int bodyTypeToInt(CivilianPopulation.Domain.CelestialBodyType type)
        {
            int res = 0;
            if (type == CivilianPopulation.Domain.CelestialBodyType.HOMEWORLD)
            {
                res = 1;
            }
            else if (type == CivilianPopulation.Domain.CelestialBodyType.HOMEWORLD_MOON)
            {
                res = 2;
            }
            return res;
        }

		public CivilianPopulation.Domain.CelestialBodyType intToBodyType(int type)
		{
            CivilianPopulation.Domain.CelestialBodyType res = CivilianPopulation.Domain.CelestialBodyType.OTHERS;
			if (type == 1)
			{
				res = CivilianPopulation.Domain.CelestialBodyType.HOMEWORLD;
			}
			else if (type == 2)
			{
				res = CivilianPopulation.Domain.CelestialBodyType.HOMEWORLD_MOON;
			}
			return res;
		}

		private void log(string message)
		{
			Debug.Log(this.GetType().Name + " - " + message);
		}
	}
}
