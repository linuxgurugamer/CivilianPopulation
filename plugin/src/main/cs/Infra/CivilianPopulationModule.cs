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
		private static CivilianPopulationRentService service;
		private static CivilianPopulationAdapter adapter;
		private static CivilianPopulationGUI gui;

		public void Start()
		{
            if (service == null)
            {
                service = new CivilianPopulationRentService(this.addFunds);
                adapter = new CivilianPopulationAdapter();
                gui = new CivilianPopulationGUI();
            }
		}

        public void OnGUI()
		{
			gui.update(Planetarium.GetUniversalTime(), getVessels());
		}

		public void FixedUpdate()
        {
            if (HighLogic.CurrentGame.Mode == Game.Modes.CAREER)
            {
                service.update(Planetarium.GetUniversalTime(), getVessels());
            }
        }

        private List<CivilianVessel> getVessels()
        {
			List<CivilianVessel> vessels = new List<CivilianVessel>();
			foreach (Vessel vessel in FlightGlobals.Vessels)
			{
                vessels.Add(adapter.asCivilianVessel(vessel));
			}
            return vessels;
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
