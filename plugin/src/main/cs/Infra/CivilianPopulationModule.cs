using System;
using System.Collections.Generic;
using CivilianPopulation.Domain;
using CivilianPopulation.GUI;
using UnityEngine;

namespace CivilianPopulation.Infra
{
	[KSPScenario(ScenarioCreationOptions.AddToAllGames, GameScenes.FLIGHT, GameScenes.TRACKSTATION, GameScenes.SPACECENTER, GameScenes.EDITOR)]
	public class CivilianPopulationModule : ScenarioModule
    {
		private static CivilianPopulationService service;
		private static CivilianPopulationGUI gui;

		public void Start()
		{
            if (service == null)
            {
                service = new CivilianPopulationService(this.addFunds);
                gui = new CivilianPopulationGUI(service);
            }
		}

		public void OnGUI()
		{
			gui.update();
		}

		public void FixedUpdate()
        {
            CivilianPopulationWorld world = getWorldFromGame();
            service.update(world);
        }

        private static CivilianPopulationWorld getWorldFromGame()
        {
            bool career = HighLogic.CurrentGame.Mode == Game.Modes.CAREER;
            double universalTime = Planetarium.GetUniversalTime();
            List<CivilianVessel> vessels = new List<CivilianVessel>();
            foreach (Vessel vessel in FlightGlobals.Vessels)
            {
                int civilianCount = 0;
                foreach (ProtoCrewMember crew in vessel.GetVesselCrew())
                {
                    if (crew.trait == "Civilian")
                    {
                        civilianCount++;
                    }
                }

                bool hasCivilianDocks = false;
                foreach (VesselModule module in vessel.vesselModules)
                {
                    if (module.GetType() == typeof(CivilianPopulationVesselModule))
                    {
                        CivilianPopulationVesselModule civPopModule = (CivilianPopulationVesselModule)module;
                        if (civPopModule.hasCivilianDocks)
                        {
                            hasCivilianDocks = true;
                        }
                    }
                }
                CivilianVessel civVessel = new CivilianVessel(vessel.GetName(), civilianCount, hasCivilianDocks);
                vessels.Add(civVessel);
            }

            CivilianPopulationWorld world = new CivilianPopulationWorld(career, universalTime, vessels);
            return world;
        }

        private void addFunds(int amount) {
			Funding.Instance.AddFunds(amount, TransactionReasons.Progression);
		}

		private void log(string message)
		{
			Debug.Log(this.GetType().Name + message);
		}
    }
}
