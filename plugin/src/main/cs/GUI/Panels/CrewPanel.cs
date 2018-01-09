using System;
using System.Collections.Generic;
using CivilianPopulation.Domain;
using CivilianPopulation.Domain.Repository;
using UnityEngine;

namespace CivilianPopulation.GUI
{
    public class CrewPanel
    {
        private Grid grid;
        private TimeFormatter formatter;

        private CivPopRepository repo;
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

        public void setRepository(CivPopRepository repo)
        {
            this.repo = repo;
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
            if (repo.KerbalExists(name)) 
            {
                CivPopKerbal kerbal = repo.GetKerbal(name);
                res = formatter.format(currentDate - kerbal.GetBirthdate(), TimeFormat.AGE);
            }
            return res;
        }

        private string getChildbirth(string name)
        {
            string res = "-";
            if (repo.KerbalExists(name)) 
            {
                CivPopKerbal kerbal = repo.GetKerbal(name);
                if (kerbal.GetExpectingBirthAt() > -1)
                {
                    res = formatter.format(kerbal.GetExpectingBirthAt() - currentDate);
                }
            }
            return res;
        }
    }
}
