using System;
using System.Collections.Generic;
using UnityEngine;

namespace CivilianPopulation.GUI
{
    public class CrewPanel
    {
        private Grid grid;

        public CrewPanel()
        {
            string[] headers = {
                "Name",
                "Trait",
                "Location",
                "Gender",
                "Age",
                "Childbirth"
            };
            grid = new Grid();
            grid.setHeaders(headers);

        }

        public void draw()
        {
            Dictionary<string, string> crewVessels = new Dictionary<string, string>();
            foreach (Vessel vessel in FlightGlobals.Vessels)
            {
                foreach (ProtoCrewMember crew in vessel.GetVesselCrew())
                {
                    crewVessels.Add(crew.name, vessel.GetName());
                }
            }

            KerbalRoster kspRoster = HighLogic.CurrentGame.CrewRoster;
            string[,] data = new string[HighLogic.CurrentGame.CrewRoster.Count, 6];

            int i = 0;
            foreach (ProtoCrewMember crew in HighLogic.CurrentGame.CrewRoster.Crew)
            {
                data[i, 0] = crew.name;
                data[i, 1] = crew.trait;
                data[i, 2] = "KSC";
                if (crewVessels.ContainsKey(crew.name))
                {
                    data[i, 2] = crewVessels[crew.name];
                }
                data[i, 3] = crew.gender.displayDescription(); 
                data[i, 4] = "?"; // Age
                data[i, 5] = "?"; // Childbirth
                i++;
            }

            GUILayout.BeginVertical();
            grid.setData(data);
            grid.draw();
            GUILayout.EndVertical();
        }
    }
}
