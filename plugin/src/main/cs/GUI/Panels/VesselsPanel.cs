using System;
using System.Collections.Generic;
using System.Linq;
using CivilianPopulation.Domain;
using UnityEngine;

namespace CivilianPopulation.GUI
{
    public class VesselsPanel
    {
        private TimeFormatter formatter;

        private double currentDate;
        private List<CivilianVessel> vessels;

        public VesselsPanel(double currentDate, List<CivilianVessel> vessels)
        {
            this.currentDate = currentDate;
            this.vessels = vessels;

            this.formatter = new TimeFormatter();
        }

        public void draw()
        {
            GUILayout.BeginVertical();
            foreach (CivilianVessel vessel in vessels)
            {
                if (vessel.getCrewCount() > 0 || vessel.getHousingCapacity() > 0)
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Label(getVesselStatus(vessel));
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    GUILayout.Label("M : " + vessel.getMales().Count()
                               + " - F : " + vessel.getFemales().Count()
                               + " (" + vessel.getFemales().Where(kerbal => kerbal.getExpectingBirthAt() > 0).Count() + ")");
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    GUILayout.Label("  Housing capacity : " + vessel.getHousingCapacity());
                    GUILayout.EndHorizontal();

                    if (vessel.getMission() != null)
                    {
                        GUILayout.BeginHorizontal();
                        GUILayout.Label("  Mission arrival : " + formatter.format(vessel.getMission().getEndDate() - currentDate));
                        GUILayout.EndHorizontal();
                    }

                    foreach (CivilianKerbal female in vessel.getFemales().Where(kerbal => kerbal.getExpectingBirthAt() > 0))
                    {
                        GUILayout.BeginHorizontal();
                        GUILayout.Label("  " + female.getName() + " will give birth in " + formatter.format(female.getExpectingBirthAt() - currentDate));
                        GUILayout.EndHorizontal();
                    }
                }
            }
            GUILayout.EndVertical();
        }

        private string getVesselStatus(CivilianVessel vessel)
        {
            string res = vessel.getName();
            res += " - ";
            if (vessel.isOrbiting())
            {
                res += "in orbit around ";
            }
            else
            {
                res += "on surface of ";
            }
            res += vessel.getBody().getName();
            res += " - ";
            res += vessel.getCivilianCount() + " civilian";
            if (vessel.getCivilianCount() > 1)
            {
                res += "s";
            }
            return res;
        }

    }
}
