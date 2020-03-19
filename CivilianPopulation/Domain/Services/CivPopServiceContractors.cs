using CivilianPopulation.Domain.Repository;
using System.Linq;

namespace CivilianPopulation.Domain.Services
{
    public class CivPopServiceContractors
    {
        private const string HOMEWORLD = "HOMEWORLD";
        private const string MOON = "MOON";

        private const double MISSION_DURATION = TimeUnit.DAY * 85;

        private CivPopKerbalBuilder builder;

        public CivPopServiceContractors(CivPopKerbalBuilder builder)
        {
            this.builder = builder;
        }

        public void Update(double date, CivPopRepository repo)
        {
            foreach (CivPopVessel vessel in repo.GetVessels())
            {
                if (vessel.IsOrbiting()
                    && vessel.GetBody().getType() != CelestialBodyType.OTHERS
                    && vessel.IsAllowDocking()
                    && vessel.GetCapacity() > repo.GetLivingRosterForVessel(vessel.GetId()).Count()
                   )
                {
                    if (vessel.GetMissionArrival() < 0)
                    {
                        if (vessel.GetBody().getType() != CelestialBodyType.HOMEWORLD)
                        {
                            vessel.SetMissionArrival(date + MISSION_DURATION * 2);
                            vessel.SetMissionType(MOON);
                        }
                        else
                        {
                            vessel.SetMissionArrival(date + MISSION_DURATION);
                            vessel.SetMissionType(HOMEWORLD);
                        }
                    }
                    else
                    {
                        if (HOMEWORLD.Equals(vessel.GetMissionType())
                            && !vessel.GetBody().getType().Equals(CelestialBodyType.HOMEWORLD))
                        {
                            CancelMission(vessel);
                        }
                        else if (MOON.Equals(vessel.GetMissionType())
                                 && !vessel.GetBody().getType().Equals(CelestialBodyType.HOMEWORLD_MOON))
                        {
                            CancelMission(vessel);
                        }
                        else
                        {
                            if (date > vessel.GetMissionArrival())
                            {
                                CivPopKerbal kerbal = builder.build(date);
                                kerbal.SetVesselId(vessel.GetId());
                                repo.Add(kerbal);
                                CancelMission(vessel);
                            }
                        }
                    }
                }
                else
                {
                    CancelMission(vessel);
                }
            }
        }

        private void CancelMission(CivPopVessel vessel)
        {
            vessel.SetMissionArrival(-1);
            vessel.SetMissionType(null);
        }
    }
}
