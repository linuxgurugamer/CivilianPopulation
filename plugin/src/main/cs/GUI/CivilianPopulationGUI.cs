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
        private TimeFormatter formatter;

		private ApplicationLauncherButton button = null;
		private bool windowShown = false;
		private Rect windowPosition = new Rect(Screen.width / 2 - 250, Screen.height / 2 - 250, 500, 300);
		private Vector2 scrollPosition;

        private double currentDate;
		private List<CivilianVessel> vessels;

		public CivilianPopulationGUI()
        {
            this.formatter = new TimeFormatter();

			GameEvents.onGUIApplicationLauncherReady.Add(onAppLauncherReady);//when AppLauncher can take apps, give it OnAppLauncherReady (mine)
			GameEvents.onGUIApplicationLauncherDestroyed.Add(onAppLauncherDestroyed);//Not sure what this does
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

			foreach (CivilianVessel vessel in vessels)
			{
                if (vessel.getCivilianCount() > 0 || vessel.getHousingCapacity() > 0)
                {
					GUILayout.BeginHorizontal();
					GUILayout.Label(getVesselStatus(vessel));
					GUILayout.EndHorizontal();

					GUILayout.BeginHorizontal();
                    GUILayout.Label("M : " + vessel.getMales().Count() 
                               + " - F : " + vessel.getFemales().Count() 
                               + " (" + vessel.getFemales().Where(kerbal => kerbal.getExpectingBirthAt() > 0).Count()+ ")");
					GUILayout.EndHorizontal();

					GUILayout.BeginHorizontal();
					GUILayout.Label("  Housing capacity : " + vessel.getHousingCapacity());
					GUILayout.EndHorizontal();

                    if (vessel.getMission() != null)
                    {
                        GUILayout.BeginHorizontal();
                        GUILayout.Label("  Mission arrival : " + formatter.format(vessel.getMission().getEndDate() - currentDate));
                        GUILayout.EndHorizontal();
                    }
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

        private string getVesselStatus(CivilianVessel vessel)
        {
            string res = vessel.getName();
            res += " - ";
            if (vessel.isOrbiting())
            {
                res += "in orbit around ";
            }
            else
            {
                res += "on surface of ";
            }
            res += vessel.getBody().getName();
			res += " - ";
            res += vessel.getCivilianCount() + " civilian";
            if (vessel.getCivilianCount() > 1)
            {
				res += "s";
			}
            return res;
		}

		private void log(string message)
		{
			Debug.Log(this.GetType().Name + message);
		}
	}
}
