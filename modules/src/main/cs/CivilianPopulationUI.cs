using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using KSP;
using KSP.UI.Screens;

namespace CivilianPopulation
{
    [KSPScenario(ScenarioCreationOptions.AddToAllGames, GameScenes.FLIGHT, GameScenes.TRACKSTATION, GameScenes.SPACECENTER, GameScenes.EDITOR)]
    public class CivilianPopulationUI : ScenarioModule
    {
        private static ApplicationLauncherButton button = null;
        private static bool windowShown = false;
        private static Rect windowPosition = new Rect(Screen.width / 2 - 250, Screen.height / 2 - 250, 500, 300);

        public void Start()
        {
            GameEvents.onGUIApplicationLauncherReady.Add(onAppLauncherReady);//when AppLauncher can take apps, give it OnAppLauncherReady (mine)
            GameEvents.onGUIApplicationLauncherDestroyed.Add(onAppLauncherDestroyed);//Not sure what this does
        }

        public void OnGUI()
        {
            if (windowShown)
            {
                windowPosition = GUILayout.Window(0, windowPosition, drawWindow, "Civilian Population");
            }
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
            foreach (CivilianVessel vessel in getVessels())
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label("Name:  " + vessel.getName() + "( isPopulated : " + vessel.isPopulated() + ")");
                GUILayout.EndHorizontal();
                foreach (ProtoCrewMember crew in vessel.getCrew())
                { 
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("|-> Crew ToString........:  " + crew.ToString());
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("|-> Crew name............:  " + crew.name);
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("|-> Crew gender..........:  " + crew.gender);
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("|-> Crew trait...........:  " + crew.trait);
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("|-> Crew experienceLevel.:  " + crew.experienceLevel);
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("|-> Crew experience......:  " + crew.experience);
                    GUILayout.EndHorizontal();
                }
            }

            GUILayout.BeginHorizontal();
            //GUILayout.FlexibleSpace();
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

        private void PopulationManagementGUI(int windowId)
        {
            string activeVesselName = FlightGlobals.ActiveVessel.vesselName;
            string activeVesselStatus = FlightGlobals.ActiveVessel.situation.ToString();
            string activeSoI = FlightGlobals.currentMainBody.name;
            if (activeVesselName != null)
                activeVesselName = FlightGlobals.ActiveVessel.vesselName;
            else
                activeVesselName = "[Could not find name]";
            int numCivilians = 0;
            List<ProtoCrewMember> listCivilians = new List<ProtoCrewMember>();
            foreach (ProtoCrewMember crewMember in FlightGlobals.ActiveVessel.GetVesselCrew())
            {
                if (crewMember.trait == "Civilian")
                {
                    listCivilians.Add(crewMember);
                    numCivilians++;
                }
            }


            //begin GUI
            GUILayout.BeginVertical();

            //row for ship information
            GUILayout.BeginHorizontal();
            {
                GUILayout.Label("Vessel Name:  " + activeVesselName);
                GUILayout.Label("Status:  " + activeVesselStatus);
                GUILayout.Label("SoI:  " + activeSoI);
            }
            GUILayout.EndHorizontal();

            //row for civilian information
            GUILayout.BeginHorizontal();
            {
                /*
                GUILayout.BeginVertical();
                if (GUILayout.Button("Show Civilian Info"))
                {
                    showPopInfo = !showPopInfo;
                    Debug.Log(debuggingClass.modName + "Civilian Info button pressed. New value:  " + showPopInfo);
                }
                if (showPopInfo)
                {
                    if (listCivilians.FirstOrDefault() != null)
                    {
                        foreach (ProtoCrewMember crewMember in listCivilians)
                        {
                            GUILayout.BeginHorizontal();
                            {
                                GUILayout.Label(crewMember.trait);
                                GUILayout.Label(crewMember.name);
                                if (GUILayout.Button("EVA"))
                                {
                                    Debug.Log(debuggingClass.modName + "User pressed button to initiate EVA of " + crewMember.name);
                                    FlightEVA.SpawnEVA(crewMember.KerbalRef);
                                }
                                //if(GUILayout.Button("Transfer")){
                                //  Debug.Log (debuggingClass.modName + "User pressed button to initiated transfer of " + crewMember.name);
                                //  CrewTransfer.Create(crewMember.seat.part,crewMember);
                                //}Transfer disabled for now; need to figure out how to use highlighting from ShipManifest mod...
                            }
                            GUILayout.EndHorizontal();
                        }
                    }
                }
                */
                GUILayout.EndVertical();
                GUILayout.Label("Civilians:  " + numCivilians);
                GUILayout.Label("Crew Capacity:  " + FlightGlobals.ActiveVessel.GetCrewCount() + "/" + FlightGlobals.ActiveVessel.GetCrewCapacity());
            }
            GUILayout.EndHorizontal();

            //row for close button
            GUILayout.BeginHorizontal();
            {
                //GUILayout.FlexibleSpace();
                //if (GUILayout.Button("Close this Window", GUILayout.Width(200f)))
                //{
                //    Debug.Log(debuggingClass.modName + "Closing CivPopGUI window");
                //    OnToggleFalse();
                //}
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
            }
            GUILayout.EndVertical();
            GUI.DragWindow();
        }
    }
}
