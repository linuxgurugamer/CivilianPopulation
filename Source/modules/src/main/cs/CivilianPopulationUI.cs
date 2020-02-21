using ClickThroughFix;
using KSP.UI.Screens;
using System;
using System.Text;
using ToolbarControl_NS;
using UnityEngine;

namespace CivilianPopulation
{

    internal class CivilianPopulationUI
    {
        private CivilianPopulationCore core;
        private CivilianPopulationAdapter adapter;

        //private ApplicationLauncherButton button = null;
        private bool windowShown = false;
        private Rect windowPosition = new Rect(Screen.width / 2 - 250, Screen.height / 2 - 250, 500, 300);
        private Vector2 scrollPosition;

        internal static String _AssemblyName { get { return System.Reflection.Assembly.GetExecutingAssembly().GetName().Name; } }
        int baseWindowID;

        public CivilianPopulationUI(CivilianPopulationAdapter adapter, CivilianPopulationCore core)
        {
            this.adapter = adapter;
            this.core = core;

#if false
            GameEvents.onGUIApplicationLauncherReady.Add(onAppLauncherReady);//when AppLauncher can take apps, give it OnAppLauncherReady (mine)
            GameEvents.onGUIApplicationLauncherDestroyed.Add(onAppLauncherDestroyed);//Not sure what this does
#endif
            InitToolbarButton();
            baseWindowID = UnityEngine.Random.Range(1000, 2000000) + _AssemblyName.GetHashCode();

        }

        public void draw()
        {
            if (windowShown)
            {
                windowPosition = ClickThruBlocker.GUILayoutWindow(baseWindowID, windowPosition, drawWindow, "Civilian Population UI");
            }
        }

        ToolbarControl toolbarControl2;
        internal const string MODID = "CivPop";
        internal const string MODNAME = "Civilian Population UI";

        private void InitToolbarButton()
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
                    ApplicationLauncher.AppScenes.ALWAYS,
                    GameDatabase.Instance.GetTexture("CivilianPopulation/GUI/CivPopIcon", false)
                );
            }
#endif
            if (toolbarControl2 == null)
            {
                GameObject gameObject = new GameObject();
                toolbarControl2 = gameObject.AddComponent<ToolbarControl>();
                toolbarControl2.AddToAllToolbars(windowToggle, windowToggle,
                    ApplicationLauncher.AppScenes.FLIGHT |
                    ApplicationLauncher.AppScenes.MAPVIEW |
                    ApplicationLauncher.AppScenes.TRACKSTATION |
                    ApplicationLauncher.AppScenes.SPACECENTER |
                    ApplicationLauncher.AppScenes.SPH |
                    ApplicationLauncher.AppScenes.VAB,
                    MODID,
                    "civPopBtn2",
                    "CivilianPopulation/PluginData/GUI/CivPopIcon-38",
                    "CivilianPopulation/PluginData/GUI/CivPopIcon-24",
                    MODNAME
                );
            }
        }

        private void windowToggle()
        {
            windowShown = !windowShown;
        }

        string FormatTime(double time)
        {
            int days = (int)(time / (6f * 3600f));
            int years = days / 426;
            days = days - years * 426;
            int seconds = (int)(time % (6f * 3600));
            int minutes = seconds / 60;
            int hours = minutes / 60;
            minutes -= hours * 60;

            StringBuilder s = new StringBuilder();
            if (years > 0)
                s.Append(years + " years, ");
            if (days > 0)
                s.Append(days + " days, ");
            s.Append(hours.ToString("D2") + ":" + minutes.ToString("D2"));

            return s.ToString();
        }
        private void drawWindow(int windowId)
        {
            GUILayout.BeginVertical();
            scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Width(500), GUILayout.Height(300));

            GUILayout.BeginHorizontal();
            GUILayout.Label("Get universal time : " + FormatTime(Planetarium.GetUniversalTime()));
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label("Time until taxes : " + FormatTime(core.getTimeUntilTaxes()));
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
            UnityEngine.GUI.DragWindow();
        }

#if false
        private void onAppLauncherDestroyed()
        {
            if (ApplicationLauncher.Instance != null && button != null)
            {
                ApplicationLauncher.Instance.RemoveApplication(button);
            }
        }
#endif
        private void OnDestroy()
        {
            if (toolbarControl2 != null)
            {
                toolbarControl2.OnDestroy();
                GameObject.Destroy(toolbarControl2);
                toolbarControl2 = null;

            }
        }


    }
}