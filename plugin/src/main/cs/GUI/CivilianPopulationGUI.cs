using System;
using System.Collections.Generic;
using System.Linq;
using CivilianPopulation.Domain;
using KSP.UI.Screens;
using UnityEngine;

namespace CivilianPopulation.GUI
{
    public class CivilianPopulationGUI
    {

		private ApplicationLauncherButton button = null;
		private bool windowShown = false;
        private CivilianPopulationWindow window = CivilianPopulationWindow.EMPTY;
		private Rect windowPosition = new Rect(Screen.width / 2 - 250, Screen.height / 2 - 250, 500, 300);
		private Vector2 scrollPosition;

        private double currentDate;
		private List<CivilianVessel> vessels;

        CheatPanel cheatPanel = new CheatPanel();
        CrewPanel crewPanel = new CrewPanel();
        VesselsPanel vesselsPanel = new VesselsPanel();


		public CivilianPopulationGUI()
        {
			GameEvents.onGUIApplicationLauncherReady.Add(onAppLauncherReady);//when AppLauncher can take apps, give it OnAppLauncherReady (mine)
			GameEvents.onGUIApplicationLauncherDestroyed.Add(onAppLauncherDestroyed);//Not sure what this does

            cheatPanel = new CheatPanel();
            crewPanel = new CrewPanel();
            vesselsPanel = new VesselsPanel();
		}

        public void update(double currentDate, List<CivilianVessel> vessels)
        {
			if (windowShown)
			{
                this.currentDate = currentDate;
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

            GUILayout.BeginHorizontal();

            if (GUILayout.Button("x", GUILayout.Width(100f)))
            {
                windowShown = false;
            }
            /*
            if (GUILayout.Button("Cheat", GUILayout.Width(100f)))
            {
                window = CivilianPopulationWindow.CHEAT;
            }*/
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
                crewPanel.draw();
            }
            if (window == CivilianPopulationWindow.VESSELS)
            {
                vesselsPanel.setCurrentDate(currentDate);
                vesselsPanel.setVessels(vessels);
                vesselsPanel.draw();
            }

            GUILayout.EndScrollView();
            GUILayout.EndVertical();
            UnityEngine.GUI.DragWindow();
        }

        private void log(string message)
		{
			Debug.Log(this.GetType().Name + message);
		}
	}
}
