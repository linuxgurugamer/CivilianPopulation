using CivilianPopulation.Domain.Repository;
using System.Linq;
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
            var i = 0;
            var data = new string[repo.GetRoster().Count(), 6];
            foreach (var crew in repo.GetRoster().OrderBy(crew => crew.GetName()))
            {
                ProtoCrewMember kCrew = null;
                foreach (var current in HighLogic.CurrentGame.CrewRoster.Crew)
                {
                    if (current.name == crew.GetName())
                    {
                        kCrew = current;
                        break;
                    }
                }

                data[i, 0] = crew.GetName();
                data[i, 1] = getTrait(kCrew);
                data[i, 2] = getStatus(crew);
                data[i, 3] = getGender(crew);
                data[i, 4] = getAge(crew);
                data[i, 5] = getChildbirth(crew);
                i++;
            }
            GUILayout.BeginVertical();
            grid.setData(data);
            grid.draw();
            GUILayout.EndVertical();
        }

        private string getTrait(ProtoCrewMember kCrew)
        {
            var res = "Civilian";
            if (kCrew != null)
            {
                res = kCrew.trait;
            }
            return res;
        }

        private string getStatus(CivPopKerbal crew)
        {
            var res = "KSC";
            foreach (var vessel in FlightGlobals.Vessels)
            {
                if (vessel != null && vessel.id.ToString() == crew.GetVesselId())
                {
                    res = vessel.GetName();
                    break;
                }
            }
            return res;
        }

        private string getGender(CivPopKerbal crew)
        {
            var gender = ProtoCrewMember.Gender.Male;
            if (crew.GetGender().Equals(CivPopKerbalGender.FEMALE))
            {
                gender = ProtoCrewMember.Gender.Female;
            }
            return gender.displayDescription();
        }

        private string getAge(CivPopKerbal crew)
        {
            var res = formatter.format(currentDate - crew.GetBirthdate(), TimeFormat.AGE);
            if (crew.IsDead())
            {
                res = res + " - Dead";
            }
            return res;
        }

        private string getChildbirth(CivPopKerbal crew)
        {
            string res = "-";
            if (crew.GetExpectingBirthAt() > -1)
            {
                res = formatter.format(crew.GetExpectingBirthAt() - currentDate);
            }
            return res;
        }
    }
}
