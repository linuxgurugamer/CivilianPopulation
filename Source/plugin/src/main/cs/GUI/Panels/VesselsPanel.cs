using CivilianPopulation.Domain.Repository;
using System.Linq;
using UnityEngine;

namespace CivilianPopulation.GUI
{
    public class VesselsPanel
    {
        private TimeFormatter formatter;

        private double currentDate;
        private CivPopRepository repo;

        public VesselsPanel()
        {
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
            GUILayout.BeginVertical();
            foreach (CivPopVessel vessel in repo.GetVessels().OrderBy(v => v.GetId()))
            {
                if (vessel.GetCapacity() > 0)
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Label(getVesselStatus(vessel));
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    GUILayout.Label("M : " + repo.GetLivingRosterForVessel(vessel.GetId()).Count(k => k.GetGender() == CivPopKerbalGender.MALE)
                               + " - F : " + repo.GetLivingRosterForVessel(vessel.GetId()).Count(k => k.GetGender() == CivPopKerbalGender.FEMALE)
                                    + " (" + repo.GetLivingRosterForVessel(vessel.GetId()).Count(k => k.GetExpectingBirthAt() > 0) + ")");
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    GUILayout.Label("  Housing capacity : " + vessel.GetCapacity());
                    GUILayout.EndHorizontal();

                    if (vessel.GetMissionArrival() > 0)
                    {
                        GUILayout.BeginHorizontal();
                        GUILayout.Label("  Mission arrival : " + formatter.format(vessel.GetMissionArrival() - currentDate));
                        GUILayout.EndHorizontal();
                    }

                    foreach (CivPopKerbal female in repo.GetLivingRosterForVessel(vessel.GetId()).Where(k => k.GetExpectingBirthAt() > 0))
                    {
                        GUILayout.BeginHorizontal();
                        GUILayout.Label("  " + female.GetName() + " will give birth in " + formatter.format(female.GetExpectingBirthAt() - currentDate));
                        GUILayout.EndHorizontal();
                    }
                }
            }
            GUILayout.EndVertical();
        }

        private string getVesselStatus(CivPopVessel vessel)
        {
            var kVessel = FlightGlobals.Vessels
                .Find(v => v.id.ToString().Equals(vessel.GetId()));

            var res = kVessel.GetName();
            res += " - ";
            if (!kVessel.LandedOrSplashed)
            {
                res += "in orbit around ";
            }
            else
            {
                res += "on surface of ";
            }
            res += kVessel.mainBody.name;
            var civCount = repo
                            .GetLivingRosterForVessel(vessel.GetId())
                            .Count(kerbal => kerbal.IsCivilian());
            res += " - ";
            res += civCount + " civilian";
            if (civCount > 1)
            {
                res += "s";
            }
            return res;
        }

        private void log(string message)
        {
            Debug.Log(this.GetType().Name + " - " + message);
        }
    }
}
