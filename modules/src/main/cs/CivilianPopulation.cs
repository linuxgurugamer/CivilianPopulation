using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using KSP;
using KSP.UI.Screens;

namespace CivilianPopulation
{
    [KSPScenario(ScenarioCreationOptions.AddToAllGames, GameScenes.FLIGHT, GameScenes.TRACKSTATION, GameScenes.SPACECENTER, GameScenes.EDITOR)]
    public class CivilianPopulation : ScenarioModule
    {
        private static ApplicationLauncherButton button = null;
        private static bool windowShown = false;
        private static Rect windowPosition = new Rect(Screen.width / 2 - 250, Screen.height / 2 - 250, 500, 300);

        private static double nextTaxesDate;

        public void Start()
        {
            GameEvents.onGUIApplicationLauncherReady.Add(onAppLauncherReady);//when AppLauncher can take apps, give it OnAppLauncherReady (mine)
            GameEvents.onGUIApplicationLauncherDestroyed.Add(onAppLauncherDestroyed);//Not sure what this does
            nextTaxesDate = getNextTaxesDate();
        }

        public void OnGUI()
        {
            if (windowShown)
            {
                windowPosition = GUILayout.Window(0, windowPosition, drawWindow, "Civilian Population");
            }
        }

        public void FixedUpdate()
        {
            if (HighLogic.CurrentGame.Mode == Game.Modes.CAREER)
            {
                double next = getNextTaxesDate();
                if (nextTaxesDate < next) {
                    nextTaxesDate = next;
                    int numCivilians = 0;
                    foreach (CivilianVessel vessel in getVessels())
                    {
                        numCivilians += vessel.getCivilianCount();
                    }
                    Funding.Instance.AddFunds(numCivilians * 200, TransactionReasons.Progression);
                }
            }
        }

        private double getTimeUntilTaxes()
        {
            return getNextTaxesDate() - Planetarium.GetUniversalTime();
        }

        private double getNextTaxesDate()
        {
            double now = Planetarium.GetUniversalTime();
            double next = 0;
            while (next<now)
            {
                next += 6 * 60 * 60;
            }
            return next;
        }

        private void log(string message)
        {
            Debug.Log(this.GetType().Name + message);
        }

        private void onAppLauncherReady()
        {
            if (ApplicationLauncher.Instance != null && button == null)
            {
                button = ApplicationLauncher.Instance.AddModApplication(
                    onTrue,
                    onFalse,
                    onHover,
                    onHoverOut,
                    onEnable,
                    onDisable,
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

        private void onTrue()
        {
            windowToggle();
        }

        private void onFalse()
        {
            windowToggle();
        }

        private Vector2 scrollPosition;

        private void drawWindow(int windowId)
        {
            GUILayout.BeginVertical();
            scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Width(500), GUILayout.Height(300));

            GUILayout.BeginHorizontal();
            GUILayout.Label("Get universal time : " + Planetarium.GetUniversalTime());
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label("Time until taxes : " + this.getTimeUntilTaxes());
            GUILayout.EndHorizontal();

            foreach (CivilianVessel vessel in getVessels())
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

        private List<CivilianVessel> getVessels()
        {
            List<CivilianVessel> res = new List<CivilianVessel>();
            foreach (Vessel vessel in FlightGlobals.Vessels)
            {
                CivilianVessel civVessel = new CivilianVessel(vessel);
                res.Add(civVessel);
            }
            return res;
        }

        private void windowToggle()
        {
            windowShown = !windowShown;
        }

        private void onHover()
        {
            log(".onHover!");
        }

        private void onHoverOut()
        {
            log(".onHoverOut!");
        }

        private void onEnable()
        {
            log(".onEnable!");
        }

        private void onDisable()
        {
            log(".onDisable!");
        }
    }
}
