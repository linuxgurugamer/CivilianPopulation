using System;
using UnityEngine;

namespace CivilianPopulation.GUI
{
    public class CrewPanel
    {
        public CrewPanel()
        {
        }

        public void draw()
        {
            GUILayout.BeginVertical();
            GUILayout.Label("Crew window.");

            string[] headers = {
                "Name",
                "Trait",
                "Location",
                "Gender",
                "Age",
                "Childbirth"
            };

            KerbalRoster kspRoster = HighLogic.CurrentGame.CrewRoster;
            string[,] data = new string[HighLogic.CurrentGame.CrewRoster.Count, 6];

            int i = 0;
            foreach (ProtoCrewMember crew in HighLogic.CurrentGame.CrewRoster.Crew)
            {
                data[i, 0] = crew.name;
                data[i, 1] = crew.trait;
                data[i, 2] = "?"; // Location
                data[i, 3] = crew.gender.displayDescription(); 
                data[i, 4] = "?"; // Age
                data[i, 5] = "?"; // Childbirth
                i++;
            }
            Grid grid = new Grid(headers, data);
            grid.draw();

            GUILayout.EndVertical();
        }
    }
}
