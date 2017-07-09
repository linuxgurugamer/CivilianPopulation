using System;
using System.Collections.Generic;
using CivilianPopulation.Domain;
using KSP.UI.Screens;
using UnityEngine;

namespace CivilianPopulation.GUI
{
    public class CivilianPopulationGUI
    {
        private TimeFormatter formatter;

		private ApplicationLauncherButton button = null;
		private bool windowShown = false;
		private Rect windowPosition = new Rect(Screen.width / 2 - 250, Screen.height / 2 - 250, 500, 300);
		private Vector2 scrollPosition;

		private List<CivilianVessel> vessels;

		public CivilianPopulationGUI()
        {
            this.formatter = new TimeFormatter();

			GameEvents.onGUIApplicationLauncherReady.Add(onAppLauncherReady);//when AppLauncher can take apps, give it OnAppLauncherReady (mine)
			GameEvents.onGUIApplicationLauncherDestroyed.Add(onAppLauncherDestroyed);//Not sure what this does
		}

        public void update(List<CivilianVessel> vessels)
        {
			if (windowShown)
			{
				this.vessels = vessels;
				windowPosition = GUILayout.Window(0, windowPosition, drawWindow, "Civilian Population");
			}
		}

		private void onAppLauncherReady()
		{
			if (ApplicationLauncher.Instance != null && button == null)
			{
				button = ApplicationLauncher.Instance.AddModApplication(
					windowToggle,
					windowToggle,
					null,
					null,
					null,
					null,
					ApplicationLauncher.AppScenes.ALWAYS,
					GameDatabase.Instance.GetTexture("CivilianPopulation/GUI/CivPopIcon", false)
				);
			}
		}

		private void onAppLauncherDestroyed()
		{
			if (ApplicationLauncher.Instance != null && button != null)
			{
				ApplicationLauncher.Instance.RemoveApplication(button);
			}
		}

		private void windowToggle()
		{
			windowShown = !windowShown;
		}

		private void drawWindow(int windowId)
		{
			GUILayout.BeginVertical();
			scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Width(500), GUILayout.Height(300));

			foreach (CivilianVessel vessel in vessels)
			{
				GUILayout.BeginHorizontal();
				GUILayout.Label(vessel.getName());
				GUILayout.EndHorizontal();

				GUILayout.BeginHorizontal();
				GUILayout.Label("  Civilians : " + vessel.getCivilianCount());
				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal();
				GUILayout.Label("  Docks capacity : " + vessel.getDocksCapacity());
				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal();
				GUILayout.Label("  In orbit ? " + vessel.isOrbiting());
				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal();
				GUILayout.Label("  Body type : " + getCelestialBodyLabel(vessel.getBody()));
				GUILayout.EndHorizontal();

				if (vessel.getMission() != null)
                {
                    ContractorMission mission = vessel.getMission();
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("  Mission arrival : " + formatter.format(mission.getEndDate()));
                    GUILayout.EndHorizontal();
					GUILayout.BeginHorizontal();
                    GUILayout.Label("  Mission destination : " + getCelestialBodyLabel(mission.getBody()));
                    GUILayout.EndHorizontal();
				}
			}

			GUILayout.BeginHorizontal();
			if (GUILayout.Button("Close", GUILayout.Width(200f)))
			{
				windowShown = false;
			}
			GUILayout.EndHorizontal();
			GUILayout.EndScrollView();
			GUILayout.EndVertical();
			UnityEngine.GUI.DragWindow();
		}

        private string getCelestialBodyLabel(CivilianPopulation.Domain.CelestialBodyType type) 
        {
			string bodyType = "Outer world";
			if (type == CivilianPopulation.Domain.CelestialBodyType.HOMEWORLD)
				bodyType = "Homeworld";
			if (type == CivilianPopulation.Domain.CelestialBodyType.HOMEWORLD_MOON)
				bodyType = "Homeworld moon";
            return bodyType;
		}

		private void log(string message)
		{
			Debug.Log(this.GetType().Name + message);
		}
	}
}