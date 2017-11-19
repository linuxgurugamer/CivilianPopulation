using System;
namespace CivilianPopulation.Domain
{
    public class CivilianPopulationContractorService
    {
        private const double DAY_IN_SECONDS = 60 * 60 * 6;
		private const double MISSION_DURATION = DAY_IN_SECONDS * 85;

        private Action<CivilianVessel, ContractorMission> setMission;
        private Action<CivilianVessel, bool> addCivilian;
		private System.Random rng;

        public CivilianPopulationContractorService(Action<CivilianVessel, ContractorMission> setMission, Action<CivilianVessel, bool> addCivilian)
        {
			this.setMission = setMission;
            this.addCivilian = addCivilian;
			this.rng = new System.Random();
		}

        public void update(double currentDate, CivilianVessel vessel)
		{
            if (vessel.isOrbiting() 
                && vessel.getBody().getType() != CelestialBodyType.OTHERS
                && vessel.isDockingAllowed()
                && vessel.getHousingCapacity() > vessel.getCrewCount())
            {
                if (vessel.getMission() == null)
                {
                    double missionDuration = MISSION_DURATION;
                    if (vessel.getBody().getType() != CelestialBodyType.HOMEWORLD)
                    {
                        missionDuration = missionDuration * 2;
                    }
                    ContractorMission mission = new ContractorMission(currentDate + missionDuration, vessel.getBody().getType());
                    setMission(vessel, mission);
				}
                else
                {
                    if (vessel.getBody().getType() != vessel.getMission().getBody())
                    {
                        setMission(vessel, null);
					}
                    else
                    {
                        if (currentDate > vessel.getMission().getEndDate())
                        {
							if (rng.Next() % 2 == 0)
							{
                                addCivilian(vessel, true);
                            } else{
                                addCivilian(vessel, false);
							}
                            setMission(vessel, null);
                        }
                    }
                }
            }
            else
            {
                setMission(vessel, null);
            }
		}
    }
}
