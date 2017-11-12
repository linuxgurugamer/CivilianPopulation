using System;
using System.Collections.Generic;
using CivilianPopulation.Domain;
using UnityEngine;

namespace CivilianPopulation.GUI
{
    public class CrewPanel
    {
        private Grid grid;
        private TimeFormatter formatter;

        private CivilianKerbalRoster roster;
        private double currentDate;

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
            this.formatter = new TimeFormatter();
        }

        public void setRoster(CivilianKerbalRoster roster)
        {
            this.roster = roster;
        }

        public void setCurrentDate(double currentDate)
        {
            this.currentDate = currentDate;
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
                data[i, 4] = getAge(crew.name);
                data[i, 5] = getChildbirth(crew.name);
                i++;
            }

            GUILayout.BeginVertical();
            grid.setData(data);
            grid.draw();
            GUILayout.EndVertical();
        }

        private string getAge(string name)
        {
            string res = "?";
            if (roster.exists(name)) 
            {
                CivilianKerbal kerbal = roster.get(name);
                res = formatter.format(currentDate - kerbal.getBirthDate(), TimeFormat.AGE);
            }
            return res;
        }

        private string getChildbirth(string name)
        {
            string res = "-";
            if (roster.exists(name))
            {
                CivilianKerbal kerbal = roster.get(name);
                if (kerbal.getExpectingBirthAt() > -1)
                {
                    res = formatter.format(kerbal.getExpectingBirthAt() - currentDate);
                }
            }
            return res;
        }
    }
}
