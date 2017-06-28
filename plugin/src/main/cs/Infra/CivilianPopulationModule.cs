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
		private CivilianPopulationService service;
		private CivilianPopulationGUI gui;

		public void Start()
		{
            CivilianPopulationAdapter adapter = new KSPCivilianPopulationAdapter();
            this.service = new CivilianPopulationService(adapter);
			this.gui = new CivilianPopulationGUI(service);
		}

		public void OnGUI()
		{
			gui.update();
		}

		public void FixedUpdate()
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
				CivilianVessel civVessel = new CivilianVessel(vessel.GetName(), civilianCount);
				vessels.Add(civVessel);
			}

			CivilianPopulationWorld world = new CivilianPopulationWorld(career, universalTime, vessels);
			service.update(world);
		}

		private void log(string message)
		{
			Debug.Log(this.GetType().Name + message);
		}
    }
}
