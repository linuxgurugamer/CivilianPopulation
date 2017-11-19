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

        internal CivilianVessel asCivilianVessel(Vessel vessel, CivilianKerbalRoster roster)
        {
			//log("-> asCivilianVessel(" + vessel + ")");
			//log("GetName is : " + vessel.GetName());
            //log("GetDisplayName is : " + vessel.GetDisplayName());
			//log("id is : " + vessel.id);
			ContractorMission mission = null;
            List<CivilianKerbal> crew = new List<CivilianKerbal>();
			int capacity = 0;
            bool allowDocking = false;
            bool allowBreeding = false;
			foreach (VesselModule module in vessel.vesselModules)
            {
				if (module.GetType() == typeof(CivilianPopulationVesselModule))
                {
					//log("vessel has civ pop module");
					CivilianPopulationVesselModule civModule = (CivilianPopulationVesselModule)module;
					capacity = civModule.capacity;
                    mission = civModule.getMission();

                    allowDocking = civModule.isAllowDocking();
                    allowBreeding = civModule.isAllowBreeding();

					//log("mission is : " + mission);
                    /*
					CivilianKerbalRoster roster = new CivilianKerbalRoster(civModule.crewJSON);
					//log("crewData is : " + crewData);
					if (roster.count() > 0)
                    {
						foreach (CivilianKerbal kerbal in roster.list())
                        {
							//log("kerbal is : " + kerbal);
							crew.Add(kerbal);
                        }
                    }*/
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
            builder.identified(vessel.id)
                   .named(vessel.GetName())
                   .withAHousingCapacityOf(capacity)
                   .allowingDocking(allowDocking)
                   .allowingBreeding(allowBreeding)
				   .inOrbit(!vessel.LandedOrSplashed)
				   .on(new Domain.CelestialBody(vessel.mainBody.name, getBodyType(vessel.mainBody)))
				   .targetedBy(mission);
			CivilianVessel civVessel = builder.build();

            foreach (ProtoCrewMember member in vessel.GetVesselCrew())
            {
                civVessel.addCrew(roster.get(member.name));
            }
			//log("<- asCivilianVessel(" + vessel + ")");
            return civVessel;
		}

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
