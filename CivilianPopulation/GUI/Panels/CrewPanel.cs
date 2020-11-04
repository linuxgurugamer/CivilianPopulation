

using System;
using System.Collections;
using System.Collections.Generic;
using CivilianPopulation.Domain.Repository;
using System.Linq;
using UnityEngine;
using Smooth.Collections;

namespace CivilianPopulation.GUI
{
    public class CrewPanel
    {
        private Grid grid;
        private TimeFormatter formatter;

        private CivPopRepository repo;
        private double currentDate;

        class StringComparer<TKey> : IComparer<string>
        {
            public int Compare(string x, string y)
            {
                return y.CompareTo(x);
            }
        }
        SortedList<string, CivPopKerbal> list = new SortedList<string, CivPopKerbal>();
        SortedList<string, CivPopKerbal> reverseList = new SortedList<string, CivPopKerbal>(new StringComparer<string>());

        IOrderedEnumerable<CivPopKerbal> sortedList = null;
        string lastSort = "";
        bool lastReverseSort = false;
        

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
            int[] headerWidth =
            {
                130,
                60,
                80,
                60,
                130,
                140
            };
            GUIStyle[] styles =
            {
                RegisterToolbar.DefGuiSkin.label,
                RegisterToolbar.labelCentered,
                RegisterToolbar.DefGuiSkin.label,
                RegisterToolbar.labelCentered,
                RegisterToolbar.DefGuiSkin.label,
                RegisterToolbar.DefGuiSkin.label
            };
            grid = new Grid();            
            grid.setHeaders(headers, headerWidth, styles);
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
            int cnt = 0;
            var data = new string[repo.GetRoster().Count(), 6];

            if (sortedList == null || lastSort != grid.GetCurrentSort || lastReverseSort != grid.GetReverseSort|| repo.GetRoster().Count() != sortedList.Count())
            {
                lastSort = grid.GetCurrentSort;
                lastReverseSort = grid.GetReverseSort;
                switch (lastSort)
                {
                    case "Name":
                        if (grid.GetReverseSort)
                            sortedList = repo.GetRoster().OrderByDescending(crew => crew.GetName());
                        else
                            sortedList = repo.GetRoster().OrderBy(crew => crew.GetName());
                        break;

                    case "Trait":
                        list.Clear();
                        reverseList.Clear();
                        foreach (var crew in repo.GetRoster())
                        {
                            foreach (var current in HighLogic.CurrentGame.CrewRoster.Crew)
                            {
                                if (current.name == crew.GetName())
                                {
                                    if (grid.GetReverseSort)
                                        reverseList.Add(getTrait(current) + cnt.ToString(), crew);
                                    else
                                        list.Add(getTrait(current) + cnt.ToString(), crew);
                                    cnt++;
                                    break;
                                }
                            }
                        }
                        if (grid.GetReverseSort)
                        {
                            cnt = 0;
                            foreach (var a in reverseList)
                                list.Add(cnt++.ToString(), a.Value);
                        }
                        sortedList = list.Values.OrderBy(key => 0);
                        break;
                    case "Location":
                        list.Clear();
                        reverseList.Clear();
                        foreach (var crew in repo.GetRoster())
                        {
                            foreach (var current in HighLogic.CurrentGame.CrewRoster.Crew)
                            {
                                if (current.name == crew.GetName())
                                {
                                    if (grid.GetReverseSort)
                                        reverseList.Add(getStatus(crew) + cnt.ToString(), crew);
                                    else
                                        list.Add(getStatus(crew) + cnt.ToString(), crew);
                                    cnt++;
                                    break;
                                }
                            }
                        }
                        if (grid.GetReverseSort)
                        {
                            cnt = 0;
                            foreach (var a in reverseList)
                                list.Add(cnt++.ToString(), a.Value);
                        }
                        sortedList = list.Values.OrderBy(key => 0);

                        break;
                    case "Gender":
                        list.Clear();
                        reverseList.Clear();
                        foreach (var crew in repo.GetRoster())
                        {
                            foreach (var current in HighLogic.CurrentGame.CrewRoster.Crew)
                            {
                                if (current.name == crew.GetName())
                                {
                                    if (grid.GetReverseSort)
                                        reverseList.Add(getGender(crew) + cnt.ToString(), crew);
                                    else
                                        list.Add(getGender(crew) + cnt.ToString(), crew);
                                    cnt++;
                                    break;
                                }
                            }
                        }
                        if (grid.GetReverseSort)
                        {
                            cnt = 0;
                            foreach (var a in reverseList)
                                list.Add(cnt++.ToString(), a.Value);
                        }
                        sortedList = list.Values.OrderBy(key => 0);

                        break;
                    case "Age":
                        if (grid.GetReverseSort)
                            sortedList = repo.GetRoster().OrderByDescending(crew => currentDate - crew.GetBirthdate());
                        else
                            sortedList = repo.GetRoster().OrderBy(crew => currentDate - crew.GetBirthdate());
                        break;

                    case "Childbirth":
                        if (grid.GetReverseSort)
                            sortedList = repo.GetRoster().OrderByDescending(crew => crew.GetExpectingBirthAt());
                        else
                            sortedList = repo.GetRoster().OrderBy(crew => crew.GetExpectingBirthAt());
                        break;
                    default:
                        if (grid.GetReverseSort)
                            sortedList = repo.GetRoster().OrderByDescending(crew => crew.GetName());
                        else
                            sortedList = repo.GetRoster().OrderBy(crew => crew.GetName());
                        break;

                }
            }
            //sortedList = repo.GetRoster().OrderBy(crew => crew.GetName());

            foreach (var crew in sortedList)
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
            string res = "       -";
            if (crew.GetExpectingBirthAt() > -1)
            {
                res = formatter.format(crew.GetExpectingBirthAt() - currentDate);
            }
            return res;
        }
    }
}
