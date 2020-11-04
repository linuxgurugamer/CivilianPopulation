using CivilianPopulation.Domain.Services;
using UnityEngine;

namespace CivilianPopulation.GUI
{
#if DEBUG
    public class CheatPanel
    {
        private CivPopServiceRent rent;

        public CheatPanel(CivPopServiceRent rent)
        {
            this.rent = rent;
        }

        public void draw()
        {
            GUILayout.BeginVertical();
            if (Funding.Instance != null && GUILayout.Button("Funds", GUILayout.Width(100f)))
            {
                Funding.Instance.AddFunds(100000f, TransactionReasons.Cheating);
            }
            if (ResearchAndDevelopment.Instance != null && GUILayout.Button("Science", GUILayout.Width(100f)))
            {
                ResearchAndDevelopment.Instance.AddScience(100, TransactionReasons.Cheating);
            }
            if (GUILayout.Button("Spawn", GUILayout.Width(100f)))
            {
                spawn();
            }

            GUILayout.BeginHorizontal();
            GUILayout.Label("Rent per civilian per day : ");
            string newRent = GUILayout.TextField(rent.GetRent().ToString());
            GUILayout.EndHorizontal();

            GUILayout.EndVertical();

            if (!newRent.Equals(rent.GetRent().ToString()))
            {
                long newValue;
                bool isLong = long.TryParse(newRent, out newValue);
                if (isLong)
                {
                    rent.SetRent(newValue);
                }
            }
        }

        private ProtoCrewMember spawn()
        {
            KerbalRoster kspRoster = HighLogic.CurrentGame.CrewRoster;
            ProtoCrewMember newKerbal = kspRoster.GetNewKerbal(ProtoCrewMember.KerbalType.Crew);
            foreach (Vessel vessel in FlightGlobals.Vessels)
            {
                if (vessel != null)
                {
                    Log.Info(vessel.GetName());
                    if (vessel.GetName().Equals("Ground Base - Laythe"))
                    {
                        Log.Info("Adding crew : " + newKerbal.name + " to vessel");
                        Part part = vessel.parts.Find(p => p.CrewCapacity > p.protoModuleCrew.Count);
                        if (part != null)
                        {
                            Log.Info("Adding crew : " + newKerbal.name + " to part");
                            part.AddCrewmember(newKerbal);
                        }
                    }
                }
            }
            return newKerbal;
        }
#if false
        private void log(string message)
        {
            Debug.Log(this.GetType().Name + " - " + message);
        }
#endif
    }
#endif
    }
