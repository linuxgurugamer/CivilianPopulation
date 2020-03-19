using CivilianPopulation.Domain.Repository;
using CivilianPopulation.Domain.Services;
using ClickThroughFix;
using KSP.UI.Screens;
using System;
using ToolbarControl_NS;
using UnityEngine;

namespace CivilianPopulation.GUI
{

    public class CivilianPopulationGUI
    {

        //private ApplicationLauncherButton button = null;
        private bool windowShown = false;
        private CivilianPopulationWindow window = CivilianPopulationWindow.EMPTY;
        private Rect windowPosition = new Rect(Screen.width / 2 - 250, Screen.height / 2 - 250, 600, 300);
        private Vector2 scrollPosition;

        private double currentDate;
        private CivPopRepository repo;

#if DEBUG
        private CheatPanel cheatPanel;
#endif
        private CrewPanel crewPanel;
        private VesselsPanel vesselsPanel;
        private CivilianPopulationUI timePanel;

        internal static String _AssemblyName { get { return System.Reflection.Assembly.GetExecutingAssembly().GetName().Name; } }
        int baseWindowID;
        public CivilianPopulationGUI(CivPopServiceRent rent)
        {
            InitButton();

#if DEBUG
            cheatPanel = new CheatPanel(rent);
#endif
            crewPanel = new CrewPanel();
            vesselsPanel = new VesselsPanel();
            timePanel = CivilianPopulation.ui;

            baseWindowID = UnityEngine.Random.Range(1000, 2000000) + _AssemblyName.GetHashCode();

        }

        public void update(double currentDate, CivPopRepository repo)
        {
            if (windowShown)
            {
                this.currentDate = currentDate;
                this.repo = repo;
                windowPosition = ClickThruBlocker.GUILayoutWindow(baseWindowID, windowPosition, drawWindow, "Civilian Population GUI");
            }
        }

        ToolbarControl toolbarControl;
        internal const string MODID = "CivPop";
        internal const string MODNAME = "Civilian Population GUI";
        public void InitButton()
        {
            if (toolbarControl == null)
            {
                GameObject gameObject = new GameObject();
                toolbarControl = gameObject.AddComponent<ToolbarControl>();
                toolbarControl.AddToAllToolbars(windowToggle, windowToggle,
                    ApplicationLauncher.AppScenes.FLIGHT |
                        ApplicationLauncher.AppScenes.MAPVIEW |
                        ApplicationLauncher.AppScenes.TRACKSTATION |
                        ApplicationLauncher.AppScenes.SPACECENTER,
                    MODID,
                    "civPopBtn1",
                    "CivilianPopulation/PluginData/GUI/CivPopIcon-38",
                    "CivilianPopulation/PluginData/GUI/CivPopIcon-24",
                    MODNAME
                );
            }
        }


        //private void onAppLauncherDestroyed()
        private void OnDestroy()
        {
            Debug.Log("CivilianPopulationGUI.OnDestroy");
            if (toolbarControl != null)
            {
                Debug.Log("CivilianPopulationGUI.OnDestroy destroying toolbarControl");
                toolbarControl.OnDestroy();
                GameObject.Destroy(toolbarControl);
                toolbarControl = null;
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

            if (GUILayout.Button("Close", GUILayout.Width(100f)))
            {
                windowShown = false;
            }
            if (GUILayout.Button("Time UI", GUILayout.Width(100f)))
            {
                window = CivilianPopulationWindow.TIME;
            }

#if DEBUG
            if (GUILayout.Button("Cheat", GUILayout.Width(100f)))
            {
                window = CivilianPopulationWindow.CHEAT;
            }
#endif
            if (GUILayout.Button("Crew", GUILayout.Width(100f)))
            {
                window = CivilianPopulationWindow.CREW;
            }
            if (GUILayout.Button("Vessels", GUILayout.Width(100f)))
            {
                window = CivilianPopulationWindow.VESSELS;
            }
            GUILayout.EndHorizontal();

#if DEBUG
            if (window == CivilianPopulationWindow.CHEAT)
            {
                cheatPanel.draw();
            }
#endif
            switch (window)
            {
                case CivilianPopulationWindow.CREW:
                    crewPanel.setCurrentDate(currentDate);
                    crewPanel.setRepository(repo);
                    crewPanel.draw();
                    break;
                case CivilianPopulationWindow.TIME:                    
                    timePanel.drawWindow(0);                    
                    break;
                case CivilianPopulationWindow.VESSELS:
                    vesselsPanel.setCurrentDate(currentDate);
                    vesselsPanel.setRepository(repo);
                    vesselsPanel.draw();
                    break;
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
