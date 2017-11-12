using System;
using UnityEngine;

namespace CivilianPopulation.GUI
{
    public class CheatPanel
    {
        public CheatPanel()
        {
        }

        public void draw()
        {
            GUILayout.BeginVertical();
            if (GUILayout.Button("Funds", GUILayout.Width(100f)))
            {
                Funding.Instance.AddFunds(100000, TransactionReasons.Cheating);
            }
            if (GUILayout.Button("Science", GUILayout.Width(100f)))
            {
                ResearchAndDevelopment.Instance.AddScience(100, TransactionReasons.Cheating);
            }
            if (GUILayout.Button("Spawn", GUILayout.Width(100f)))
            {
                spawn();
            }
            GUILayout.EndVertical();
        }

        private ProtoCrewMember spawn()
        {
            KerbalRoster kspRoster = HighLogic.CurrentGame.CrewRoster;
            ProtoCrewMember newKerbal = kspRoster.GetNewKerbal(ProtoCrewMember.KerbalType.Crew);
            foreach (Vessel vessel in FlightGlobals.Vessels)
            {
                log(vessel.GetName()); 
                if (vessel.GetName().Equals("Ground Base - Laythe"))
                {
                    log("Adding crew : " + newKerbal.name + " to vessel");
                    Part part = vessel.parts.Find(p => p.CrewCapacity > p.protoModuleCrew.Count);
                    if (part != null)
                    {
                        log("Adding crew : " + newKerbal.name + " to part");
                        part.AddCrewmember(newKerbal);
                    }
                }
            }
            return newKerbal;
        }

        private void log(string message)
        {
            Debug.Log(this.GetType().Name + " - " + message);
        }
    }
}
