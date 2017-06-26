using System;
using KSP.UI.Screens;
using UnityEngine;

namespace CivilianPopulation
{
    internal class CivilianPopulationUI
    {
        private CivilianPopulationCore core;
        private CivilianPopulationAdapter adapter;

        private ApplicationLauncherButton button = null;
        private bool windowShown = false;
        private Rect windowPosition = new Rect(Screen.width / 2 - 250, Screen.height / 2 - 250, 500, 300);
        private Vector2 scrollPosition;

        public CivilianPopulationUI(CivilianPopulationAdapter adapter, CivilianPopulationCore core)
        {
            this.adapter = adapter;
            this.core = core;

            GameEvents.onGUIApplicationLauncherReady.Add(onAppLauncherReady);//when AppLauncher can take apps, give it OnAppLauncherReady (mine)
            GameEvents.onGUIApplicationLauncherDestroyed.Add(onAppLauncherDestroyed);//Not sure what this does
        }

        public void draw()
        {
            if (windowShown)
            {
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

        private void windowToggle()
        {
            windowShown = !windowShown;
        }

        private void drawWindow(int windowId)
        {
            GUILayout.BeginVertical();
            scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Width(500), GUILayout.Height(300));

            GUILayout.BeginHorizontal();
            GUILayout.Label("Get universal time : " + Planetarium.GetUniversalTime());
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label("Time until taxes : " + core.getTimeUntilTaxes());
            GUILayout.EndHorizontal();

            foreach (CivilianVessel vessel in adapter.getVessels())
            {
                if (vessel.getCivilianCount() > 0)
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Label(vessel.getName() + " : " + vessel.getCivilianCount() + " Civilians");
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
            GUI.DragWindow();
        }

        private void onAppLauncherDestroyed()
        {
            if (ApplicationLauncher.Instance != null && button != null)
            {
                ApplicationLauncher.Instance.RemoveApplication(button);
            }
        }
    }
}