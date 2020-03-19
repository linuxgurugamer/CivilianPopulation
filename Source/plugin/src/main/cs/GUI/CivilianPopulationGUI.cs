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

        private CheatPanel cheatPanel;
        private CrewPanel crewPanel;
        private VesselsPanel vesselsPanel;

        internal static String _AssemblyName { get { return System.Reflection.Assembly.GetExecutingAssembly().GetName().Name; } }
        int baseWindowID;
        public CivilianPopulationGUI(CivPopServiceRent rent)
        {
#if false
            GameEvents.onGUIApplicationLauncherReady.Add(onAppLauncherReady);//when AppLauncher can take apps, give it OnAppLauncherReady (mine)
			GameEvents.onGUIApplicationLauncherDestroyed.Add(onAppLauncherDestroyed);//Not sure what this does
#endif
            InitButton();

            cheatPanel = new CheatPanel(rent);
            crewPanel = new CrewPanel();
            vesselsPanel = new VesselsPanel();

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
#if false
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
#endif
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
#if false
            if (ApplicationLauncher.Instance != null && button != null)
			{
				ApplicationLauncher.Instance.RemoveApplication(button);
			}
#endif
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
#if true
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
