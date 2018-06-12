using System;
using System.Collections.Generic;
using System.Linq;
using CivilianPopulation.Domain;
using CivilianPopulation.Domain.Repository;
using CivilianPopulation.Domain.Services;
using KSP.UI.Screens;
using UnityEngine;

namespace CivilianPopulation.GUI
{
    public class CivilianPopulationGUI
    {

		private ApplicationLauncherButton button = null;
		private bool windowShown = false;
        private CivilianPopulationWindow window = CivilianPopulationWindow.EMPTY;
		private Rect windowPosition = new Rect(Screen.width / 2 - 250, Screen.height / 2 - 250, 600, 300);
		private Vector2 scrollPosition;

        private double currentDate;
        private CivPopRepository repo;

        private CheatPanel cheatPanel;
        private CrewPanel crewPanel;
        private VesselsPanel vesselsPanel;

        public CivilianPopulationGUI(CivPopServiceRent rent)
        {
			GameEvents.onGUIApplicationLauncherReady.Add(onAppLauncherReady);//when AppLauncher can take apps, give it OnAppLauncherReady (mine)
			GameEvents.onGUIApplicationLauncherDestroyed.Add(onAppLauncherDestroyed);//Not sure what this does

            cheatPanel = new CheatPanel(rent);
            crewPanel = new CrewPanel();
            vesselsPanel = new VesselsPanel();
		}

        public void update(double currentDate, CivPopRepository repo)
        {
			if (windowShown)
			{
                this.currentDate = currentDate;
                this.repo = repo;
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
					ApplicationLauncher.AppScenes.FLIGHT |
					ApplicationLauncher.AppScenes.MAPVIEW |
					ApplicationLauncher.AppScenes.TRACKSTATION |
					ApplicationLauncher.AppScenes.SPACECENTER,
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
            scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Width(600), GUILayout.Height(300));

            GUILayout.BeginHorizontal();

            if (GUILayout.Button("x", GUILayout.Width(100f)))
            {
                windowShown = false;
            }
	        /*
            if (GUILayout.Button("Cheat", GUILayout.Width(100f)))
            {
                window = CivilianPopulationWindow.CHEAT;
            }
            */
            if (GUILayout.Button("Crew", GUILayout.Width(100f)))
            {
                window = CivilianPopulationWindow.CREW;
            }
            if (GUILayout.Button("Vessels", GUILayout.Width(100f)))
            {
                window = CivilianPopulationWindow.VESSELS;
            }
            GUILayout.EndHorizontal();

            if (window == CivilianPopulationWindow.CHEAT)
            {
                cheatPanel.draw();
            }
            if (window == CivilianPopulationWindow.CREW)
            {
                crewPanel.setCurrentDate(currentDate);
                crewPanel.setRepository(repo);
                crewPanel.draw();
            }
            if (window == CivilianPopulationWindow.VESSELS)
            {
                vesselsPanel.setCurrentDate(currentDate);
                vesselsPanel.setRepository(repo);
                vesselsPanel.draw();
            }

            GUILayout.EndScrollView();
            GUILayout.EndVertical();
            UnityEngine.GUI.DragWindow();
        }

        private void log(string message)
		{
			Debug.Log(this.GetType().Name + " - " + message);
		}
	}
}
