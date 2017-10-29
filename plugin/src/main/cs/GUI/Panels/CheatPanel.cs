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
            GUILayout.EndVertical();

        }
    }
}
