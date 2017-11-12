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

        private static CivilianPopulationRentService rentService;
		private static CivilianPopulationAdapter adapter;
		private static CivilianPopulationGUI gui;

        protected System.Random rng;

        [KSPField(isPersistant = true, guiActive = false)]
        public string rosterJSON;

		public void Start()
		{
            if (rentService == null)
            {
                rentService = new CivilianPopulationRentService(this.addFunds);
                adapter = new CivilianPopulationAdapter();
                gui = new CivilianPopulationGUI();
            }
            this.rng = new System.Random();
		}

        public void OnGUI()
		{
			gui.update(Planetarium.GetUniversalTime(), getVessels(), getRoster());
		}

		public void FixedUpdate()
        {
            CivilianKerbalRoster roster = getRoster();

            KerbalRoster kspRoster = HighLogic.CurrentGame.CrewRoster;
            ProtoCrewMember.KerbalType type = ProtoCrewMember.KerbalType.Crew;
            //ProtoCrewMember.KerbalType.Applicant;
            //ProtoCrewMember.KerbalType.Crew;
            //ProtoCrewMember.KerbalType.Tourist;
            //ProtoCrewMember.KerbalType.Unowned;

            ProtoCrewMember.RosterStatus[] statuses = {
                ProtoCrewMember.RosterStatus.Assigned,
                ProtoCrewMember.RosterStatus.Available,    
                ProtoCrewMember.RosterStatus.Dead,    
                ProtoCrewMember.RosterStatus.Missing    
            };

            foreach (ProtoCrewMember kerbal in kspRoster.Kerbals(type, statuses))
            {
                if (!roster.exists(kerbal.name))
                {
                    bool isMale = true;
                    if (kerbal.gender.Equals(ProtoCrewMember.Gender.Female))
                    {
                        isMale = false;
                    }
                    double birth = Planetarium.GetUniversalTime() - 15 * TimeUnit.YEAR - rng.Next(15 * TimeUnit.YEAR);
                    CivilianKerbal civKerbal = new CivilianKerbal(kerbal.name, kerbal.trait, isMale, birth, -1);
                    roster.add(civKerbal);
                }
            }

            if (HighLogic.CurrentGame.Mode == Game.Modes.CAREER)
            {
                rentService.update(Planetarium.GetUniversalTime(), getVessels());
            }
            rosterJSON = roster.toString();
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

        private CivilianKerbalRoster getRoster()
        {
            CivilianKerbalRoster res = new CivilianKerbalRoster();
            if (rosterJSON != null)
            {
                res = new CivilianKerbalRoster(rosterJSON.Replace(']', '}').Replace('[', '{'));
            }
            return res;
        }

        private void addFunds(int amount) {
			Funding.Instance.AddFunds(amount, TransactionReasons.Progression);
		}

		private void log(string message)
		{
			Debug.Log(this.GetType().Name + " - " + message);
		}
    }
}
