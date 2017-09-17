using System;
using System.Collections.Generic;

using CivilianPopulation.Domain;

namespace CivilianPopulation.Infra
{
    public class CivilianPopulationAdapter
    {
        public CivilianPopulationAdapter()
        {
        }

        internal CivilianVessel asCivilianVessel(Vessel vessel)
        {
            List<CivilianKerbal> civilians = new List<CivilianKerbal>();
			foreach (ProtoCrewMember crew in vessel.GetVesselCrew())
			{
				if (crew.trait == "Civilian")
				{
                    Boolean male = crew.gender == ProtoCrewMember.Gender.Male;
                    CivilianKerbal kerbal = new CivilianKerbal(male);
                    civilians.Add(kerbal);
				}
			}

			ContractorMission mission = null;

			int capacity = 0;
			foreach (VesselModule module in vessel.vesselModules)
			{
				if (module.GetType() == typeof(CivilianPopulationVesselModule))
				{
					CivilianPopulationVesselModule civPopModule = (CivilianPopulationVesselModule)module;
					capacity += civPopModule.getCapacity();
					for (int i = 0; i < civPopModule.getWaitingMales(); i++)
					{
						CivilianKerbal kerbal = new CivilianKerbal(true);
						civilians.Add(kerbal);
					}
					for (int i = 0; i < civPopModule.getWaitingFemales(); i++)
					{
						CivilianKerbal kerbal = new CivilianKerbal(false);
						civilians.Add(kerbal);
					}
					mission = civPopModule.getMission();
				}
			}
			CivilianVesselBuilder builder = new CivilianVesselBuilder();
			builder.named(vessel.GetName())
				   .withADockCapacityOf(capacity)
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

	}
}
