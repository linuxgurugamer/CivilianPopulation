﻿using System;
namespace CivilianPopulation.Domain
{
    public class CivilianPopulationContractorService
    {
        private const double DAY_IN_SECONDS = 60 * 60 * 6;
		private const double MISSION_DURATION = DAY_IN_SECONDS * 85;

		private Action<ContractorMission> setMission;
        private Action<bool> addCivilian;
		private System.Random rng;

		public CivilianPopulationContractorService(Action<ContractorMission> setMission, Action<bool> addCivilian)
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
					setMission(mission);
				}
                else
                {
                    if (vessel.getBody().getType() != vessel.getMission().getBody())
                    {
						setMission(null);
					}
                    else
                    {
                        if (currentDate > vessel.getMission().getEndDate())
                        {
							if (rng.Next() % 2 == 0)
							{
								addCivilian(true);
                            } else{
								addCivilian(false);
							}
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
