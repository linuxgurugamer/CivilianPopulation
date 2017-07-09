using System;
namespace CivilianPopulation.Domain
{
    public class CivilianPopulationContractorService
    {
        private const double DAY_IN_SECONDS = 60 * 60 * 6;
		private const double MISSION_DURATION = DAY_IN_SECONDS * 85;

		private Action<ContractorMission> setMission;
        private Action addCivilian;

        public CivilianPopulationContractorService(Action<ContractorMission> setMission, Action addCivilian)
        {
			this.setMission = setMission;
            this.addCivilian = addCivilian;
        }

        public void update(double currentDate, CivilianVessel vessel)
		{
            if (vessel.isOrbiting() 
                && vessel.getBody() != CelestialBodyType.OTHERS
                && vessel.getDocksCapacity() > 0)
            {
                if (vessel.getMission() == null)
                {
                    double missionDuration = MISSION_DURATION;
                    if (vessel.getBody() != CelestialBodyType.HOMEWORLD)
                    {
                        missionDuration = missionDuration * 2;
                    }
                    ContractorMission mission = new ContractorMission(currentDate + missionDuration, vessel.getBody());
					setMission(mission);
				}
                else
                {
                    if (vessel.getBody() != vessel.getMission().getBody())
                    {
						setMission(null);
					}
                    else
                    {
                        if (currentDate > vessel.getMission().getEndDate())
                        {
                            addCivilian();
                            setMission(null);
                        }
                    }
                }
            }
            else
            {
                setMission(null);
            }
		}
    }
}
